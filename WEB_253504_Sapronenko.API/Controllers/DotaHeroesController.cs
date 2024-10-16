using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
        private readonly AppDbContext _context;
        private readonly IHeroService _heroService;

        public DotaHeroesController(AppDbContext context, IHeroService heroService)
        {
            _context = context;
            _heroService = heroService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<DotaHero>>>> GetHeroes()
        {
            return Ok(await _heroService.GetHeroListAsync("any", 0));
        }

        // GET: api/DotaHeroes
        [HttpGet("{category}-dotaheroes/page{pageNo}")]
        public async Task<ActionResult<ResponseData<List<DotaHero>>>> GetHeroes(string category,
                                                                                int pageNo=1,
                                                                                int pageSize=3)
        {
            return Ok(await _heroService.GetHeroListAsync(category,
                                                          pageNo,
                                                          pageSize));
        }

        // GET: api/DotaHeroes/5
        [HttpGet("{id}")]
        public async Task<ResponseData<DotaHero>> GetDotaHero(int id)
        {
            var dotaHero = (await _heroService.GetHeroByIdAsync(id)).Data!;

            return ResponseData<DotaHero>.Success(dotaHero);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDotaHero([FromForm] int idJson, [FromForm] string jsonDotaHero, [FromForm] IFormFile? image = null)
        {
            var id = JsonSerializer.Deserialize<int>(idJson);

            var dotaHero = JsonSerializer.Deserialize<DotaHero>(jsonDotaHero);

            if (id != dotaHero.Id)
            {
                return BadRequest();
            }

            if (image != null && image.Length > 0)
            {
                var imagePathStartIndex = dotaHero.Image.IndexOf("Images");

                var relativeImagePath = dotaHero.Image.Substring(imagePathStartIndex);
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeImagePath);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                dotaHero.Image = "https://localhost:7002/Images/" + uniqueFileName;
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

        [HttpPost]
        public async Task<ActionResult<DotaHero>> PostDotaHero([FromForm] string dotaHero, [FromForm] IFormFile? image = null)
        {
            var hero = JsonSerializer.Deserialize<DotaHero>(dotaHero);

            if (hero == null)
            {
                return BadRequest("Некорректные данные героя.");
            }

            if (image != null && image.Length > 0)
            {
                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                hero.Image = "https://localhost:7002/Images/" + uniqueFileName;
            }

            _context.Entry(hero).State = EntityState.Added;
            _context.Heroes.Add(hero);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDotaHero", new { id = hero.Id }, dotaHero);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDotaHero(int id)
        {
            var dotaHero = await _context.Heroes.FindAsync(id);
            if (dotaHero == null)
            {
                return NotFound();
            }

            var imagePathStartIndex = dotaHero.Image.IndexOf("Images");

            var relativeImagePath = dotaHero.Image.Substring(imagePathStartIndex);
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeImagePath);

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
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
