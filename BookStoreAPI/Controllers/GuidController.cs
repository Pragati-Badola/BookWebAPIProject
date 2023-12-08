using BookStoreAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuidController : Controller
    {
        private readonly IGuidService _guidService;

        public GuidController(IGuidService guidService)
        {
            _guidService = guidService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var guid = _guidService.GetGuid();
            return Ok(guid);
        }
    }
}
