using DemoHashPassword.API.DTOs;
using DemoHashPassword.API.Mapper;
using DemoHashPassword.DL.Entities;
using DemoHashPassword.DTOs;
using DemoHashPasword.BLL.Intefaces;
using DemoHashPasword.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoHashPassword.API.Controllers
{
	[Authorize("authPolicy")]
	[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly JWTService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthService service, JWTService tokenService, IRefreshTokenService rtService, IConfiguration configuration)
        {
            _service = service;
            _tokenService = tokenService;
            _configuration = configuration;
            _refreshTokenService = rtService;
        }

        // GET: api/<AuthController>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                Tokens token = _service.Login(login.Username, login.Password);
                
                // Set the refresh token as an HTTP-only cookie
                Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production
                    Expires = DateTime.UtcNow.AddHours(Double.Parse(_configuration["JwtOptions:RefreshTokenExpirationHours"]))
                });
                // Set the token as an HTTP-only cookie
                Response.Cookies.Append("token", token.JWT, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production
                    Expires = DateTime.UtcNow.AddSeconds(Double.Parse(_configuration["JwtOptions:Expiration"]))
                });

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
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refresh)
        {
            try
            {
                ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity is null)
                {
                    throw new Exception("Aucun token trouvé ou Token vide");
                }

                List<Claim> claims = identity.Claims.ToList();
                
                string idFromToken = claims.FirstOrDefault((claim) => (claim.Type == ClaimTypes.NameIdentifier)).Value;

                if (idFromToken is null)
                {
                    throw new Exception("Token invalide");
                }

                int id = int.Parse(idFromToken);

                // Validate the refresh token (retrieve the stored refresh token from the database)
                Tokens newToken = _refreshTokenService.GetRefreshedToken(refresh, id);

                return Ok(new { token = newToken.JWT, refreshToken = newToken.RefreshToken });
            }
            catch(Exception exc)
            {
                return Unauthorized(exc.Message);
            }
        }

        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword([FromBody] PasswordChangeForm passwordChangeForm)
        {
            if (passwordChangeForm.OldPassword == passwordChangeForm.NewPassword)
            {
                return BadRequest("Le nouveau mot de passe ne doit pas être le même que l'ancien!");
            }
            try
            {
                // TODO check token valid ? ou null
				ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
				if (identity is null)
				{
					throw new Exception("Aucun token trouvé ou Token vide");
				}

				List<Claim> claims = identity.Claims.ToList();

				string idFromToken = claims.FirstOrDefault((claim) => (claim.Type == ClaimTypes.NameIdentifier)).Value;
				
                _service.ChangePassword(int.Parse(idFromToken), passwordChangeForm.OldPassword, passwordChangeForm.NewPassword);

			}
            catch(Exception exception)
			{
				return Unauthorized(exception.Message);
			}

			return NoContent();
        }
    }
}
