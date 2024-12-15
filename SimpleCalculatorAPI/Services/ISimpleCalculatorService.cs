namespace SimpleCalculatorAPI.Services
{
    public interface ISimpleCalculatorService
    {
        Task<double> AddAsync(double x, double y);
        Task<double> SubtractAsync(double x, double y);
        Task<double> MultiplyAsync(double x, double y);
        Task<double> DivideAsync(double x, double y);
    }
}
