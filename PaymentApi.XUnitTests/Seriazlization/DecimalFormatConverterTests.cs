using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentApi.Services.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace PaymentApi.XUnitTests.Seriazlization
{
	public class DecimalFormatConverterTests
	{
		[Fact]
		public void DecimalFormatConverter_FiveDecimals_ExpectTwoDecimalsInResult()
		{
			var obj = new { Amount = 10.12332m };
			var result = JsonConvert.SerializeObject(obj, new DecimalFormatConverter());
			result.Should().Be("{\"Amount\":10.12}");
		}

		[Fact]
		public void DecimalFormatConverter_FiveDecimalsZero_ExpectTwoDecimalsInResult()
		{
			var obj = new { Amount = 10.00000m };
			var result = JsonConvert.SerializeObject(obj, new DecimalFormatConverter());
			result.Should().Be("{\"Amount\":10.00}");
		}

		[Fact]
		public void DecimalFormatConverter_OneDecimalsZero_ExpectTwoDecimalsInResult()
		{
			var obj = new { Amount = 10.7m };
			var result = JsonConvert.SerializeObject(obj, new DecimalFormatConverter());
			result.Should().Be("{\"Amount\":10.70}");
		}

		[Fact]
		public void DecimalFormatConverter_NoDecimalsZero_ExpectTwoDecimalsInResult()
		{
			var obj = new { Amount = 10m };
			var result = JsonConvert.SerializeObject(obj, new DecimalFormatConverter());
			result.Should().Be("{\"Amount\":10.00}");
		}
	}
}