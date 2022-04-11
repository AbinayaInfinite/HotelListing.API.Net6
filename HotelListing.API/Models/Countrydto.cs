namespace HotelListing.API.Models
{
    public class Countrydto : BaseCountrydto
    {
        public int Id { get; set; }
        public List<Hoteldto> Hotels { get; set; }
    }
}
