using AutoMapper;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using ProdavnicaTehnikeBekend.Models.DTOs.PorudzbinaProizvodDto;

namespace ProdavnicaTehnikeBekend.Profiles
{
    public class PorudzbinaProizvodProfile : Profile
    {

        public PorudzbinaProizvodProfile()
        {
            CreateMap<PorudzbinaProizvodDto, PorudzbinaProizvod>().ReverseMap();
            CreateMap<PorudzbinaProizvodUpdateDto, PorudzbinaProizvod>().ReverseMap();
            CreateMap<PorudzbinaProizvodCreateDto, PorudzbinaProizvod>().ReverseMap();
        }
    }
}
