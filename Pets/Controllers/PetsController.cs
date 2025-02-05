using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.DTO;
using Pets.Interfaces.Services;
using Pets.ViewModels;

namespace Pets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        /// <summary>
        /// Retrieves all pets.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetViewModel>>> GetAll()
        {
            var pets = await _petService.GetAllPetsAsync();
            return Ok(pets);
        }

        /// <summary>
        /// Retrieves a single pet by its identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PetViewModel>> GetById(Guid id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                return NotFound(new { message = "Pet not found." });
            }
            return Ok(pet);
        }

        /// <summary>
        /// Creates a new pet.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PetViewModel>> Create([FromBody] PetDto petDto)
        {
            // Optionally, validate the DTO using your custom validators (if not handled by FluentValidation).
            var createdPet = await _petService.CreatePetAsync(petDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPet.Id }, createdPet);
        }

        /// <summary>
        /// Updates an existing pet.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PetDto petDto)
        {
            try
            {
                await _petService.UpdatePetAsync(id, petDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                // In case of an error (for example, pet not found), let the exception propagate.
                // It will be handled by our ExceptionHandlingMiddleware.
                throw;
            }
        }

        /// <summary>
        /// Deletes a pet.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _petService.DeletePetAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Exceptions are handled by the middleware.
                throw;
            }
        }
    }
}
