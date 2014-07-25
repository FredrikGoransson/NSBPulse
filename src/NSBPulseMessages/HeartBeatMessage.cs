using System;
using NServiceBus;

namespace NSBPulseMessages
{
    [Serializable]
    public class HeartBeatChallenge : IMessage
    {
        public DateTime TimeSent { get; set; }
        public Guid Id { get; set; }
    }

    [Serializable]
    public class HeartBeatConfirm : IMessage
    {
        public DateTime TimeRecieved { get; set; }
        public Guid Id { get; set; }
    }
}
