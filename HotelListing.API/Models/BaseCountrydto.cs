using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models
{
    public abstract class BaseCountrydto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
