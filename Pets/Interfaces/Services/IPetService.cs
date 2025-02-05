using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.DTO;
using Pets.ViewModels;

namespace Pets.Interfaces.Services
{
    public interface IPetService
    {
        // Retrieve all pets.
        Task<IEnumerable<PetViewModel>> GetAllPetsAsync();

        // Retrieve a single pet by its identifier.
        Task<PetViewModel?> GetPetByIdAsync(Guid id);

        // Create a new pet from a DTO.
        Task<PetViewModel> CreatePetAsync(PetDto petDto);

        // Update an existing pet using its identifier and DTO.
        Task UpdatePetAsync(Guid id, PetDto petDto);

        // Delete a pet by its identifier.
        Task DeletePetAsync(Guid id);
    }
}
