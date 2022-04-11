﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models
{
    public abstract class BaseHoteldto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public double? Rating { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int CountryId { get; set; }

    }
}
