using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;

namespace ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces
{
    public interface ITaxBandRepository
    {
        Task<IList<TaxBand>> GetAllAsync();
    }
}
