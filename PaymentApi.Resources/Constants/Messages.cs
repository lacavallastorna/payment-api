namespace PaymentApi.Resources.Constants
{
	public class Messages
	{
		public const string Account_FailedToCreate = "Error: Failed to create new Account.";
		public const string Account_InvalidAccountId = "Error: Invalid Account Id.";
		public const string Account_AccountNotFound = "Error: Account not found.";

		public const string Deposit_FailedToCreate = "Error: Failed to create new Deposit.";

		public const string Payment_FailedToProcess = "Error: Failed to process Payment.";
		public const string Payment_StatusIsClosed = "Error: Payment Status is Closed.";
		public const string Payment_StatusIsProcessed = "Error: Payment Status is Processed.";
		public const string Payment_NotFound = "Error: Payment not found.";
		public const string Payment_FailedToCancel = "Error: Failed to Cancel Payment.";
		public const string Payment_FailedToCreate = "Error: Failed to create Payment request.";

		public const string Payment_NotEnoughFundsReason = "Not enough funds";
	}
}