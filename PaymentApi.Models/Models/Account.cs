using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models.Models
{
	public class Account
	{
		[Key]
		public int Id { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required]
		public DateTime CreationDate { get; set; }
	}
}