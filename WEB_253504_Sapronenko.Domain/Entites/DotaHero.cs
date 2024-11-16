using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253504_Sapronenko.Domain.Entites
{
    public class DotaHero
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int BaseDamage { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public string? Image {  get; set; } = string.Empty;
        public double Price { get; set; } = 9.99;
    }
}
