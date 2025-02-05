using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pets.Events;
using Pets.Interfaces.Events;

namespace Pets.EventHandlers
{
    public class PetUpdatedEventHandler : IPetEventHandler
    {
        private readonly ILogger<PetUpdatedEventHandler> _logger;

        public PetUpdatedEventHandler(ILogger<PetUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(IEvent petEvent)
        {
            return petEvent is PetUpdatedEvent;
        }

        public async Task HandleAsync(IEvent petEvent)
        {
            if (petEvent is PetUpdatedEvent updatedEvent)
            {
                _logger.LogInformation("PetUpdatedEvent: Pet {PetId} named '{Name}' was updated at {OccurredOn}.",
                    updatedEvent.PetId, updatedEvent.Name, updatedEvent.OccurredOn);
            }
            await Task.CompletedTask;
        }
    }
}
