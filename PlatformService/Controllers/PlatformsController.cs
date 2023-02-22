using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaltformService.Data.Interfaces;
using PaltformService.Dtos;
using PaltformService.Models;
using PaltformService.SyncDataServices.Http.Interfaces;

namespace PaltformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper,
            ICommandDataClient commandDataClient
        )
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
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
        public async Task<ActionResult<PlatformReadDto>> Create(PlatformCreateDto platform)
        {
            var mapResult = _mapper.Map<Platform>(platform);
            _repository.Create(mapResult);
            _repository.SaveChanges();

            var result = _mapper.Map<PlatformReadDto>(mapResult);
            try
            {
                 await _commandDataClient.SendPlatformToCommand(result);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetById), new { Id = result.Id }, result);
        }
    }
}