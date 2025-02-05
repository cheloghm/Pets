using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pets.Events;
using Pets.Interfaces.Events;

namespace Pets.EventHandlers
{
    public class PetCreatedEventHandler : IPetEventHandler
    {
        private readonly ILogger<PetCreatedEventHandler> _logger;

        public PetCreatedEventHandler(ILogger<PetCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(IEvent petEvent)
        {
            return petEvent is PetCreatedEvent;
        }

        public async Task HandleAsync(IEvent petEvent)
        {
            if (petEvent is PetCreatedEvent createdEvent)
            {
                _logger.LogInformation("PetCreatedEvent: Pet {PetId} named '{Name}' was created at {OccurredOn}.",
                    createdEvent.PetId, createdEvent.Name, createdEvent.OccurredOn);
            }
            await Task.CompletedTask;
        }
    }
}
