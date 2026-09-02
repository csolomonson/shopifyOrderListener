using System.Collections.Generic;

namespace M1.Core;

public static class ConcurrencyHelper
{
	private const string SALESORDERLINES = "SalesOrderLines";

	private const string PURCHASEORDERLINES = "PurchaseOrderLines";

	public static List<string> ExtraVerificationTableNames = new List<string> { "SalesOrderLines", "PurchaseOrderLines" };
}
