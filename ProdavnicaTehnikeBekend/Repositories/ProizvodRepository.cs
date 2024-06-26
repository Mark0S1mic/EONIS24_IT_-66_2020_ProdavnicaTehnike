
using Microsoft.EntityFrameworkCore;
using ProdavnicaTehnikeBekend.Models;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public class ProizvodRepository : IProizvodRepository
    {

        ProdavnicaTehnikeContext _dbContext;

        public ProizvodRepository(ProdavnicaTehnikeContext dbContext)
        {
        
            _dbContext = dbContext;
        }



        public async Task<List<Proizvod>> GetProizvodi()
        {
            try
            {
                return await _dbContext.Proizvods.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Proizvod> GetProizvodById(int proizvodId)
        {
            try
            {
                Proizvod? search = await _dbContext.Proizvods.FirstOrDefaultAsync(k => k.ProizvodId == proizvodId);

                if (search == null)
                    throw new KeyNotFoundException();

                return search;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public async Task<Proizvod> GetProizvodByNaziv(string nazivProizvoda)
        {
            try
            {
                Proizvod? search = await _dbContext.Proizvods.FirstOrDefaultAsync(p => p.NazivProizvoda == nazivProizvoda);
                if (search == null)
                    throw new KeyNotFoundException();

                return search;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }





        public  async Task<Proizvod> CreateProizvod(Proizvod proizvod)
        {
            var createdProizvod = await _dbContext.AddAsync(proizvod);

            await _dbContext.SaveChangesAsync();

            return createdProizvod.Entity;
        }

        public async Task DeleteProizvod(int proizvodId)
        {
            try
            {
                Proizvod? search = await _dbContext.Proizvods.FirstOrDefaultAsync(p => p.ProizvodId == proizvodId);

                if (search == null)
                    throw new KeyNotFoundException();

                _dbContext.Proizvods.Remove(search);

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

      

        public async Task<Proizvod> UpdateProizvod(Proizvod proizvod)
        {
            try
            {
                var toUpdate = await _dbContext.Proizvods.FirstOrDefaultAsync(p => p.ProizvodId == proizvod.ProizvodId);

                if (toUpdate == null)
                    throw new KeyNotFoundException("Proizvod not found");

                toUpdate.ProizvodId = proizvod.ProizvodId;
                toUpdate.NazivProizvoda = proizvod.NazivProizvoda;
                toUpdate.CenaProizvoda = proizvod.CenaProizvoda;
                toUpdate.TipProizvoda = proizvod.TipProizvoda;
                toUpdate.OpisProizvoda = proizvod.OpisProizvoda;
                toUpdate.Kolicina = proizvod.Kolicina;
                toUpdate.StatusProizvoda = proizvod.StatusProizvoda;

                await _dbContext.SaveChangesAsync();

                return toUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
