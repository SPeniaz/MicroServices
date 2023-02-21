using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaltformService.Data.Interfaces;
using PaltformService.Dtos;

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
    }
}