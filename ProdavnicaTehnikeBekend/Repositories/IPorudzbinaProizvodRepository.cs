using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IPorudzbinaProizvodRepository
    {
        Task<List<PorudzbinaProizvod>> GetPorudzbinaProizvodi();
        Task<PorudzbinaProizvod> GetPorudzbinaProizvodById(int proizvodId, int porudzbinaId);
        Task<PorudzbinaProizvod> CreatePorudzbinaProizvod(PorudzbinaProizvod porudzbinaProizvod);
        Task<PorudzbinaProizvod> UpdatePorudzbinaProizvod(PorudzbinaProizvod porudzbinaProizvod);
        Task DeletePorudzbinaProizvod(int proizvodId, int porudzbinaId);
    }
}
