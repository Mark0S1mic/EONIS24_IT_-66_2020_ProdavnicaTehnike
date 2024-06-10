using System.Collections.Generic;
using System.Linq;
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
    public class PorudzbinaController : ControllerBase
    {
        private  IPorudzbinaRepository _porudzbinaRepository;
        private readonly IMapper _mapper;

        public PorudzbinaController(IPorudzbinaRepository porudzbinaRepository, IMapper mapper)
        {
            _porudzbinaRepository = porudzbinaRepository;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult<List<PorudzbinaDto>>> GetPorudzbine()
        {
            try
            {
                List<Porudzbina> porudzbinas = await _porudzbinaRepository.GetPorudzbine();

                if (porudzbinas == null || porudzbinas.Count == 0)
                    return NoContent();

                List<PorudzbinaDto> porudzbinaDtos = new List<PorudzbinaDto>();

                foreach (var porudzbina in porudzbinas)
                {
                    PorudzbinaDto porudzbinaDto = _mapper.Map<PorudzbinaDto>(porudzbina);
                    porudzbinaDtos.Add(porudzbinaDto);
                }

                return Ok(porudzbinaDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{porudzbinaId}")]
        public async Task<ActionResult<PorudzbinaDto>> GetPorudzbinaById(int porudzbinaId)
        {
            try
            {
                Porudzbina porudzbina = await _porudzbinaRepository.GetPorudzbinaById(porudzbinaId);

                if (porudzbina == null)
                    return NotFound();

                PorudzbinaDto porudzbinaDto = _mapper.Map<PorudzbinaDto>(porudzbina);

                return Ok(porudzbinaDto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
       // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PorudzbinaDto>> CreatePorudzbina(PorudzbinaCreateDto porudzbina)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Porudzbina toCreate = _mapper.Map<Porudzbina>(porudzbina);

                Porudzbina createdPorudzbina = await _porudzbinaRepository.CreatePorudzbina(toCreate);

                return _mapper.Map<PorudzbinaDto>(createdPorudzbina);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<ActionResult<PorudzbinaDto>> UpdatePorudzbina(PorudzbinaUpdateDto porudzbina)
        {
            try
            {
                Porudzbina toUpdate = _mapper.Map<Porudzbina>(porudzbina);

                Porudzbina updatedPorudzbina = await _porudzbinaRepository.UpdatePorudzbina(toUpdate);

                PorudzbinaDto porudzbinaDto = _mapper.Map<PorudzbinaDto>(updatedPorudzbina);

                return Ok(porudzbinaDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeletePorudzbina(int porudzbinaId)
        {
            try
            {

                await _porudzbinaRepository.DeletePorudzbina(porudzbinaId);
                return Ok();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }
    }
}
