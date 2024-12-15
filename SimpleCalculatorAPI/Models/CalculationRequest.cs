using System.ComponentModel.DataAnnotations;

namespace SimpleCalculatorAPI.Models
{
    public class CalculationRequest
    {
        [Required(ErrorMessage = "Operand 1 is a required field.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Please enter a valid number (whole or decimal).")]
        public double Operand1 { get; set; }
        [Required(ErrorMessage = "Operand 2 is a required field.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Please enter a valid number (whole or decimal).")]
        public double Operand2 { get; set; }
    }
}
