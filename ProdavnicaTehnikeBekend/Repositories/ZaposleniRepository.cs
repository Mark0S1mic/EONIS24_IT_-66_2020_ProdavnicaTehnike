
using Microsoft.EntityFrameworkCore;
using ProdavnicaTehnikeBekend.Models;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public class ZaposleniRepository : IZaposleniRepository
    {

        ProdavnicaTehnikeContext _dbContext;
        private readonly HashingService _hashingService;
        public ZaposleniRepository(ProdavnicaTehnikeContext dbContext)
        {
            _dbContext = dbContext;
            _hashingService = new HashingService();
        }

        public async Task<List<Zaposleni>> GetZaposleni()
        {
            try
            {
                return await _dbContext.Zaposlenis.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Zaposleni> GetZaposleniById(int zaposleniId)
        {
            try
            {
                Zaposleni? search = await _dbContext.Zaposlenis.FirstOrDefaultAsync(z => z.ZaposleniId == zaposleniId);

                if (search == null)
                    throw new KeyNotFoundException();

                return search;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Zaposleni> CreateZaposleni(Zaposleni zaposleni)
        {

            zaposleni.SifraZaposlenog = _hashingService.HashPassword(zaposleni.SifraZaposlenog);
            var createdZaposleni = await _dbContext.AddAsync(zaposleni);

            await _dbContext.SaveChangesAsync();

            return createdZaposleni.Entity;
        }

        public async Task<Zaposleni> UpdateZaposleni(Zaposleni zaposleni)
        {
            try
            {
                var toUpdate = await _dbContext.Zaposlenis.FirstOrDefaultAsync(z => z.ZaposleniId == zaposleni.ZaposleniId);

                if (toUpdate == null)
                    throw new KeyNotFoundException();

                toUpdate.ZaposleniId = zaposleni.ZaposleniId;
                toUpdate.KorisnickoImeZaposlenog = zaposleni.KorisnickoImeZaposlenog;
                toUpdate.SifraZaposlenog = zaposleni.SifraZaposlenog;
                toUpdate.KontaktZaposlenog = zaposleni.KontaktZaposlenog;
             

                await _dbContext.SaveChangesAsync();

                return toUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteZaposleni(int zaposleniId)
        {
            try
            {
                Zaposleni? search = await _dbContext.Zaposlenis.FirstOrDefaultAsync(z => z.ZaposleniId == zaposleniId);

                if (search == null)
                    throw new KeyNotFoundException();

                _dbContext.Zaposlenis.Remove(search);

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}