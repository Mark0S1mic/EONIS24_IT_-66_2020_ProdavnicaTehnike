namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IProizvodRepository
    {

        Task<List<Proizvod>> GetProizvodi();
        Task<Proizvod> GetProizvodById(int proizvodId);
        Task<Proizvod> CreateProizvod(Proizvod proizvod);
        Task<Proizvod> UpdateProizvod(Proizvod proizvod);
        Task DeleteProizvod(int proizvodId);

    }
}
