using MongoDB.Driver;
using Pets.Interfaces.Repositories;
using Pets.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly IMongoCollection<Pet> _petsCollection;

        public PetRepository(IMongoDatabase database)
        {
            // "Pets" is the collection name in MongoDB.
            _petsCollection = database.GetCollection<Pet>("Pets");
        }

        public async Task<IEnumerable<Pet>> GetAllAsync()
        {
            return await _petsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Pet?> GetByIdAsync(Guid id)
        {
            return await _petsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pet> AddAsync(Pet pet)
        {
            await _petsCollection.InsertOneAsync(pet);
            return pet;
        }

        public async Task UpdateAsync(Pet pet)
        {
            await _petsCollection.ReplaceOneAsync(p => p.Id == pet.Id, pet);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _petsCollection.DeleteOneAsync(p => p.Id == id);
        }
    }
}
