using AutoMapper;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;

namespace ProdavnicaTehnikeBekend.Profiles
{
    public class ZaposleniProfile : Profile
    {
        public ZaposleniProfile()
        {
     
            CreateMap<ZaposleniCreateDto, Zaposleni>().ReverseMap();
            CreateMap<ZaposleniUpdateDto, Zaposleni>().ReverseMap();
            CreateMap<ZaposleniDto, Zaposleni>().ReverseMap();
        }
    }
}
