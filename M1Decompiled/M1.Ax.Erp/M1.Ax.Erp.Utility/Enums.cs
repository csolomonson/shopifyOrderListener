namespace M1.Ax.Erp.Utility;

public static class Enums
{
	public enum ChangeTypeId : short
	{
		NewId = 1,
		KeepSource,
		KeepDestination
	}

	public enum SupplierStatus : short
	{
		None,
		Prospective,
		Active,
		Inactive
	}

	public enum PriceType : short
	{
		PurchasePrice = 1,
		SellPrice
	}
}
