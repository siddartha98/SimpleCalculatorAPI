using SimpleCalculatorAPI.Services;

namespace SimpleCalculatorUnitTests
{
    public class SimpleCalculatorUnitTests
    {
        private readonly SimpleCalculatorService _calculatorService = new();

        [Fact]
        public async Task Add_ShouldReturnCorrectSum()
        {
            var result = await _calculatorService.AddAsync(3, 2);
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task Subtract_ShouldReturnCorrectDifference()
        {
            var result = await _calculatorService.SubtractAsync(6, 2);
            Assert.Equal(4, result);
        }

        [Fact]
        public async Task Multiply_ShouldReturnCorrectMultiple()
        {
            var result = await _calculatorService.MultiplyAsync(3, 2);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task Divide_ByZero_ShouldThrowException()
        {
            var exception = await Assert.ThrowsAsync<DivideByZeroException>(
                () => _calculatorService.DivideAsync(10, 0));
        }
    }
}