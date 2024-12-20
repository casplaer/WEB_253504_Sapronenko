﻿using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            string address = app.Configuration["ImageAddress:Address"]; 

            var _categories = new List<Category>
            {           
                    new Category {Id=1, Name="Ловкость",
                    NormalizedName="agility"},
                    new Category {Id=2, Name="Сила",
                    NormalizedName="strength"},
                    new Category {Id=3, Name="Интеллект", NormalizedName="intellect"},
                    new Category {Id=4, Name="Универсальный",
                    NormalizedName="universal"},
            };

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();

            if (!context.Heroes.Any())
            {
                var heroes = new List<DotaHero>
                {
                    new DotaHero { 
                                    Name = "Juggernaut",
                                    Description = "Воин с острова масок.",
                                    BaseDamage = 49, Image = $"{address}/Images/Juggernaut.jpg",
                                    CategoryId = 1,
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("agility"))},
                    new DotaHero {
                                    Name = "Pudge",
                                    Description = "Я как шаверма, если не повезло, будешь страдать...",
                                    BaseDamage = 68, Image = $"{address}/Images/Pudge.jpg",
                                    CategoryId = 2,
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("strength"))},
                    new DotaHero {
                                    Name = "Keeper of the Light",
                                    Description = "На бледном жеребце скачет, мчит на защиту света искра бессчётных солнц, Эзалор.",
                                    BaseDamage = 43, Image = $"{address}/Images/KOTL.jpg",
                                    CategoryId = 3,
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("intellect"))},
                    new DotaHero {
                                    Name = "Invoker",
                                    Description = "Cреди всех волшебников было всего одно, но гениальное и владеющее огромной памятью исключение, и имя ему — Invoker.",
                                    BaseDamage = 39, Image = $"{address}/Images/Invoker.jpg",
                                    CategoryId = 4,
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("universal"))},
                };

                await context.AddRangeAsync(heroes);
            }

            await context.SaveChangesAsync();
        }
    }
}
