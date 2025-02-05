using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pets.Events;
using Pets.Interfaces.Events;

namespace Pets.EventHandlers
{
    public class PetDeletedEventHandler : IPetEventHandler
    {
        private readonly ILogger<PetDeletedEventHandler> _logger;

        public PetDeletedEventHandler(ILogger<PetDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(IEvent petEvent)
        {
            return petEvent is PetDeletedEvent;
        }

        public async Task HandleAsync(IEvent petEvent)
        {
            if (petEvent is PetDeletedEvent deletedEvent)
            {
                _logger.LogInformation("PetDeletedEvent: Pet {PetId} named '{Name}' was deleted at {OccurredOn}.",
                    deletedEvent.PetId, deletedEvent.Name, deletedEvent.OccurredOn);
            }
            await Task.CompletedTask;
        }
    }
}
