using System;
using System.Collections.Generic;
using System.Linq;
using NSBPulse.Message;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace NSBPulse
{
    public class Challenge
    {
        public Address Address { get; set; }
        public HeartBeatChallenge Message { get; set; }
    }

    public class PulseMessageSender : IHandleMessages<HeartBeatConfirm>
    {

        private IBus _bus;
        private ISubscriptionStorage _subscriptionStorage;
        private MessageTypeInvestigator _messageTypeInvestigator;
        private PulseTracker _pulseTracker;

        public PulseMessageSender()
        {
        }

        private void CheckDependencies()
        {
            if (_subscriptionStorage == null)
                _subscriptionStorage = NServiceBus.Configure.Instance.Builder.Build<ISubscriptionStorage>();

            if (_bus == null)
                _bus = NServiceBus.Configure.Instance.Builder.Build<IBus>();

            if (_pulseTracker == null)
            {
                NServiceBus.Configure.Instance.Configurer.ConfigureComponent<PulseTracker>(DependencyLifecycle.SingleInstance);
                _pulseTracker = NServiceBus.Configure.Instance.Builder.Build<PulseTracker>();
            }

            if (_messageTypeInvestigator == null)
            {
                NServiceBus.Configure.Instance.Configurer.ConfigureComponent<MessageTypeInvestigator>(DependencyLifecycle.SingleInstance);
                _messageTypeInvestigator = NServiceBus.Configure.Instance.Builder.Build<MessageTypeInvestigator>();
            }
        }

        public void SendHeartBeat()
        {
            CheckDependencies();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("HeartBeat Challenge sent");
            Console.ResetColor();

            var messageTypes = _messageTypeInvestigator.MessageTypesInAppDomain.Select(t => new MessageType(t)).ToArray();
            var subscriberAddressesForMessage = _subscriptionStorage.GetSubscriberAddressesForMessage(messageTypes);

            foreach (var address in subscriberAddressesForMessage)
            {
                var message = new HeartBeatChallenge() {Id = Guid.NewGuid(), TimeSent = DateTime.Now};
                _pulseTracker.AddChallenge(message, address);
                _bus.Send(address, message);
            }
        }

        public void Handle(HeartBeatConfirm message)
        {
            _pulseTracker.ConfirmChallenge(message);
        }

    }
}
