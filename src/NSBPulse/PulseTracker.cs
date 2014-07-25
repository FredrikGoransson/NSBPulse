using System;
using System.Collections.Generic;
using NSBPulse.Message;
using NServiceBus;

namespace NSBPulse
{
    public class PulseTracker
    {
        private readonly object _padlock = new object();
        private readonly IDictionary<Guid, Challenge> _sentChallenges;

        public PulseTracker()
        {
            _sentChallenges = new Dictionary<Guid, Challenge>();
        }

        public void AddChallenge(HeartBeatChallenge message, Address address)
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

        public void ConfirmChallenge(HeartBeatConfirm message)
        {
            lock (_padlock)
            {
                if (_sentChallenges.ContainsKey(message.Id))
                {
                    var challenge = _sentChallenges[message.Id];
                    _sentChallenges.Remove(message.Id);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("HeartBeat confirm recieved from {2} {0}, {1}", message.Id, message.TimeRecieved, challenge.Address);
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
    }
}