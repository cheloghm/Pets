using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.Interfaces.Events;

namespace Pets.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IEnumerable<IPetEventHandler> _handlers;

        public EventDispatcher(IEnumerable<IPetEventHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task DispatchAsync(IEvent petEvent)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(petEvent))
                {
                    await handler.HandleAsync(petEvent);
                }
            }
        }
    }
}
