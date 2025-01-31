using DemoHashPassword.API.DTOs;
using DemoHashPassword.API.Mapper;
using DemoHashPassword.DL.Entities;
using DemoHashPassword.DTOs;
using DemoHashPasword.BLL.Intefaces;
using DemoHashPasword.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoHashPassword.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        // GET: api/<AuthController>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginDTO login)
        {
            try
            {
                string token = _service.Login(login.Username, login.Password);
                return Ok(token);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // GET api/<AuthController>/5
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserRegisterForm userRegister)
        {
            try
            {
                _service.Register(userRegister.ToEntity(), userRegister.Password);
                return Created();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
