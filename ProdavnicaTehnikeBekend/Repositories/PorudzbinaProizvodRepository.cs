using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public class PorudzbinaProizvodRepository : IPorudzbinaProizvodRepository
    {
        private readonly ProdavnicaTehnikeContext _dbContext;

        public PorudzbinaProizvodRepository(ProdavnicaTehnikeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PorudzbinaProizvod>> GetPorudzbinaProizvodi()
        {
            try
            {
                return await _dbContext.PorudzbinaProizvods
            .Include(p => p.Porudzbina)
            .Include(p => p.Proizvod)
            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PorudzbinaProizvod> GetPorudzbinaProizvodById(int proizvodId, int porudzbinaId)
        {
            try
            {
                PorudzbinaProizvod? search = await _dbContext.PorudzbinaProizvods
                    .FirstOrDefaultAsync(pp => pp.ProizvodId == proizvodId && pp.PorudzbinaId == porudzbinaId);

                if (search == null)
                    throw new KeyNotFoundException();

                return search;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PorudzbinaProizvod> CreatePorudzbinaProizvod(PorudzbinaProizvod porudzbinaProizvod)
        {
            var createdPorudzbinaProizvod = await _dbContext.PorudzbinaProizvods.AddAsync(porudzbinaProizvod);
            await _dbContext.SaveChangesAsync();
            return createdPorudzbinaProizvod.Entity;
        }

        public async Task<PorudzbinaProizvod> UpdatePorudzbinaProizvod(PorudzbinaProizvod porudzbinaProizvod)
        {
            try
            {
                _dbContext.Entry(porudzbinaProizvod).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return porudzbinaProizvod;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeletePorudzbinaProizvod(int proizvodId, int porudzbinaId)
        {
            try
            {
                var porudzbinaProizvod = await GetPorudzbinaProizvodById(proizvodId, porudzbinaId);
                _dbContext.PorudzbinaProizvods.Remove(porudzbinaProizvod);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
