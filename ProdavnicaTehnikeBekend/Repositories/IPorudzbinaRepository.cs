using ProdavnicaTehnikeBekend.Models;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IPorudzbinaRepository
    {

        Task<List<Porudzbina>> GetPorudzbine();
        Task<Porudzbina> GetPorudzbinaById(int porudzbinaId);
        Task<Porudzbina> CreatePorudzbina(Porudzbina porudzbina);
        Task<Porudzbina> UpdatePorudzbina(Porudzbina porudzbina);
        Task DeletePorudzbina(int porudzbinaId);

    }
}
