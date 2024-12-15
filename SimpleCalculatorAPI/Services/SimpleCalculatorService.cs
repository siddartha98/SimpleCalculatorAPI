namespace SimpleCalculatorAPI.Services
{
    public class SimpleCalculatorService : ISimpleCalculatorService
    {
        //The use of async programming here is to ensure scalability and future proof.
        /*Using Task.FromResult() ensures the result returned complies with async
        programming as method expects a Task to be returned. This syntax wraps the result
        in a Task.*/
        public Task<double> AddAsync(double x, double y) => Task.FromResult(x + y);

        public Task<double> SubtractAsync(double x, double y) => Task.FromResult(x - y);

        public Task<double> MultiplyAsync(double x, double y) => Task.FromResult(x * y);

        public Task<double> DivideAsync(double x, double y)
        {
            if (y == 0)
            {
                throw new DivideByZeroException("Division by zero is not valid.");
            }

            return Task.FromResult(x / y);
        }
    }
}
