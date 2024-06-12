using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using ProdavnicaTehnikeBekend.Repositories;

namespace ProdavnicaTehnikeBekend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KupacController : ControllerBase
    {
        private IKupacRepository _kupacRepository;
        private readonly IMapper _mapper;

        public KupacController(IKupacRepository kupacRepository, IMapper mapper)
        {
            _kupacRepository = kupacRepository;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult<List<KupacDto>>> GetKupci()
        {
            try
            {
                List<Kupac> kupci = await _kupacRepository.GetKupci();

                if (kupci == null || kupci.Count == 0)
                    return NoContent();

                List<KupacDto> kupacsDto = new List<KupacDto>();

                foreach (var kupac in kupci)
                {
                    KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);
                    kupacsDto.Add(kupacDto);
                }

                return Ok(kupacsDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

        }
         
        // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpGet("{kupacId}")]
        public async Task<ActionResult<KupacDto>> GetKupacById(int kupacId)
        {
            try
            {
                Kupac kupac = await _kupacRepository.GetKupacById(kupacId);

                if (kupac == null)
                    return NotFound();

                KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);

                return Ok(kupacDto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }





        // [Authorize(Roles = "Admin, User")]
        [AllowAnonymous]
        [HttpGet("ime/{korisnickoImeKupca}")]
        public async Task<ActionResult<KupacDto>> GetKupacByKorisnickoIme(string korisnickoImeKupca)
        {
            try
            {
                Kupac kupac = await _kupacRepository.GetKupacByKorisnickoIme(korisnickoImeKupca);

                if (kupac == null)
                
                    return NotFound();

                KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);

                return Ok(kupacDto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }





        //  [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<KupacDto>> CreateKupac(KupacCreateDto kupac)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Kupac toCreate = _mapper.Map<Kupac>(kupac);
                Kupac createdKupac = await _kupacRepository.CreateKupac(toCreate);

                return _mapper.Map<KupacDto>(createdKupac);

               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Došlo je do greške prilikom kreiranja kupca: {ex.Message}");
            }
        }


        [Authorize(Roles = "Admin, User")]
        [HttpPut("{kupacId}")]
        public async Task<ActionResult<KupacDto>> UpdateKupac( KupacUpdateDto kupac)
        {
            try
            {
                Kupac toUpdate = _mapper.Map<Kupac>(kupac);

                Kupac updatedKupac = await _kupacRepository.UpdateKupac(toUpdate);

                KupacDto updatedKupacDto = _mapper.Map<KupacDto>(updatedKupac);

                return Ok(updatedKupacDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Došlo je do greške prilikom ažuriranja kupca: {ex.Message}");
            }
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteKupac(int kupacId)
        {
            try
            {

                await _kupacRepository.DeleteKupac(kupacId);
                return Ok();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }
    }
}