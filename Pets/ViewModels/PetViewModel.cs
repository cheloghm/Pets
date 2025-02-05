using System;

namespace Pets.ViewModels
{
    public class PetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string AnimalType { get; set; }
        public string Description { get; set; }
    }
}
