using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253504_Sapronenko.Domain.Entites
{
    public class CartItem
    {
        public DotaHero Item { get; set; } = default!;
        public int Count { get; set; } = 0;
    }
}
