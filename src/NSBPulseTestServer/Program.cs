using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSBPulse;
using NSBPulseMessages;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace NSBPulseTestServer
{
    class Program
    {
        private static IBus _bus;
        static void Main(string[] args)
        {
            var configure = Configure.With()
                //.Log4Net()
                .DefaultBuilder()
                .RunTimeoutManagerWithInMemoryPersistence()
                .MsmqSubscriptionStorage()
                .XmlSerializer()
                .MsmqTransport()
                .IsTransactional(true)
                .PurgeOnStartup(false);
            _bus = configure
                .UnicastBus()
                    .ImpersonateSender(false)
                    .LoadMessageHandlers()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            var pulseMessageHandler = configure.Builder.Build<PulseMessageSender>();
            var domainMessageSender = new DomainMessageSender();

            Console.WriteLine("\r\n\r\n\r\nNSB init done.");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("\r\n\r\n\r\nServer ready");

            var startup = new Startup();
            startup.Init();
            startup.Run();

            
            var key = Console.ReadKey(); 
            while (key.KeyChar != 'q' && key.KeyChar != 'Q')
            {
                if (key.KeyChar == '1')
                {
                    domainMessageSender.SendDomainMessage();
                }
                else
                {
                    pulseMessageHandler.SendHeartBeat();
                }
                key = Console.ReadKey(); 
            }
        }
    }

    public class Startup : IConfigureThisEndpoint, IWantToRunAtStartup, IWantCustomInitialization
    {
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Server Startup.Run");
            Console.ResetColor();
        }

        public void Stop()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Server Startup.Stop");
            Console.ResetColor();
        }

        public void Init()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Server Startup.Init");
            Console.ResetColor();

            
        }
    }

    /*
    public class PulseMessageSender : IHandleMessages<HeartBeatConfirm>
    {
        private IBus _bus;
        private ISubscriptionStorage _subscriptionStorage;

        private void CheckDependencies()
        {
            if( _subscriptionStorage == null)
                _subscriptionStorage = NServiceBus.Configure.Instance.Builder.Build<ISubscriptionStorage>();

            if( _bus == null)
                _bus = NServiceBus.Configure.Instance.Builder.Build<IBus>();
        }

        public void SendHeartBeat()
        {
            CheckDependencies();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test Server HeartBeat Challenge sent");
            Console.ResetColor();

            var messageTypes = FindAllMessages().Select(t => new MessageType(t)).ToArray();
            var subscriberAddressesForMessage =
                _subscriptionStorage.GetSubscriberAddressesForMessage(messageTypes);

            foreach (var address in subscriberAddressesForMessage)
            {
                _bus.Send(address, new HeartBeatChallenge() {Id = Guid.NewGuid(), TimeSent = DateTime.Now});
            }
        }

        public IEnumerable<Type> FindAllMessages()
        {
            var messageTypes = this.GetType()
                .Assembly.GetReferencedAssemblies()
                .Select(System.Reflection.Assembly.Load)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof (IMessage)))
                .ToList();

            return messageTypes;
        }

        public void Handle(HeartBeatConfirm message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test Server HeartBeat confirm recieved {0}, {1}", message.Id, message.TimeRecieved);
            Console.ResetColor();
        }
    }
    */
}
