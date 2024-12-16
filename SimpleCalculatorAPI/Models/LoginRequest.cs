using System.ComponentModel.DataAnnotations;

namespace SimpleCalculatorAPI.Models
{
    public class LoginRequest
    {
        //As a user login is simply simulated on Swagger for security purposes through user auth, the username and password
        //implemented can be found in the Login controller action method. Hence, strict validation isn't enforced except for
        //a regex to apply basic validation and security.
        [Required(ErrorMessage = "Username is a required field.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        public string? Password { get; set; }
    }
}
