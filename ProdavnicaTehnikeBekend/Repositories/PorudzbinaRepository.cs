
using Microsoft.EntityFrameworkCore;
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
             .Include(p => p.Kupacs)
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
                Porudzbina? search = await _dbContext.Porudzbinas.Include(s => s.Kupacs).Include(p => p.Zaposlenis).
                    FirstOrDefaultAsync(w => w.PorudzbinaId == porudzbinaId);

                if (search == null)
                    throw new KeyNotFoundException();

                return search;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
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
                toUpdate.Kupacs = porudzbina.Kupacs;
                toUpdate.Zaposlenis = porudzbina.Zaposlenis;


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
