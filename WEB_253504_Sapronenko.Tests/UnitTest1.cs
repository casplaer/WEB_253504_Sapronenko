using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Controllers;
using WEB_253504_Sapronenko.UI.Services.Authentication;
using WEB_253504_Sapronenko.UI.Services.CategoryService;
using WEB_253504_Sapronenko.UI.Services.FileService;
using WEB_253504_Sapronenko.UI.Services.HeroService;
using Xunit.Abstractions;

namespace WEB_253504_Sapronenko.Tests
{
    public class HeroControllerTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IHeroService _heroService;
        private readonly ICategoryService _categoryService;

        private readonly ResponseData<ListModel<DotaHero>> _heroResponse;
        private readonly ResponseData<List<Category>> _categoryResponse;

        public HeroControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _heroService = Substitute.For<IHeroService>();
            _categoryService = Substitute.For<ICategoryService>();

            var heroes = new ListModel<DotaHero>
            {
                Items = new List<DotaHero>
                {
                    new DotaHero { Name = "Juggernaut", CategoryId = 1 },
                    new DotaHero { Name = "Pudge", CategoryId = 2 },
                    new DotaHero { Name = "Keeper of the Light", CategoryId = 3 },
                    new DotaHero { Name = "Invoker", CategoryId = 4 }
                },
                TotalPages = 2
            };

            _heroResponse = new ResponseData<ListModel<DotaHero>>
            {
                Successfull = true,
                Data = heroes
            };

            _categoryResponse = new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = new List<Category>
                {
                    new Category { Id = 1, Name = "Agility", NormalizedName = "agility" },
                    new Category { Id = 2, Name = "Strength", NormalizedName = "strength" },
                    new Category { Id = 3, Name = "Intellect", NormalizedName = "intellect" },
                    new Category { Id = 4, Name = "Универсал", NormalizedName = "unviersal" },
                }
            };
        }

        [Fact]
        public async Task Index_SetsCurrentCategoryToDefault_WhenCategoryIsNull()
        {
            _heroService.GetHeroListAsync().Returns(Task.FromResult(_heroResponse));
            _categoryService.GetCategoriesAsync().Returns(Task.FromResult(_categoryResponse));

            var controller = new HeroController(_heroService, _categoryService);

            var result = await controller.Index(null);

            Assert.Equal("Все", controller.ViewBag.currentCategory); 
        }

        [Fact]
        public async Task Index_ReturnCorrectCategory_WhenPassedCategory()
        {
            _heroService.GetHeroListAsync("agility").Returns(Task.FromResult(_heroResponse));
            _categoryService.GetCategoriesAsync().Returns(Task.FromResult(_categoryResponse));

            var controller = new HeroController(_heroService, _categoryService);

            var result = await controller.Index("agility");

            Assert.Equal("Ловкость", controller.ViewBag.currentCategory);
        }

        [Fact]
        public async Task Index_ReturnsCorrectModel()
        {
            _heroService.GetHeroListAsync().Returns(Task.FromResult(_heroResponse));
            _categoryService.GetCategoriesAsync().Returns(Task.FromResult(_categoryResponse));

            var controller = new HeroController(_heroService, _categoryService);

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model as ListModel<DotaHero>;
            Assert.NotNull(model);
            Assert.Equal(1, model.CurrentPage);
            Assert.Equal(2, model.TotalPages);
            Assert.Equal(4, model.Items.Count);

        }

        [Fact]
        public async Task Index_Return404_WhenDontGetCorrectHeroList()
        {
            var incorrectResponse = _heroResponse;
            incorrectResponse.Successfull = false;

            _heroService.GetHeroListAsync().Returns(Task.FromResult(incorrectResponse));
            _categoryService.GetCategoriesAsync().Returns(Task.FromResult(_categoryResponse));

            var controller = new HeroController(_heroService, _categoryService);

            var result = await controller.Index(null);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}