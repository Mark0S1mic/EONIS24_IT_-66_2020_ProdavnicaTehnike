namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IZaposleniRepository
    {

        Task<List<Zaposleni>> GetZaposleni();
        Task<Zaposleni> GetZaposleniById(int zaposleniId);
        Task<Zaposleni> CreateZaposleni(Zaposleni zaposleni);
        Task<Zaposleni> UpdateZaposleni(Zaposleni zaposleni);
        Task DeleteZaposleni(int zaposleniId);

    }
}
