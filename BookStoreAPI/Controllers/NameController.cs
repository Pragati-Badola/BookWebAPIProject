using BookStoreAPI.Models;
using BookStoreAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : Controller
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager; 
        public NameController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet, Authorize(Roles = "Manager")]
        public IActionResult Get()
        {
            return Ok(new string[] { "New York", "New Jersey" });
        }

        [HttpGet("{id}", Name = "Get"), Authorize(Roles = "Operator")]
        public IActionResult Get(int id)
        {
            return Ok("New Jersey");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = jwtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);
            if( token == null )
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
