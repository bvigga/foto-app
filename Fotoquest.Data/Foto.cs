using System;

namespace Fotoquest.Data
{
    public class Foto
    {
        public Guid Id { get; set; }
        public string GeoDirection { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
