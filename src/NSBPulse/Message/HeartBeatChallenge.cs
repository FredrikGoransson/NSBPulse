using System;
using NServiceBus;

namespace NSBPulse.Message
{
    [Serializable]
    public class HeartBeatChallenge : IMessage
    {
        public DateTime TimeSent { get; set; }
        public Guid Id { get; set; }
    }
}