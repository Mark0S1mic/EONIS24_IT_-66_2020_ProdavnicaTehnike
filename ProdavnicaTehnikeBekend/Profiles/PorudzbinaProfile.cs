using AutoMapper;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;

namespace ProdavnicaTehnikeBekend.Profiles
{
    public class PorudzbinaProfile : Profile
    {
        public PorudzbinaProfile()
        {
            CreateMap<PorudzbinaDto, Porudzbina>().ReverseMap();
            CreateMap<PorudzbinaCreateDto, Porudzbina>().ReverseMap();
            CreateMap<PorudzbinaUpdateDto, Porudzbina>().ReverseMap();
        }
    }
}
