namespace PaymentApi.Services.Interfaces
{
	public interface IServiceResult
	{
		string ContentResult { get; set; }
		int StatusCode { get; set; }
	}
}