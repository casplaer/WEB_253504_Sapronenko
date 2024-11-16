using Microsoft.AspNetCore.Mvc;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.Views.Components
{
	public class CartComponent : ViewComponent
	{
		public IViewComponentResult Invoke(Cart cart)
		{
			return View(cart);
		}
	}
}
