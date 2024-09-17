namespace WEB_253504_Sapronenko.Views.Components
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartInfo = "00,0 руб <i class='fa-solid fa-cart-shopping'></i> (0)";
            return View("Default", cartInfo);
        }
    }
}
