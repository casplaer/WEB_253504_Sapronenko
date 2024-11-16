using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.UI.Services.SessionService
{
    public interface ISessionCartService
    {
        void AddToCart(DotaHero hero);

        void RemoveItems(int id);

        void ClearAll();

        Cart GetCart();
    }
}
