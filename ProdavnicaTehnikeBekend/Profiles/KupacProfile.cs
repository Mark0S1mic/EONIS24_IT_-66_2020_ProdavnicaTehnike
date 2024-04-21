using AutoMapper;
using ProdavnicaTehnikeBekend.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ProdavnicaTehnikeBekend.Models.DTOs;

namespace ProdavnicaTehnikeBekend.Profiles
{
    public class KupacProfile : Profile
    {
        public KupacProfile()
        {
            CreateMap<KupacDto, Kupac>().ReverseMap();
            CreateMap<KupacCreateDto, Kupac>();
            CreateMap<KupacUpdateDto, Kupac>().ReverseMap();
        }
    }
}
