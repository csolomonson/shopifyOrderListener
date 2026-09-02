using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("PurchasePlanner")]
[ComVisible(true)]
public class AppAxPurchasePlanner : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxPurchasePlanner(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool Generate(object sessionID, object lineID)
	{
		return new PurchasePlanner().Generate(_Database, Convert.ToString(sessionID), Convert.ToInt16(lineID));
	}

	public void Clear(object sessionID)
	{
		new PurchasePlanner().Clear(_Database, Convert.ToString(sessionID));
	}

	public bool MissingLastRunDates()
	{
		return new PurchasePlanner().MissingLastRunDates(_Database);
	}

	public string MissingSuppliers(object sessionID)
	{
		return new PurchasePlanner().MissingSuppliers(_Database, Convert.ToString(sessionID));
	}

	public string VerifyInactiveParts(object sessionID)
	{
		return new PurchasePlanner().VerifyInactiveParts(_Database, Convert.ToString(sessionID));
	}

	public void AddSupplierRequirementsLine(M1BindingSource detailsBs, DataRow parentRow)
	{
		new PurchasePlanner().AddSupplierRequirementsLine(detailsBs, parentRow);
	}

	public void RefreshOrderDetailsPricing(M1BindingSource detailsBs, DataRow parentRow, DataRow currentSuppliersRow, bool applyToAllLines)
	{
		new PurchasePlanner().RefreshOrderDetailsPricing(detailsBs, parentRow, currentSuppliersRow, applyToAllLines);
	}

	public void CreateNewSession(string sourceTable, object topLevelIDs)
	{
		new PurchasePlanner().CreateNewSession(provider, _Database, sourceTable, (object[])topLevelIDs);
	}

	public void CompletePurchasePlanner(M1BindingSource plannerBindingSource)
	{
		new PurchasePlanner().CompletePurchasePlanner(plannerBindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}
}
