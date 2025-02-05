using AutoMapper;
using Pets.DTO;
using Pets.Models;
using Pets.ViewModels;

namespace Pets.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from PetDto to Pet.
            CreateMap<PetDto, Pet>();

            // Map from Pet to PetViewModel.
            CreateMap<Pet, PetViewModel>();
        }
    }
}
