using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITC.IncomeTaxCalculatorService.Infrastructure.Data.Repositories
{
    public class TaxBandRepository : ITaxBandRepository
    {
        private readonly TaxCalculatorContext _dbContext;

        public TaxBandRepository(TaxCalculatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<TaxBand>> GetAllAsync()
        {
            return await _dbContext.Set<TaxBand>().ToListAsync();
        }
    }
}