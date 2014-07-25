using System;
using NSBPulseTestMessages;
using NServiceBus;

namespace NSBPulseTestServer
{
    public class DomainMessageSender
    {
        private IBus _bus;

        private void CheckDependencies()
        {
            if (_bus == null)
                _bus = NServiceBus.Configure.Instance.Builder.Build<IBus>();
        }

        public void SendDomainMessage()
        {
            CheckDependencies();

            var message = new SomeDomainRelatedMessage();
            _bus.Publish(message);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test Server Domain Message sent {0}", message.Message);
            Console.ResetColor();
        }
    }
}