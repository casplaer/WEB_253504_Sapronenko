using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.Domain.Models
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = [];

        public virtual void AddToCart(DotaHero hero)
        {
            var newItem = new CartItem { Item = hero, Count = 1 };
            if(CartItems.ContainsKey(hero.Id))
            {
                newItem.Count++;
            }
            else
                CartItems.Add(newItem.Item.Id, newItem);
        }

        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                if(CartItems[id].Count == 1)
                    CartItems.Remove(id);
                else
                    CartItems[id].Count--;
            }
            return;
        }

        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        public int Count => CartItems.Sum(item => item.Value.Count); 

        public double TotalPrice => CartItems.Sum(ci => ci.Value.Item.Price * ci.Value.Count);
        
    }
}
