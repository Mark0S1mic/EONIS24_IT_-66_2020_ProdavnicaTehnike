using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using ProdavnicaTehnikeBekend.Models.DTOs.PorudzbinaProizvodDto;
using ProdavnicaTehnikeBekend.Repositories;

namespace ProdavnicaTehnikeBekend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PorudzbinaProizvodController : ControllerBase
    {
        private IPorudzbinaProizvodRepository _porudzbinaProizvodRepository;
        private readonly IMapper _mapper;

        public PorudzbinaProizvodController(IPorudzbinaProizvodRepository porudzbinaProizvodRepository, IMapper mapper)
        {
            _mapper = mapper;
            _porudzbinaProizvodRepository = porudzbinaProizvodRepository;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<PorudzbinaProizvodDto>>> GetPorudzbinaProizvodi()
        {
            try
            {
                List<PorudzbinaProizvod> porudzbinaProizvodi = await  _porudzbinaProizvodRepository.GetPorudzbinaProizvodi();

                if (porudzbinaProizvodi == null || porudzbinaProizvodi.Count == 0)
                    return NoContent();

                List<PorudzbinaProizvodDto> porudzbinaProizvodDtos = new List<PorudzbinaProizvodDto>();

                foreach (var porudzbinaProizvod in porudzbinaProizvodi)
                {
                    var porudzbinaProizvodDto = _mapper.Map<PorudzbinaProizvodDto>(porudzbinaProizvod);
                    porudzbinaProizvodDtos.Add(porudzbinaProizvodDto);
                }

                return Ok(porudzbinaProizvodDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{proizvodId}/{porudzbinaId}")]
        public async Task<ActionResult<PorudzbinaProizvodDto>> GetPorudzbinaProizvodById(int proizvodId, int porudzbinaId)
        {
            try
            {
                var porudzbinaProizvod = await _porudzbinaProizvodRepository.GetPorudzbinaProizvodById(proizvodId, porudzbinaId);

                if (porudzbinaProizvod == null)
                    return NotFound();

                var porudzbinaProizvodDto = _mapper.Map<PorudzbinaProizvodDto>(porudzbinaProizvod);

                return Ok(porudzbinaProizvodDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

      //  [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PorudzbinaProizvodDto>> CreatePorudzbinaProizvod(PorudzbinaProizvodCreateDto porudzbinaProizvod)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                PorudzbinaProizvod toCreate = _mapper.Map<PorudzbinaProizvod>(porudzbinaProizvod);

                PorudzbinaProizvod createdPorudzbinaProizvod = await _porudzbinaProizvodRepository.CreatePorudzbinaProizvod(toCreate);

                return _mapper.Map<PorudzbinaProizvodDto>(createdPorudzbinaProizvod);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<PorudzbinaProizvodDto>> UpdatePorudzbinaProizvod(PorudzbinaProizvodUpdateDto porudzbinaProizvod)
        {
            try
            {
                PorudzbinaProizvod toUpdate = _mapper.Map<PorudzbinaProizvod>(porudzbinaProizvod);

                PorudzbinaProizvod updatedPorudzbinaProizvod = await _porudzbinaProizvodRepository.UpdatePorudzbinaProizvod(toUpdate);

                PorudzbinaProizvodDto porudzbinaProizvodDto = _mapper.Map<PorudzbinaProizvodDto>(updatedPorudzbinaProizvod);

                return Ok(porudzbinaProizvodDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{proizvodId}/{porudzbinaId}")]
        public async Task<ActionResult> DeletePorudzbinaProizvod(int proizvodId, int porudzbinaId)
        {
            try
            {
                await _porudzbinaProizvodRepository.DeletePorudzbinaProizvod(proizvodId, porudzbinaId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
