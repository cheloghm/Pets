using System.ComponentModel.DataAnnotations;

namespace Pets.DTO
{
    public class PetDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Animal Type is required.")]
        [MaxLength(50, ErrorMessage = "Animal Type cannot exceed 50 characters.")]
        public string AnimalType { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
    }
}
