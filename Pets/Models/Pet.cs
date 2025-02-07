using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pets.Models
{
    public class Pet
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Store Guid as string for readability.
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Pet name is required.")]
        [MaxLength(100, ErrorMessage = "Pet name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Animal type is required.")]
        [MaxLength(50, ErrorMessage = "Animal type cannot exceed 50 characters.")]
        public string AnimalType { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        // Audit fields (optional)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
