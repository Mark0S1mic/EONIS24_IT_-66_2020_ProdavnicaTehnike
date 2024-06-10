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
    public class ZaposleniController : ControllerBase
    {
        private IZaposleniRepository _zaposleniRepository;
        private readonly IMapper _mapper;

        public ZaposleniController(IZaposleniRepository zaposleniRepository, IMapper mapper)
        {
            _zaposleniRepository = zaposleniRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<ZaposleniDto>>> GetZaposleni()
        {
            try
            {
                List<Zaposleni> zaposlenis = await _zaposleniRepository.GetZaposleni();

                if (zaposlenis == null || zaposlenis.Count == 0)
                    return NoContent();

                List<ZaposleniDto> zaposleniDtos = new List<ZaposleniDto>();

                foreach (var zaposleni in zaposlenis)
                {
                    var zaposleniDto = _mapper.Map<ZaposleniDto>(zaposleni);
                    zaposleniDtos.Add(zaposleniDto);
                }

                return Ok(zaposleniDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{zaposleniId}")]
        public async Task<ActionResult<ZaposleniDto>> GetZaposleniById(int zaposleniId)
        {
            try
            {
                Zaposleni zaposleni = await _zaposleniRepository.GetZaposleniById(zaposleniId);

                if ( zaposleni == null)
                    return NotFound();

                ZaposleniDto zaposleniDto = _mapper.Map<ZaposleniDto>(zaposleni);

                return Ok(zaposleniDto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ZaposleniDto>> CreateZaposleni(ZaposleniCreateDto zaposleni)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Zaposleni toCreate = _mapper.Map<Zaposleni>(zaposleni);

                Zaposleni createdZaposleni = await _zaposleniRepository.CreateZaposleni(toCreate);

                return _mapper.Map<ZaposleniDto>(createdZaposleni);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ZaposleniDto>> UpdateZaposlenir(ZaposleniUpdateDto zaposleni)
        {
            try
            {
                Zaposleni toUpdate = _mapper.Map<Zaposleni>(zaposleni);

                Zaposleni updatedZaposleni = await _zaposleniRepository.UpdateZaposleni(toUpdate);

                ZaposleniDto zaposleniDto = _mapper.Map<ZaposleniDto>(updatedZaposleni);

                return Ok(zaposleniDto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteZaposleni(int zaposleniId)
        {
            try
            {

                await _zaposleniRepository.DeleteZaposleni(zaposleniId);
                return Ok();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }
    }
}
