namespace PaymentApi.Models.Interfaces.Dtos
{
	public interface ITransactionUpdaterDto
	{
		public int? AccountId { get; set; }
		public int? TransactionId { get; set; }
	}
}