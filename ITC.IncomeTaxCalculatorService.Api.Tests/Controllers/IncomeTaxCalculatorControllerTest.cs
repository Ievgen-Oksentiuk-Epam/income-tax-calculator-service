using ITC.IncomeTaxCalculatorService.Api.Controllers;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;

namespace ITC.IncomeTaxCalculatorService.Api.Tests.Controllers;

public class IncomeTaxCalculatorControllerTest
{
    private Mock<ITaxCalculationService> _taxCalculationServiceMock;

    private IncomeTaxCalculatorController _subject;

    [SetUp]
    public void Setup()
    {
        _taxCalculationServiceMock = new Mock<ITaxCalculationService>();

        _subject = new IncomeTaxCalculatorController(_taxCalculationServiceMock.Object);
    }

    [Test]
    public async Task CalculateIncomeTax_ReturnsTaxCalculation_WhenAnnualIncomeProvided()
    {
        // Arrange
        var annualIncome = 40000;
        var expected = new TaxCalculation
        {
            GrossAnnualSalary = 40000,
            GrossMonthlySalary = 3333,
            NetAnnualSalary = 29000,
            NetMonthlySalary = 2416,
            AnnualTaxPaid = 11000,
            MonthlyTaxPaid = 916
        };

        _taxCalculationServiceMock.Setup(s => s.GetTaxCalculation(annualIncome)).ReturnsAsync(expected);

        // Act
        var actual = await _subject.CalculateIncomeTax(annualIncome);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}