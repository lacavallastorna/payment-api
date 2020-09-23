using PaymentApi.Services.Interfaces;

namespace PaymentApi.Services.Services
{
	public class ServiceResult : IServiceResult
	{
		public int StatusCode { get; set; }
		public string ContentResult { get; set; }
	}
}