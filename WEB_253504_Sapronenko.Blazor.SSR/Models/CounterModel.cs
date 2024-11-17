using System.ComponentModel.DataAnnotations;

namespace WEB_253504_Sapronenko.Blazor.SSR.Models
{
    public class CounterModel
    {
        [Required(ErrorMessage = "Count should be between 1 and 10")]
        [Range(1, 10)]
        public int Count { get; set; }
    }
}
