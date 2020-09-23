using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentApi.Models.Models
{
	public class Transaction
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int AccountId { get; set; }

		[ForeignKey(nameof(AccountId))]
		public Account Account { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,3)")]
		[Range(0.01, 999999999999999.99, ErrorMessage = "Amount must be between 0.01 and 999999999999999.99")]
		public decimal Amount { get; set; }

		[Required]
		public TransactionTypeEnum TransactionType { get; set; }

		[Required]
		public TransactionStatusEnum TransactionStatus { get; set; }

		[Required]
		public DateTime CreationDate { get; set; }

		[Required]
		public DateTime LastUpdateDate { get; set; }

		public string ClosedReason { get; set; }
	}
}