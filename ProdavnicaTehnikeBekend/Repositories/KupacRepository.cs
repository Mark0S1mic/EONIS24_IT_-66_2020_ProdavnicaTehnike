using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public class KupacRepository : IKupacRepository
    {
        private readonly ProdavnicaTehnikeContext _dbContext;
        private readonly HashingService _hashingService;

        public KupacRepository(ProdavnicaTehnikeContext dbContext)
        {
            _dbContext = dbContext;
            _hashingService = new HashingService();
        }

        public async Task<List<Kupac>> GetKupci()
        {
            try
            {
                return await _dbContext.Kupacs.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Kupac> GetKupacById(int kupacId)
        {
            try
            {
                Kupac? search = await _dbContext.Kupacs.FirstOrDefaultAsync(k => k.KupacId == kupacId);

                if (search == null)
                    throw new KeyNotFoundException();

                return search;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public async Task<Kupac> GetKupacByKorisnickoIme(string korisnickoImeKupca)
        {
            try
            {
                Kupac? search = await _dbContext.Kupacs.FirstOrDefaultAsync(k => k.KorisnickoImeKupca == korisnickoImeKupca);
                if (search == null)
                    throw new KeyNotFoundException();

                return search;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }





        public async Task<Kupac> CreateKupac(Kupac kupac)
        {
            kupac.SifraKupca = _hashingService.HashPassword(kupac.SifraKupca);
            var createdKupac = await _dbContext.AddAsync(kupac);

            await _dbContext.SaveChangesAsync();

            return createdKupac.Entity;
        }

        public async Task<Kupac> UpdateKupac(Kupac kupac)
        {
            try
            {
                var toUpdate = await _dbContext.Kupacs.FirstOrDefaultAsync(k => k.KupacId == kupac.KupacId);

                if (toUpdate == null)
                    throw new KeyNotFoundException();

                toUpdate.KupacId = kupac.KupacId;
                toUpdate.KorisnickoImeKupca = kupac.KorisnickoImeKupca;
                toUpdate.SifraKupca = kupac.SifraKupca;
                toUpdate.KontaktKupca = kupac.KontaktKupca;
                toUpdate.GradKupca = kupac.GradKupca;
                toUpdate.AdresaKupca = kupac.AdresaKupca;
                toUpdate.DatumRodjenjaKupca = kupac.DatumRodjenjaKupca;
                toUpdate.BrojTelefonaKupca = kupac.BrojTelefonaKupca;
                toUpdate.Porudzbinas = kupac.Porudzbinas;

                await _dbContext.SaveChangesAsync();

                return toUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteKupac(int kupacId)
        {
            try
            {
                Kupac? search = await _dbContext.Kupacs.FirstOrDefaultAsync(k => k.KupacId == kupacId);

                if (search == null)
                    throw new KeyNotFoundException();

                _dbContext.Kupacs.Remove(search);

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        

    }
}