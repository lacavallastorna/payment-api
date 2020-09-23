using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models.Models.Dtos
{
	public class TransactionInsertDto
	{
		[Required]
		public int? AccountId { get; set; }

		[Required]
		[Range(0.01, 999999999999999.99, ErrorMessage = "Amount must be between 0.01 and 999999999999999.99")]
		public decimal? Amount { get; set; }

		[Required]
		public DateTime? Date { get; set; }
	}
}