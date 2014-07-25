using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;

namespace NSBPulse
{
    public class MessageTypeInvestigator
    {
        private IEnumerable<Type> _messageTypes;

        public IEnumerable<Type> MessageTypesInAppDomain
        {
            get { return FindAllMessages(); }
        }

        private IEnumerable<Type> FindAllMessages()
        {
            if (_messageTypes == null)
            {
                _messageTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetInterfaces().Any(i => i == typeof(IMessage)))
                    .ToList();
            }

            return _messageTypes;
        }

        public MessageTypeInvestigator()
        {
            FindAllMessages();
        }
    }
}