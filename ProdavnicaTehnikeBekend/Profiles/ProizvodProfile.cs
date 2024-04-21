using AutoMapper;
using ProdavnicaTehnikeBekend.Models.DTOs;

namespace ProdavnicaTehnikeBekend.Profiles
{
    public class ProizvodProfile : Profile
    {

        public  ProizvodProfile()
        {
            CreateMap<ProizvodDto, Proizvod>().ReverseMap();
            CreateMap<ProizvodCreateDto, Proizvod>().ReverseMap();
            CreateMap<ProizvodUpdateDto, Proizvod>().ReverseMap();
        }

    }
}
