using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.API.Services.HeroService;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using static System.Net.WebRequestMethods;

namespace WEB_253504_Sapronenko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotaHeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public DotaHeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<DotaHero>>>> GetHeroes()
        {
            return Ok(await _heroService.GetHeroListAsync("any", 0));
        }

        [HttpGet("{category}-dotaheroes/page{pageNo}")]
        public async Task<ActionResult<ResponseData<List<DotaHero>>>> GetHeroes(string category,
                                                                                int pageNo=1,
                                                                                int pageSize=3)
        {
            return Ok(await _heroService.GetHeroListAsync(category,
                                                          pageNo,
                                                          pageSize));
        }

        [Authorize(Roles = "POWER-USER")]
        [HttpGet("{id}")]
        public async Task<ResponseData<DotaHero>> GetDotaHero(int id)
        {
            var dotaHero = (await _heroService.GetHeroByIdAsync(id)).Data!;

            return ResponseData<DotaHero>.Success(dotaHero);
        }

        [Authorize(Roles = "POWER-USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDotaHero(int id, DotaHero dotaHero, IFormFile? image = null)
        {
            await _heroService.UpdateHeroAsync(id, dotaHero, null);
            
            return NoContent();
        }

        [Authorize(Roles = "POWER-USER")]
        [HttpPost]
        public async Task<ActionResult<DotaHero>> PostDotaHero(DotaHero dotaHero)
        {
            await _heroService.CreateHeroAsync(dotaHero);

            return CreatedAtAction("GetDotaHero", new { id = dotaHero.Id }, dotaHero);
        }

        [Authorize(Roles = "POWER-USER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDotaHero(int id)
        {
            await _heroService.DeleteHeroAsync(id);

            return NoContent();
        }
    }
}
