using Newtonsoft.Json;

namespace PaymentApi.Models.Models.Dtos
{
	public class DepositTransactionResultDto : TransactionResultDto
	{
		[JsonIgnore]
		public override string TransactionStatus { get; set; }

		[JsonIgnore]
		public override string ClosedReason { get; set; }
	}
}