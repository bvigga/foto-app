using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Fotoquest.Core.DTOs
{
    public class CreateFotoDTO
    {
        [Required]
        public IFormFile Fotos { get; set; }

        //[Required]
        public string GeoDirection { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        //[Required]
        public string ImageUrl { get; set; }
    }

    public class UpdateFotoDTO : CreateFotoDTO
    {

    }

    public class FotoDTO : CreateFotoDTO
    {
        public Guid Id { get; set; }
    }
}
