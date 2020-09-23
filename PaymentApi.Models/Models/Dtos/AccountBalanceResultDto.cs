using System.Collections.Generic;

namespace PaymentApi.Models.Models.Dtos
{
	public class AccountBalanceResultDto
	{
		public int AccountId { get; set; }
		public decimal OpeningBalance { get; set; }
		public decimal ProcessedPaymentsBalance { get; set; }
		public decimal PendingdPaymentsBalance { get; set; }
		public decimal ClosingBalance { get; set; }
		public List<DepositTransactionResultDto> Deposits { get; set; }
		public List<PaymentTransactionResultDto> Payments { get; set; }
	}
}