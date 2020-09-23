using AutoMapper;
using PaymentApi.Models.Models;
using PaymentApi.Models.Models.Dtos;

namespace PaymentApi.Models.Mapper
{
	public class PaymentApiMapper : Profile
	{
		public PaymentApiMapper()
		{
			CreateMap<Account, AccountInsertDto>().ReverseMap();
			CreateMap<Transaction, TransactionInsertDto>().ReverseMap();
			CreateMap<Transaction, TransactionResultDto>().ReverseMap();
			CreateMap<Transaction, PaymentTransactionResultDto>()
				.ForMember(t => t.TransactionStatus, opt => opt.MapFrom(src => src.TransactionStatus.ToString()))
				.ForMember(t => t.ClosedReason, opt => opt.MapFrom(src => src.ClosedReason ?? string.Empty));
			CreateMap<Transaction, DepositTransactionResultDto>();
		}
	}
}