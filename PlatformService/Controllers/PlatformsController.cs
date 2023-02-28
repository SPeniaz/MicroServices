using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaltformService.Data.Interfaces;
using PaltformService.Dtos;
using PaltformService.Models;
using PaltformService.SyncDataServices.Http.Interfaces;
using PlatformService.AsyncDataServices.Interfaces;

namespace PaltformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient
        )
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
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

            //Send Sync Message
            try
            {
                 await _commandDataClient.SendPlatformToCommand(result);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(result);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                
                System.Console.WriteLine($"--> Could not send async: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetById), new { Id = result.Id }, result);
        }
    }
}