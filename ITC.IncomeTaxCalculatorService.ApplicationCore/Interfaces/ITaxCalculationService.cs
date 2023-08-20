using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;

namespace ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces
{
    public interface ITaxCalculationService
    {
        Task<TaxCalculation> GetTaxCalculation(decimal annualIncome);
    }
}
