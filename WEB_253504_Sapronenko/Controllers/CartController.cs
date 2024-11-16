using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Services.HeroService;
using WEB_253504_Sapronenko.UI.Services.SessionService;

namespace WEB_253504_Sapronenko.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private IHeroService _heroService;
        private ISessionCartService _sessionCartService;

        public CartController(IHeroService heroService, ISessionCartService sessionCartService)
        {
            _heroService = heroService;
            _sessionCartService = sessionCartService;
        }

        public IActionResult Index()
        {
            Cart cart = _sessionCartService.GetCart();
            return View(cart);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var response = await _heroService.GetHeroByIdAsync(id);
            if (response.Successfull)
            {
                _sessionCartService.AddToCart(response.Data!);
            }
            return Redirect(returnUrl);
        }

        [Route("[controller]/delete/{id:int}")]
        public ActionResult Delete(int id, string returnUrl)
        {
            _sessionCartService.RemoveItems(id);
            return Redirect(returnUrl);
        }
    }
}
