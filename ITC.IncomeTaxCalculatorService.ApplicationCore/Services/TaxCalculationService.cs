using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;

namespace ITC.IncomeTaxCalculatorService.ApplicationCore.Services
{
    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly ITaxBandRepository _taxBandRepository;
        private readonly IMemoryCacheService _memoryCacheService;

        public TaxCalculationService(ITaxBandRepository taxBandRepository, IMemoryCacheService memoryCacheService)
        {
            _taxBandRepository = taxBandRepository;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<TaxCalculation> GetTaxCalculation(decimal annualIncome)
        {
            IList<TaxBand> taxBands = await GetTaxBandsAsync();

            decimal annualTaxPaid = 0;
            foreach (var taxCalculation in taxBands)
            {
                if (annualIncome <= taxCalculation.LowerLimit)
                    break;

                decimal salaryInBand = taxCalculation.UpperLimit.HasValue && annualIncome > taxCalculation.UpperLimit ?
                    taxCalculation.UpperLimit.Value - taxCalculation.LowerLimit
                    : annualIncome - taxCalculation.LowerLimit;

                annualTaxPaid += salaryInBand * taxCalculation.TaxRate / 100;
            }

            var result = new TaxCalculation
            {
                AnnualTaxPaid = Math.Round(annualTaxPaid, 2),
                MonthlyTaxPaid = Math.Round(annualTaxPaid / 12, 2),
                GrossAnnualSalary = annualIncome,
                GrossMonthlySalary = Math.Round(annualIncome / Constants.MonthsInYear, 2),
                NetAnnualSalary = Math.Round(annualIncome - annualTaxPaid, 2),
                NetMonthlySalary = Math.Round((annualIncome - annualTaxPaid) / Constants.MonthsInYear, 2),
            };

            return result;
        }

        private async Task<IList<TaxBand>> GetTaxBandsAsync()
        {
            if (_memoryCacheService.TryGetValue(Constants.TaxBandsCacheKey, out IList<TaxBand> taxBands))
                return taxBands;

            taxBands = await _taxBandRepository.GetAllAsync();

            _memoryCacheService.SetValue(Constants.TaxBandsCacheKey, taxBands);

            return taxBands;
        }
    }
}
