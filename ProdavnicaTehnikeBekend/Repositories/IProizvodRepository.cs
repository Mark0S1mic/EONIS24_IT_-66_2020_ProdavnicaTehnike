using ProdavnicaTehnikeBekend.Models;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IProizvodRepository
    {

        Task<List<Proizvod>> GetProizvodi();
        Task<Proizvod> GetProizvodById(int proizvodId);

        Task<Proizvod> GetProizvodByNaziv(string nazivProizvoda);
        Task<Proizvod> CreateProizvod(Proizvod proizvod);
        Task<Proizvod> UpdateProizvod(Proizvod proizvod);
        Task DeleteProizvod(int proizvodId);

    }
}
