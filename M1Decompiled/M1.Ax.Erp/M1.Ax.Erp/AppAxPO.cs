using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("PO")]
[ComVisible(true)]
public class AppAxPO
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxPO(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool IsPurchaseOrderLandedCost(string purchaseOrderID)
	{
		return new PurchaseOrders().IsPurchaseOrderLandedCost(_Database, purchaseOrderID);
	}

	public byte GetPurchaseOrderLineType(string poID, int poLineID, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new PurchaseOrders().GetPurchaseOrderLineType(_Database, (SqlTransaction)transaction, poID, poLineID);
	}

	public void UpdateQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new PurchaseOrders().UpdateQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}

	public decimal GetTotalComponentsCost(M1BindingSource bindingSource, DataRow currentRow)
	{
		return new PurchaseOrders().GetTotalComponentsCost(bindingSource, currentRow);
	}

	public void RefreshTaxSubtotal(M1BindingSource bsOrder, SqlTransaction transaction)
	{
		new PurchaseOrders().RefreshTaxSubtotal(_Database, bsOrder, transaction);
	}

	public bool RefreshPurchaseOrderTotal(string orderID, object transaction = null)
	{
		return new PurchaseOrders().RefreshPurchaseOrderTotal(_Database, orderID, (SqlTransaction)transaction);
	}
}
