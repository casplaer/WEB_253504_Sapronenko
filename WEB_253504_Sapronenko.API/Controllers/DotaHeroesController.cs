using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.API.Services.HeroService;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotaHeroesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHeroService _heroService;

        public DotaHeroesController(AppDbContext context, IHeroService heroService)
        {
            _context = context;
            _heroService = heroService;
        }

        // GET: api/DotaHeroes
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<DotaHero>>>> GetHeroes(string? category,
                                                                                int pageNo=1,
                                                                                int pageSize=3)
        {
            return Ok(await _heroService.GetHeroListAsync(category,
                                                          pageNo,
                                                          pageSize));
        }

        // GET: api/DotaHeroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DotaHero>> GetDotaHero(int id)
        {
            var dotaHero = await _context.Heroes.FindAsync(id);

            if (dotaHero == null)
            {
                return NotFound();
            }

            return dotaHero;
        }

        // PUT: api/DotaHeroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDotaHero(int id, DotaHero dotaHero)
        {
            if (id != dotaHero.Id)
            {
                return BadRequest();
            }

            _context.Entry(dotaHero).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DotaHeroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DotaHeroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DotaHero>> PostDotaHero(DotaHero dotaHero)
        {
            _context.Heroes.Add(dotaHero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDotaHero", new { id = dotaHero.Id }, dotaHero);
        }

        // DELETE: api/DotaHeroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDotaHero(int id)
        {
            var dotaHero = await _context.Heroes.FindAsync(id);
            if (dotaHero == null)
            {
                return NotFound();
            }

            _context.Heroes.Remove(dotaHero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DotaHeroExists(int id)
        {
            return _context.Heroes.Any(e => e.Id == id);
        }
    }
}
