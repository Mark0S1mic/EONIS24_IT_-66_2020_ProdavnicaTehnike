using System.Collections.Generic;
using System.Threading.Tasks;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IKupacRepository
    {
        Task<List<Kupac>> GetKupci();
        Task<Kupac> GetKupacById(int kupacId);

        Task<Kupac> GetKupacByKorisnickoIme(string korisnickoImeKupca);
        Task<Kupac> CreateKupac(Kupac kupac);
        Task<Kupac> UpdateKupac( Kupac kupac);
        Task DeleteKupac(int kupacId);
    }
}
