using AutoMapper;
using CommandService.Data.Interfaces;
using CommandService.Dtos;
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
    }
}