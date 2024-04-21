using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using ProdavnicaTehnikeBekend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProdavnicaTehnikeBekend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProizvodController : ControllerBase
    {
        private IProizvodRepository _proizvodRepository;
        private readonly IMapper _mapper;
        public ProizvodController(IProizvodRepository proizvodRepository, IMapper mapper)
        {

            _proizvodRepository = proizvodRepository;
            _mapper = mapper;
        }

        // GET: api/Proizvod
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ProizvodDto>>> GetProizvodi()
        {
            try
            {
                List<Proizvod> proizvodi = await _proizvodRepository.GetProizvodi();

                if (proizvodi == null || proizvodi.Count == 0)
                    return NoContent();

                List<ProizvodDto> proizvodDtos = new List<ProizvodDto>();

                foreach (var proizvod in proizvodi)
                {
                    var proizvodDto = _mapper.Map<ProizvodDto>(proizvod);
                    proizvodDtos.Add(proizvodDto);
                }

                return Ok(proizvodDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

        }

        [AllowAnonymous]
        [HttpGet("{proizvodId}")]
        public async Task<ActionResult<ProizvodDto>> GetProizvodById(int proizvodId)
        {
            try
            {
                Proizvod proizvod = await _proizvodRepository.GetProizvodById(proizvodId);

                if (proizvod == null)
                    return NotFound();

                ProizvodDto proizvodDto = _mapper.Map<ProizvodDto>(proizvod);

                return Ok(proizvodDto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProizvodDto>> CreateProizvod(ProizvodCreateDto proizvod)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Proizvod toCreate = _mapper.Map<Proizvod>(proizvod);

                Proizvod createdProizvod = await _proizvodRepository.CreateProizvod(toCreate);

                return _mapper.Map<ProizvodDto>(createdProizvod);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ProizvodDto>> UpdateProizvod(ProizvodUpdateDto proizvod)
        {
            try
            {
                Proizvod toUpdate = _mapper.Map<Proizvod>(proizvod);

                Proizvod updatedProizvod= await _proizvodRepository.UpdateProizvod(toUpdate);

                ProizvodDto proizvodDto = _mapper.Map<ProizvodDto>(updatedProizvod);

                return Ok(proizvodDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProizvod(int proizvodId)
        {
            try
            {

                await _proizvodRepository.DeleteProizvod(proizvodId);
                return Ok();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);

            }


        }

        }
}
