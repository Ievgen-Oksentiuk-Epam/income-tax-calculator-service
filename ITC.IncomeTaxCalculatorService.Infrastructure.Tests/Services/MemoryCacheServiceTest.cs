using ITC.IncomeTaxCalculatorService.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ITC.IncomeTaxCalculatorService.Infrastructure.Tests.Services
{
    public class MemoryCacheServiceTest
    {
        private Mock<IMemoryCache> _memoryCacheMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogger<MemoryCacheService>> _loggerMock;

        private MemoryCacheService _subject;

        [SetUp]
        public void Setup()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<MemoryCacheService>>();

            _subject = new MemoryCacheService(_memoryCacheMock.Object, _configurationMock.Object, _loggerMock.Object);
        }

        [Test]
        public void TryGetValue_ReturnsTrueAndValue_WhenValueIsCached()
        {
            // Arrange
            _configurationMock.Setup(c => c["MemoryCacheExpiration"]).Returns("5");

            object expectedValue = "cached_value";
            _memoryCacheMock.Setup(m => m.TryGetValue(string.Empty, out expectedValue!)).Returns(true);

            _subject = new MemoryCacheService(_memoryCacheMock.Object, _configurationMock.Object, _loggerMock.Object);

            // Act
            var actual = _subject.TryGetValue(string.Empty, out string actualValue);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.True);
                Assert.That(actualValue, Is.EqualTo(expectedValue));
            });
        }

        [Test]
        public void TryGetValue_ReturnsFalse_WhenAbsoluteExpirationIs0()
        {
            // Arrange
            _configurationMock.Setup(c => c["MemoryCacheExpiration"]).Returns("0");

            _subject = new MemoryCacheService(_memoryCacheMock.Object, _configurationMock.Object, _loggerMock.Object);

            // Act
            var actual = _subject.TryGetValue(string.Empty, out string value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.False);
                Assert.That(value, Is.Null);
            });

            object cachedValue;
            _memoryCacheMock.Verify(m => m.TryGetValue(string.Empty, out cachedValue!), Times.Never());
        }

        [Test]
        public void TryGetValue_ReturnsFalse_WhenAbsoluteExpirationIsInvalid()
        {
            // Arrange
            _configurationMock.Setup(c => c["MemoryCacheExpiration"]).Returns("n/a");

            _subject = new MemoryCacheService(_memoryCacheMock.Object, _configurationMock.Object, _loggerMock.Object);

            // Act
            var actual = _subject.TryGetValue(string.Empty, out string value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.False);
                Assert.That(value, Is.Null);
            });
        }

        [Test]
        public void SetValue_AddsValueToCache_WhenAbsoluteExpirationIsSet()
        {
            // Arrange
            _configurationMock.Setup(c => c["MemoryCacheExpiration"]).Returns("5");

            string cacheKey = "cache_key";
            object expectedValue = "cached_value";

            Mock<ICacheEntry> cacheEntry = new Mock<ICacheEntry>();

            _memoryCacheMock.Setup(m => m.CreateEntry(cacheKey)).Returns(cacheEntry.Object);

            _subject = new MemoryCacheService(_memoryCacheMock.Object, _configurationMock.Object, _loggerMock.Object);

            // Act
            _subject.SetValue(cacheKey, expectedValue);

            // Assert
            cacheEntry.VerifySet(e => e.Value = expectedValue);
        }

        [Test]
        public void SetValue_DoesNotAddValueToCache_WhenAbsoluteExpirationIs0()
        {
            // Arrange
            _configurationMock.Setup(c => c["MemoryCacheExpiration"]).Returns("0");

            string cacheKey = "cache_key";
            object expectedValue = "cached_value";

            Mock<ICacheEntry> cacheEntry = new Mock<ICacheEntry>();

            _memoryCacheMock.Setup(m => m.CreateEntry(cacheKey)).Returns(cacheEntry.Object);

            _subject = new MemoryCacheService(_memoryCacheMock.Object, _configurationMock.Object, _loggerMock.Object);

            // Act
            _subject.SetValue(cacheKey, expectedValue);

            // Assert
            cacheEntry.VerifySet(e => e.Value = It.IsAny<string>(), Times.Never());
        }
    }
}
