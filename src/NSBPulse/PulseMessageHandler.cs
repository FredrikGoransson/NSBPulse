using System;
using NSBPulse.Message;
using NServiceBus;

namespace NSBPulse
{
    public class PulseMessageHandler : IHandleMessages<HeartBeatChallenge>
    {
        private IBus _bus;

        private void CheckDependencies()
        {
            if (_bus == null)
                _bus = NServiceBus.Configure.Instance.Builder.Build<IBus>();
        }

        public void Handle(HeartBeatChallenge message)
        {
            CheckDependencies();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test Client HeartBeat Challenge recieved {0}, {1}", message.Id, message.TimeSent);
            Console.ResetColor();

            _bus.Reply(new HeartBeatConfirm() { Id = message.Id, TimeRecieved = DateTime.Now });
        }
    }
}