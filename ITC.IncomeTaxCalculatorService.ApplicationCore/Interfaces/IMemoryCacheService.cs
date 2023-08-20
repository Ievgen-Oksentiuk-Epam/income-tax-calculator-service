namespace ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces
{
    public interface IMemoryCacheService
    {
        void SetValue<T>(string key, T value);
        bool TryGetValue<T>(string key, out T value);
    }
}