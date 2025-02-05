using AutoMapper;
using Pets.DTO;
using Pets.Events;
using Pets.Interfaces.Events;
using Pets.Interfaces.Repositories;
using Pets.Interfaces.Services;
using Pets.Models;
using Pets.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pets.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IMapper _mapper;

        public PetService(IPetRepository petRepository, IEventDispatcher eventDispatcher, IMapper mapper)
        {
            _petRepository = petRepository;
            _eventDispatcher = eventDispatcher;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PetViewModel>> GetAllPetsAsync()
        {
            var pets = await _petRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PetViewModel>>(pets);
        }

        public async Task<PetViewModel?> GetPetByIdAsync(Guid id)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            return pet != null ? _mapper.Map<PetViewModel>(pet) : null;
        }

        public async Task<PetViewModel> CreatePetAsync(PetDto petDto)
        {
            var pet = _mapper.Map<Pet>(petDto);
            pet.Id = Guid.NewGuid();

            var createdPet = await _petRepository.AddAsync(pet);

            await _eventDispatcher.DispatchAsync(new PetCreatedEvent(createdPet.Id, createdPet.Name, DateTime.UtcNow));

            return _mapper.Map<PetViewModel>(createdPet);
        }

        public async Task UpdatePetAsync(Guid id, PetDto petDto)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            // Update the pet with data from petDto.
            _mapper.Map(petDto, pet);
            pet.UpdatedAt = DateTime.UtcNow;

            await _petRepository.UpdateAsync(pet);

            await _eventDispatcher.DispatchAsync(new PetUpdatedEvent(pet.Id, pet.Name, DateTime.UtcNow));
        }

        public async Task DeletePetAsync(Guid id)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            await _petRepository.DeleteAsync(id);

            await _eventDispatcher.DispatchAsync(new PetDeletedEvent(pet.Id, pet.Name, DateTime.UtcNow));
        }
    }
}
