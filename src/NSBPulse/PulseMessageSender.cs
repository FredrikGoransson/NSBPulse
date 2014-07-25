using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSBPulseMessages;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace NSBPulse
{
    public class PulseMessageSender : IHandleMessages<HeartBeatConfirm>
    {
        private readonly object _padlock = new object();

        private IBus _bus;
        private ISubscriptionStorage _subscriptionStorage;
        private readonly IDictionary<Guid, Challenge> _sentChallenges;
        private IEnumerable<Type> _messageTypes;

        private void AddChallenge(HeartBeatChallenge message, Address address)
        {
            var challenge = new Challenge()
            {
                Address = address,
                Message = message
            };

            lock (_padlock)
            {
                _sentChallenges.Add(challenge.Message.Id, challenge);
            }
        }

        private void ConfirmChallenge(HeartBeatConfirm message)
        {
            lock (_padlock)
            {
                if (_sentChallenges.ContainsKey(message.Id))
                {
                    var challenge = _sentChallenges[message.Id];
                    _sentChallenges.Remove(message.Id);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("HeartBeat confirm recieved {0}, {1}", message.Id, message.TimeRecieved);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("HeartBeat confirm unrecognized {0}, {1}", message.Id, message.TimeRecieved);
                    Console.ResetColor();
                }
            }
        }

        public PulseMessageSender()
        {
            _sentChallenges = new Dictionary<Guid, Challenge>();
        }

        private void CheckDependencies()
        {
            if (_subscriptionStorage == null)
                _subscriptionStorage = NServiceBus.Configure.Instance.Builder.Build<ISubscriptionStorage>();

            if (_bus == null)
                _bus = NServiceBus.Configure.Instance.Builder.Build<IBus>();
        }

        public void SendHeartBeat()
        {
            CheckDependencies();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("HeartBeat Challenge sent");
            Console.ResetColor();

            var messageTypes = FindAllMessages().Select(t => new MessageType(t)).ToArray();
            var subscriberAddressesForMessage = _subscriptionStorage.GetSubscriberAddressesForMessage(messageTypes);

            foreach (var address in subscriberAddressesForMessage)
            {
                var message = new HeartBeatChallenge() {Id = Guid.NewGuid(), TimeSent = DateTime.Now};
                AddChallenge(message, address);
                _bus.Send(address, message);
            }
        }

        private IEnumerable<Type> FindAllMessages()
        {
            if (_messageTypes == null)
            {
                _messageTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetInterfaces().Any(i => i == typeof (IMessage)))
                    .ToList();
            }

            return _messageTypes;
        }

        public void Handle(HeartBeatConfirm message)
        {
            ConfirmChallenge(message);
        }

        private class Challenge
        {
            public Address Address { get; set; }
            public HeartBeatChallenge Message { get; set; }
        }
    }
}
