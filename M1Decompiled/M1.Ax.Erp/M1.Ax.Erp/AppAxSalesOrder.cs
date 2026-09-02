using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("SalesOrder")]
[ComVisible(true)]
public class AppAxSalesOrder : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxSalesOrder(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public string CreateSalesOrder(string customerID, string locationID, string currencyID, string partID, string revisionID, decimal orderQty = 1m, string orderID = "", string shipOrgID = "", string shipLocationID = "", string customerPO = "", string orgContactID = "", string shipContactID = "")
	{
		return new SalesOrder().CreateSalesOrder(_Database, customerID, locationID, currencyID, partID, revisionID, orderQty, orderID, shipOrgID, shipLocationID, customerPO, orgContactID, shipContactID);
	}

	public void AddDeliveryLine(M1BindingSource bindingSource, object transaction)
	{
		new SalesOrder().AddDeliveryLine(bindingSource, (SqlTransaction)transaction);
	}

	public void UpdatePurchaseUnitCost(M1BindingSource bindingSource)
	{
		new SalesOrder().UpdatePurchaseUnitCost(bindingSource);
	}

	public void DefaultDeliveryQuantity(M1BindingSource bindingSource, DataRow deliveryRow)
	{
		new SalesOrder().DefaultDeliveryQuantity(bindingSource, deliveryRow);
	}

	public void UpdateQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new SalesOrder().UpdateQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}

	public void CreateSalesOrderJobLinks(string orderID, int salesOrderLineID, int salesOrderJobLinkID, int linkType, int salesOrderDeliveryID, string jobID, bool closed, string createdBy, DateTime createdDate)
	{
		new SalesOrder().CreateSalesOrderJobLinks(_Database, null, orderID, salesOrderLineID, salesOrderJobLinkID, linkType, salesOrderDeliveryID, jobID, closed, createdBy, createdDate);
	}

	public void RefreshTaxSubtotal(M1BindingSource bsOrder, SqlTransaction transaction)
	{
		new SalesOrder().RefreshTaxSubtotal(_Database, bsOrder, transaction);
	}

	public bool RefreshOrderTotal(string orderID, bool forceApprovalCheck = false)
	{
		return new SalesOrder().RefreshOrderTotal(_Database, orderID, forceApprovalCheck);
	}

	public void Dispose()
	{
		_Database = null;
		provider = null;
	}
}
