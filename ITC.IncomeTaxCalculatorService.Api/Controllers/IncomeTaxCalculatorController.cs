using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ITC.IncomeTaxCalculatorService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncomeTaxCalculatorController : ControllerBase
    {
        private readonly ITaxCalculationService _taxCalculationService;

        public IncomeTaxCalculatorController(ITaxCalculationService taxCalculationService)
        {
            _taxCalculationService = taxCalculationService;
        }

        [HttpPost("CalculateIncomeTax")]
        public async Task<TaxCalculation> CalculateIncomeTax([FromBody][RegularExpression(@"^\d{1,9}(\.\d{1,2})?$")] decimal annualIncome)
        {
            return await _taxCalculationService.GetTaxCalculation(annualIncome);
        }
    }
}