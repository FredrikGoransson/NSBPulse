using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using NSBPulseMessages;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace NSBPulseTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.With()
                //.Log4Net()
                .DefaultBuilder()
                .RunTimeoutManagerWithInMemoryPersistence()
                .MsmqSubscriptionStorage()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(true)
                    .PurgeOnStartup(false)
                .UnicastBus()
                    .ImpersonateSender(false)
                    .LoadMessageHandlers()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            Console.WriteLine("\r\n\r\n\r\nNSB init done.");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("\r\n\r\n\r\nClient ready");


            var startup = new Startup();
            startup.Init();
            startup.Run();

            var key = Console.ReadKey();
            while (key.KeyChar != 'q' && key.KeyChar != 'Q')
            {
                key = Console.ReadKey();
            }
        }
    }

    public class Startup : IConfigureThisEndpoint, IWantToRunAtStartup, IWantCustomInitialization
    {
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Client Startup.Run");
            Console.ResetColor();


            var subscriptionStorage = NServiceBus.Configure.Instance.Builder.Build<ISubscriptionStorage>();
            var subscriberAddressesForMessage = subscriptionStorage.GetSubscriberAddressesForMessage(new[] {new MessageType(typeof (HeartBeatChallenge))});

           

        }

        public void Stop()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Client Startup.Stop");
            Console.ResetColor();
        }

        public void Init()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Client Startup.Init");
            Console.ResetColor();
        }
    }
    /*
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

            _bus.Reply(new HeartBeatConfirm() {Id = message.Id, TimeRecieved = DateTime.Now});
        }
    }
     */
}