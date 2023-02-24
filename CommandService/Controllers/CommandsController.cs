using AutoMapper;
using CommandService.Data.Interfaces;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/commands/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public CommandsController(
            ICommandRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            System.Console.WriteLine($"--> Getting commands for platform {platformId}");

            if (_repository.IsPlatformExists(platformId) == false)
            {
                return NotFound();
            }

            var repResult = _repository.GetForPlatform(platformId);
            var result = _mapper.Map<IEnumerable<CommandReadDto>>(repResult);

            return Ok(result);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            System.Console.WriteLine($"--> Getting command {commandId} for platform {platformId}");

            if (_repository.IsPlatformExists(platformId) == false)
            {
                return NotFound();
            }

            var repResult = _repository.GetForPlatformAndId(platformId, commandId);

            if (repResult == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CommandReadDto>(repResult);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            System.Console.WriteLine($"--> Creating command for platform {platformId}");
            if (_repository.IsPlatformExists(platformId) == false)
            {
                return NotFound();
            }
            
            var mapResult = _mapper.Map<Command>(commandDto);
            _repository.CreateForPlatform(platformId, mapResult);
            _repository.SaveChanges();

            var result = _mapper.Map<CommandReadDto>(mapResult);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = result.Id } );
        }
    }
}