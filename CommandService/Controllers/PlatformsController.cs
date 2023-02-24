using AutoMapper;
using CommandService.Data.Interfaces;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/commands/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(
            ICommandRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAll()
        {
            System.Console.WriteLine("--> Getting platforms from Command Service.");

            var repResult = _repository.GetAllPlatforms();
            var result = _mapper.Map<IEnumerable<PlatformReadDto>>(repResult);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            System.Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test of Platforms Controller");
        }
    }
}