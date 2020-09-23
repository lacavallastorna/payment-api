using Newtonsoft.Json;
using System;

namespace PaymentApi.Models.Models.Dtos
{
	public class TransactionResultDto
	{
		[JsonProperty(Order = 1)]
		public int Id { get; set; }

		[JsonProperty(Order = 2)]
		public int AccountId { get; set; }

		[JsonProperty(Order = 3)]
		public decimal Amount { get; set; }

		[JsonProperty(Order = 4)]
		public DateTime Date { get; set; }

		[JsonProperty(Order = 5)]
		public virtual string TransactionStatus { get; set; }

		[JsonProperty(Order = 6)]
		public virtual string ClosedReason { get; set; }
	}
}