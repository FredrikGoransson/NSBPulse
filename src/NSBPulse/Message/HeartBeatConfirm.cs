using System;
using NServiceBus;

namespace NSBPulse.Message
{
    [Serializable]
    public class HeartBeatConfirm : IMessage
    {
        public DateTime TimeRecieved { get; set; }
        public Guid Id { get; set; }
    }
}
