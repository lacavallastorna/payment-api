namespace PaymentApi.DataAccess.Interfaces
{
	public interface IDbInitializer
	{
		void Initialize();

		void SeedData();
	}
}