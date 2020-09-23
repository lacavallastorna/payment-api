using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models.Models.Dtos
{
	public class AccountInsertDto
	{
		[Required(AllowEmptyStrings = false)]
		[MinLength(4)]
		public string Name { get; set; }
	}
}