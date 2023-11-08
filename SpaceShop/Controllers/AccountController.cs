using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpaceShop.Dto;
using SpaceShop.Interfaces;
using SpaceShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpaceShop.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration config;

        public AccountController(IUnitOfWork uow, IConfiguration config)
        {
            this.uow = uow;
            this.config = config;
        }

        // Authenticate User: POST api/Account/login

        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginReqDto loginReq)
        {
            var user = await uow.UserRepository.Authenticate(loginReq.Username, loginReq.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var loginRes = new LoginResDto();
            loginRes.Username = user.Username;
            loginRes.Token = CreateJWT(user);

            return Ok(loginRes);
        }

        // Register User: POST api/account/register 

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginReqDto loginReq)
        {
            if (await uow.UserRepository.UserAlreadyExists(loginReq.Username))
                return BadRequest("Username already exists, please try something else");

            uow.UserRepository.Register(loginReq.Username, loginReq.Password);
            await uow.SaveAsync();

            return StatusCode(201);
        }

        // This method creates JWT token for authentication. 
            private string CreateJWT(User user)
        {
            var secretkey = config.GetSection("AppSettings:key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
