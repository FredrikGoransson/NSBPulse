using System;
using NSBPulseTestMessages;
using NServiceBus;

namespace NSBPulseTestClient
{
    public class DomainMessageHandler : IHandleMessages<SomeDomainRelatedMessage>
    {
        public void Handle(SomeDomainRelatedMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Test Client Domain Message recieved {0}", message.Message);
            Console.ResetColor();
        }
    }
}