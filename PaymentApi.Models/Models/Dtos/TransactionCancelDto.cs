using PaymentApi.Models.Interfaces.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models.Models.Dtos
{
	public class TransactionCancelDto : ITransactionUpdaterDto
	{
		[Required]
		public int? AccountId { get; set; }

		[Required]
		public int? TransactionId { get; set; }

		public string Reason { get; set; }
	}
}