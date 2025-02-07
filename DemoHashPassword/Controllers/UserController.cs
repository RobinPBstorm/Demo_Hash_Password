using DemoHashPassword.API.Mapper;
using DemoHashPassword.DTOs;
using DemoHashPasword.BLL.Intefaces;
using DemoHashPasword.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoHashPassword.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService userService)
        {
            _service = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserFullDTO>))]
        public IActionResult Get()
        {
            return Ok(_service.GetAll().Select((user) => user.ToFullDTO()));
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserFullDTO))]
        [ProducesResponseType(404)]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                return Ok(_service.GetOneById(id).ToFullDTO());
            }
            catch(Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

    }
}
