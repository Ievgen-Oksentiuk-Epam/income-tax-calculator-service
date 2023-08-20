using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Services;

namespace ITC.IncomeTaxCalculatorService.ApplicationCore.Tests.Services;

public class TaxCalculationServiceTest
{
    private Mock<ITaxBandRepository> _taxBandRepositoryMock;
    private Mock<IMemoryCacheService> _memoryCacheServiceMock;

    private TaxCalculationService _subject;

    [SetUp]
    public void Setup()
    {
        _taxBandRepositoryMock = new Mock<ITaxBandRepository>();
        _memoryCacheServiceMock = new Mock<IMemoryCacheService>();

        _subject = new TaxCalculationService(_taxBandRepositoryMock.Object, _memoryCacheServiceMock.Object);
    }

    [TestCase(0, 0, 0, 0, 0, 0, 0)]
    [TestCase(40000, 40000, 3333.33, 29000, 2416.67, 11000, 916.67)]
    [TestCase(10000, 10000, 833.33, 9000, 750, 1000, 83.33)]
    [TestCase(999999999.99, 999999999.99, 83333333.33, 600004999.99, 50000416.67, 399995000, 33332916.67)]
    public async Task GetTaxCalculation_ReturnsTaxCalculation_WhenAnnualIncomeProvided(
        decimal annualIncome,
        decimal grossAnnualSalary,
        decimal grossMonthlySalary,
        decimal netAnnualSalary,
        decimal netMonthlySalary,
        decimal annualTaxPaid,
        decimal monthlyTaxPaid)
    {
        // Arrange
        _taxBandRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaxBand> {
            new TaxBand { Id = 1, Name = "A", LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
            new TaxBand { Id = 2, Name = "B", LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
            new TaxBand { Id = 3, Name = "C", LowerLimit = 20000, TaxRate = 40 }
        });

        // Act
        var actual = await _subject.GetTaxCalculation(annualIncome);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.GrossAnnualSalary, Is.EqualTo(grossAnnualSalary));
            Assert.That(actual.GrossMonthlySalary, Is.EqualTo(grossMonthlySalary));
            Assert.That(actual.NetAnnualSalary, Is.EqualTo(netAnnualSalary));
            Assert.That(actual.NetMonthlySalary, Is.EqualTo(netMonthlySalary));
            Assert.That(actual.AnnualTaxPaid, Is.EqualTo(annualTaxPaid));
            Assert.That(actual.MonthlyTaxPaid, Is.EqualTo(monthlyTaxPaid));
        });
    }

    [Test]
    public async Task GetTaxCalculation_UsesTaxBandsFromCache_WhenCacheContainsTaxBands()
    {
        // Arrange
        var annualIncome = 40000;

        IList<TaxBand> taxBands = new List<TaxBand> { new TaxBand() };
        _memoryCacheServiceMock.Setup(s => s.TryGetValue(Constants.TaxBandsCacheKey, out taxBands)).Returns(true);

        // Act
        await _subject.GetTaxCalculation(annualIncome);

        // Assert
        _memoryCacheServiceMock.Verify(s => s.TryGetValue(Constants.TaxBandsCacheKey, out taxBands), Times.Once());
    }

    [Test]
    public async Task GetTaxCalculation_SetsRetrievedTaxBandsToCache_WhenCacheDoesNotContainTaxBands()
    {
        // Arrange
        var annualIncome = 40000;

        List<TaxBand> taxBands = new() { new TaxBand() };
        _taxBandRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(taxBands);

        // Act
        await _subject.GetTaxCalculation(annualIncome);

        // Assert
        _memoryCacheServiceMock.Verify(s => s.SetValue<IList<TaxBand>>(Constants.TaxBandsCacheKey, taxBands), Times.Once());
    }
}