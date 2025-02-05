using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.Models;

namespace Pets.Interfaces.Repositories
{
    public interface IPetRepository
    {
        // Retrieve all pets.
        Task<IEnumerable<Pet>> GetAllAsync();

        // Retrieve a pet by its unique identifier.
        Task<Pet?> GetByIdAsync(Guid id);

        // Add a new pet.
        Task<Pet> AddAsync(Pet pet);

        // Update an existing pet.
        Task UpdateAsync(Pet pet);

        // Delete a pet.
        Task DeleteAsync(Guid id);
    }
}
