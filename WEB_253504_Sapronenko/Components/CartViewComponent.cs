namespace WEB_253504_Sapronenko.Views.Components
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartInfo = "";
            return View();
        }
    }
}
