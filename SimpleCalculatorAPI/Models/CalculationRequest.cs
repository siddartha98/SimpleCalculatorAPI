using System.ComponentModel.DataAnnotations;

namespace SimpleCalculatorAPI.Models
{
    public class CalculationRequest
    {   //Declared the operand variables that will store the input arugments provided by user.
        [Required(ErrorMessage = "Operand 1 is a required field.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Please enter a valid number (whole or decimal).")]
        public double Operand1 { get; set; }

        [Required(ErrorMessage = "Operand 2 is a required field.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Please enter a valid number (whole or decimal).")]
        public double Operand2 { get; set; }
    }
}
