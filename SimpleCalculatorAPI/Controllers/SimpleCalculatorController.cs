using SimpleCalculatorAPI.Models;
using SimpleCalculatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace SimpleCalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleCalculatorController : ControllerBase
    {
        private readonly ISimpleCalculatorService _calculatorService;
        private readonly ILogger<SimpleCalculatorController> _logger;

        public SimpleCalculatorController(ISimpleCalculatorService calculatorService, ILogger<SimpleCalculatorController> logger) 
        {
            _calculatorService = calculatorService;
            _logger = logger;
        }

        //Login method responsible for authenticating users. Upon successful login, a JWT token is provided.
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                      .Select(e => e.ErrorMessage)
                                                      .ToList();
                _logger.LogError("Validation errors: {Errors}", string.Join(", ", errorMessages));
                return BadRequest(new { Errors = errorMessages });
            }

            var secureKey = Environment.GetEnvironmentVariable("TEST_SECURE_KEY");
            if (string.IsNullOrEmpty(secureKey))
            {
                _logger.LogError("Secure key is not configured.");
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
            _logger.LogWarning("Invalid username or password.");
            return Unauthorized("Invalid username or password.");
        }

        //Addition operation method responsible for performing the add calculation provided that user is authorized
        //using the valid JWT token and valid input arguments are provided.
        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<double>> Add([FromBody] CalculationRequest calculationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();
                    _logger.LogError("Validation errors: {Errors}", string.Join(", ", errorMessages));
                    return BadRequest(new { Errors = errorMessages });
                }

                var result = await _calculatorService.AddAsync(calculationRequest.Operand1, calculationRequest.Operand2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Add operation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Subtraction operation method responsible for performing the subtract calculation provided that user is authorized
        //using the valid JWT token and valid input arguments are provided.
        [Authorize]
        [HttpPost("subtract")]
        public async Task<ActionResult<double>> Subtract([FromBody] CalculationRequest calculationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();
                    _logger.LogError("Validation errors: {Errors}", string.Join(", ", errorMessages));
                    return BadRequest(new { Errors = errorMessages });
                }

                var result = await _calculatorService.SubtractAsync(calculationRequest.Operand1, calculationRequest.Operand2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Subtract operation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Multiply operation method responsible for performing the multiply calculation provided that user is authorized
        //using the valid JWT token and valid input arguments are provided.
        [Authorize]
        [HttpPost("multiply")]
        public async Task<ActionResult<double>> Multiply([FromBody] CalculationRequest calculationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();
                    _logger.LogError("Validation errors: {Errors}", string.Join(", ", errorMessages));
                    return BadRequest(new { Errors = errorMessages });
                }

                var result = await _calculatorService.MultiplyAsync(calculationRequest.Operand1, calculationRequest.Operand2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Multiply operation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Division operation method responsible for performing the divide calculation provided that user is authorized
        //using the valid JWT token and valid input arguments are provided.
        [Authorize]
        [HttpPost("divide")]
        public async Task<ActionResult<double>> Divide([FromBody] CalculationRequest calculationRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();
                _logger.LogError("Validation errors: {Errors}", string.Join(", ", errorMessages));
                return BadRequest(new { Errors = errorMessages });
            }

            try
            {
                var result = await _calculatorService.DivideAsync(calculationRequest.Operand1, calculationRequest.Operand2);
                return Ok(result);
            }
            catch (DivideByZeroException ex) 
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
