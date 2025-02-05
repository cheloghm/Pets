using AutoMapper;
using Pets.Models;
using Pets.ViewModels;

namespace Pets.Extensions
{
    public static class MappingExtensions
    {
        public static PetViewModel ToViewModel(this Pet pet, IMapper mapper)
        {
            return mapper.Map<PetViewModel>(pet);
        }
    }
}
