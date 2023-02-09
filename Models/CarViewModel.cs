using System.ComponentModel.DataAnnotations;

namespace MVC_CRUD.Models
{
    public class CarViewModel
    {
        [Key]
        public int carID { get; set; }
        [Required]
        public string Brand { get; set; }
        [Range(100000 , int.MaxValue ,ErrorMessage = "should be greater than 100,000")]
        public int Price { get; set; }
        [Required]
        public string model { get; set; }
    }
}
