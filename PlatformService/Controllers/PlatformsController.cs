using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaltformService.Data.Interfaces;
using PaltformService.Dtos;
using PaltformService.Models;

namespace PaltformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAll()
        {
            var repResult = _repository.GetAll();
            var result = _mapper.Map<IEnumerable<PlatformReadDto>>(repResult);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<PlatformReadDto> GetById(int id)
        {
            var repResult = _repository.Get(id);
            if (repResult == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<PlatformReadDto>(repResult);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> Create(PlatformCreateDto platform)
        {
            var mapResult = _mapper.Map<Platform>(platform);
            _repository.Create(mapResult);
            _repository.SaveChanges();

            var result = _mapper.Map<PlatformReadDto>(mapResult);
            return CreatedAtRoute(nameof(GetById), new { Id = result.Id }, result);
        }
    }
}