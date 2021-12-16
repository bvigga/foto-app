using AutoMapper;
using Fotoquest.Core.DTOs;
using Fotoquest.Core.IRepository;
using Fotoquest.Core.Models;
using Fotoquest.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
//using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Hosting;

namespace Fotoquest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FotoController> _logger;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;
        // private readonly IDistributedCache _cache;
        private static readonly int[] SupportedSizes = { 128, 512, 2048 };

        private static int SanitizeSize(int value)
        {
            if (value >= 2048) { return 2048; }
            return SupportedSizes.First(size => size >= value);
        }

        public FotoController(IUnitOfWork unitOfWork,
            ILogger<FotoController> logger,
            IHostingEnvironment env,
            //IDistributedCache cache,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _env = env;
            //_cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFotos([FromQuery] RequestParams requestParams)
        {
            var fotos = await _unitOfWork.Fotos.GetPagedList(requestParams);
            var results = _mapper.Map<IList<FotoDTO>>(fotos);
            return Ok(results);
        }

        [HttpGet("{id:Guid}/{width}/{height}", Name = "GetFoto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFoto(Guid id, int width, int height)
        {
            var foto = await _unitOfWork.Fotos.Get(q => q.Id == id);
            var result = _mapper.Map<FotoDTO>(foto);
            var result2 = ResizeImage(result, width, height);
            if(result2 is null) { return BadRequest(); }
            return Ok(result2);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFoto([FromForm] CreateFotoDTO fotoDTO)
        {
            if (ModelState.IsValid)
            {
                // if (!string.IsNullOrEmpty(fotoDTO.ImageUrl))
                // {
                    try
                    {
                        //var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\images"}";

                    var path = Path.Combine(_env.WebRootPath , "images");
                    var foto = new Foto();
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (FileStream fileStream = System.IO.File.Create(path + fotoDTO.Fotos.FileName))
                            {
                            fotoDTO.Fotos.CopyTo(fileStream);
                            fileStream.Flush();
                            foto = _mapper.Map<Foto>(fotoDTO);
                            foto.ImageUrl = Path.Combine(path , fotoDTO.Fotos.FileName);
                            await _unitOfWork.Fotos.Insert(foto);
                            await _unitOfWork.Save();
                        }

                        return CreatedAtRoute("GetFoto", new { id = foto.Id }, foto);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Invalid POST attempt in {nameof(CreateFoto)} - {ex.Message}");
                        return BadRequest(ModelState);
                    }
                // }
                //return BadRequest(ModelState);
            }
            _logger.LogError($"Invalid POST attempt in {nameof(CreateFoto)}");
            return BadRequest(ModelState);
        }

        //[Authorize]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFoto(Guid id, [FromBody] UpdateFotoDTO fotoDTO)
        {
            if (!ModelState.IsValid || id == Guid.Empty)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateFoto)}");
                return BadRequest(ModelState);
            }


            var foto = await _unitOfWork.Fotos.Get(q => q.Id == id);
            if (foto == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateFoto)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(fotoDTO, foto);
            _unitOfWork.Fotos.Update(foto);
            await _unitOfWork.Save();

            return NoContent();

        }

        //[Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFoto(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteFoto)}");
                return BadRequest();
            }

            var foto = await _unitOfWork.Fotos.Get(q => q.Id == id);
            if (foto == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteFoto)}");
                return BadRequest("Submitted data is invalid");
            }

            await _unitOfWork.Fotos.Delete(id);
            await _unitOfWork.Save();

            return NoContent();

        }

        /// <summary>
        /// Takes an image and returns a byte stream of resized image.
        /// This is done on the fly as the image is being requested.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private byte[] ResizeImage(FotoDTO fotoDTO, int width, int height)
        {
            if (width >= 0 && height >= 0 && (width != 0 || height != 0))
            {
                if (height == 0)
                {
                    width = SanitizeSize(width);
                }
                else
                {
                    width = 0;
                    height = SanitizeSize(height);
                }

                //var path = Path.Combine(_env.WebRootPath, "images", $"{fotoDTO.Fotos.FileName}");
                //var imageFileStream = System.IO.File.OpenRead(path);

                //var imagePath = System.IO.File.Create(fotoDTO.ImageUrl);
                //var fileInfo = _webHostEnv.WebRootPath + imagePath;

                byte[] data = null;

                using (var outputStream = new MemoryStream())
                {
                    using (var inputStream = System.IO.File.Create(fotoDTO.ImageUrl))
                    using (var image = Image.Load(inputStream))
                    {
                        image.Mutate(x => x.Resize(width, height));
                        image.SaveAsJpeg(outputStream);
                    }

                    data = outputStream.ToArray();
                }
                    return data;
            }
            return null;
        }

    }
}
