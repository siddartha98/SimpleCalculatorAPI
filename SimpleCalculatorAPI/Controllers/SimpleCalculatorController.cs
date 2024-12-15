using SimpleCalculatorAPI.Models;
using SimpleCalculatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using DotNetEnv;

namespace SimpleCalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleCalculatorController : ControllerBase
    {
        private readonly ISimpleCalculatorService _calculatorService;

        public SimpleCalculatorController(ISimpleCalculatorService calculatorService) 
        {
            _calculatorService = calculatorService;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var secureKey = Environment.GetEnvironmentVariable("TEST_SECURE_KEY");
            if (string.IsNullOrEmpty(secureKey))
            {
                return StatusCode(500, "Secure key is not configured.");
            }

            if(loginRequest.Username == "admin" && loginRequest.Password == "test")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secureKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, loginRequest.Username),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }
            return Unauthorized("Invalid username or password.");
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<double>> Add([FromBody] CalculationRequest calculationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _calculatorService.AddAsync(calculationRequest.Operand1, calculationRequest.Operand2);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("subtract")]
        public async Task<ActionResult<double>> Subtract([FromBody] CalculationRequest calculationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _calculatorService.SubtractAsync(calculationRequest.Operand1, calculationRequest.Operand2);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("multiply")]
        public async Task<ActionResult<double>> Multiply([FromBody] CalculationRequest calculationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _calculatorService.MultiplyAsync(calculationRequest.Operand1, calculationRequest.Operand2);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("divide")]
        public async Task<ActionResult<double>> Divide([FromBody] CalculationRequest calculationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _calculatorService.DivideAsync(calculationRequest.Operand1, calculationRequest.Operand2);
                return Ok(result);
            }
            catch (DivideByZeroException ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
