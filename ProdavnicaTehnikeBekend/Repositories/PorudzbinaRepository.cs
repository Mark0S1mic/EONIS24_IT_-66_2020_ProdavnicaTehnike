
using Microsoft.EntityFrameworkCore;
using ProdavnicaTehnikeBekend.Models;
namespace ProdavnicaTehnikeBekend.Repositories
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {

        ProdavnicaTehnikeContext _dbContext;

        public PorudzbinaRepository(ProdavnicaTehnikeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Porudzbina>> GetPorudzbine()
        {
            try
            {
                return await _dbContext.Porudzbinas
             .Include(p => p.Kupac)
             .Include(p => p.Zaposlenis)
             .ToListAsync();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Porudzbina> GetPorudzbinaById(int porudzbinaId)
        {
            try
            {
                Porudzbina porudzbina = await _dbContext.Porudzbinas
                    .Include(p => p.Kupac) // Uključujemo podatke o kupcu
                    .Include(p => p.Zaposlenis) // Uključujemo podatke o zaposlenima
                    .FirstOrDefaultAsync(p => p.PorudzbinaId == porudzbinaId);

                if (porudzbina == null)
                {
                    throw new KeyNotFoundException($"Porudžbina sa ID-jem {porudzbinaId} nije pronađena.");
                }

                return porudzbina;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom dohvatanja porudžbine sa ID-jem {porudzbinaId}: {ex.Message}", ex);
            }
        }
        public async Task<Porudzbina> CreatePorudzbina(Porudzbina porudzbina)
        {
            var createdPorudzbina = await _dbContext.AddAsync(porudzbina);

            await _dbContext.SaveChangesAsync();

            return createdPorudzbina.Entity;
        }

        public async Task<Porudzbina> UpdatePorudzbina(Porudzbina porudzbina)
        {
            try
            {
                var toUpdate = await _dbContext.Porudzbinas.FirstOrDefaultAsync(w => w.PorudzbinaId == porudzbina.PorudzbinaId);

                if (toUpdate == null)
                    throw new KeyNotFoundException();

                toUpdate.PorudzbinaId = porudzbina.PorudzbinaId;
                toUpdate.DatumPorudzbine = porudzbina.DatumPorudzbine;
                toUpdate.AdresaPorudzbine = porudzbina.AdresaPorudzbine;
                toUpdate.DatumPlacanja = porudzbina.DatumPlacanja;
                toUpdate.KupacId = porudzbina.KupacId;


                await _dbContext.SaveChangesAsync();

                return toUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeletePorudzbina(int porudzbinaId)
        {
            try
            {
                Porudzbina? search = await _dbContext.Porudzbinas.FirstOrDefaultAsync(w => w.PorudzbinaId == porudzbinaId);

                if (search == null)
                    throw new KeyNotFoundException();

                _dbContext.Porudzbinas.Remove(search);

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



    }
}
