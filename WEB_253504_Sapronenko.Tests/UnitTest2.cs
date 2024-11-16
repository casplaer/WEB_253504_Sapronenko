using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.API.Services.HeroService;
using WEB_253504_Sapronenko.Domain.Entites;
using Xunit.Abstractions;

namespace WEB_253504_Sapronenko.Tests
{
    public class ProductServiceTests
    {
        private readonly ITestOutputHelper _output;
        private readonly AppDbContext _context;

        public ProductServiceTests(ITestOutputHelper output)
        {
            _output = output;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            SeedData(_context);
        }

        private void SeedData(AppDbContext context)
        {
            var categories = new List<Category>
            {
                    new Category {Id=1, Name="Ловкость",
                    NormalizedName="agility"},
                    new Category {Id=2, Name="Сила",
                    NormalizedName="strength"},
                    new Category {Id=3, Name="Интеллект", NormalizedName="intellect"},
                    new Category {Id=4, Name="Универсальный",
                    NormalizedName="universal"},
            };

            context.Category.AddRangeAsync(categories);

            var heroes = new List<DotaHero>
            {
                 new DotaHero {
                                    Name = "Juggernaut",
                                    Description = "Воин с острова масок.",
                                    BaseDamage = 49, Image = $"/Images/Juggernaut.jpg",
                                    CategoryId = 1,
                                    Category=
                                    categories.Find(c=>c.NormalizedName.Equals("agility"))},
                    new DotaHero {
                                    Name = "Pudge",
                                    Description = "Я как шаверма, если не повезло, будешь страдать...",
                                    BaseDamage = 68, Image = $"/Images/Pudge.jpg",
                                    CategoryId = 2,
                                    Category=
                                    categories.Find(c=>c.NormalizedName.Equals("strength"))},
                    new DotaHero {
                                    Name = "Keeper of the Light",
                                    Description = "На бледном жеребце скачет, мчит на защиту света искра бессчётных солнц, Эзалор.",
                                    BaseDamage = 43, Image = $"/Images/KOTL.jpg",
                                    CategoryId = 3,
                                    Category=
                                    categories.Find(c=>c.NormalizedName.Equals("intellect"))},
                    new DotaHero {
                                    Name = "Invoker",
                                    Description = "Cреди всех волшебников было всего одно, но гениальное и владеющее огромной памятью исключение, и имя ему — Invoker.",
                                    BaseDamage = 39, Image = $"/Images/Invoker.jpg",
                                    CategoryId = 4,
                                    Category=
                                    categories.Find(c=>c.NormalizedName.Equals("universal"))},
            };

            context.Heroes.AddRangeAsync(heroes);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetHeroListAsync_ReturnsFirstPageWithThreeHeroesAndCalculatesTotalPagesCorrectly()
        {
            var service = new HeroService(_context);
            var response = await service.GetHeroListAsync("any", 1);

            Assert.True(response.Successfull); 
            Assert.NotNull(response.Data); 
            Assert.Equal(3, response.Data.Items.Count); 
            Assert.Equal(2, response.Data.TotalPages); 
        }

        [Fact]
        public async Task GetHeroListAsync_ReturnsCorrectCurrentPage()
        {
            var service = new HeroService(_context);
            var response = await service.GetHeroListAsync("any", 2);

            Assert.True(response.Successfull);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.CurrentPage);
        }

        [Fact]
        public async Task GetHeroListAsync_CategoryFilteredCorrectly()
        {
            var service = new HeroService(_context);
            var response = await service.GetHeroListAsync("agility", 1);

            Assert.True(response.Successfull);
            Assert.NotNull(response.Data);

            foreach (var hero in response.Data.Items)
            {
                Assert.Equal("agility", hero.Category!.NormalizedName);
            }
        }

        [Fact]
        public async Task GetHeroListAsync_SetCorrectPageSizeIfExceeded()
        {
            for (int i = 0; i < 10; i++)
            {
                _context.Heroes.Add(new DotaHero 
                {
                    Name = $"Juggernaut{i}",
                    Description = "Воин с острова масок.",
                    BaseDamage = 49,
                    Image = $"/Images/Juggernaut.jpg",
                    CategoryId = 1,
                    Category = 
                        _context.Category.FirstOrDefault(c => c.NormalizedName.Equals("agility"))
                });
            }

            await _context.SaveChangesAsync();

            var service = new HeroService(_context);
            var response = await service.GetHeroListAsync("any", 1, 11);

            Assert.True(response.Successfull);
            Assert.NotNull(response.Data);

            Assert.Equal(5, response.Data.Items.Count);
        }

        [Fact]
        public async Task GetHeroListAsync_FalseIfExceededPageAmount()
        {
            var service = new HeroService(_context);
            var response = await service.GetHeroListAsync("agility", 10);

            Assert.False(response.Successfull);

        }
    }
}