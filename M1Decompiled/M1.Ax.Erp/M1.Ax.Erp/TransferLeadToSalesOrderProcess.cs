using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferLeadToSalesOrderProcess : ProcessParameters
{
	public TransferLeadToSalesOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "lolLeadID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "lolLeadID", "lolLeadLineID" };
		KeyValueTableName = "LeadLines";
		Description = "Use this screen to transfer the selected lead details to an order.";
		GridID = "M1ADDFROMSOLEAD";
		HelpLink = "om_createorder.htm";
		BindingSourceTable = "SalesOrders";
		ContinueMessage = "This will create a sales order from the {0} selected lead lines. Are you sure you want to continue?";
		CreatedBindingSourceCaption = "Create Order from Lead";
		HeaderSourceFields = new string[15]
		{
			"lopCustomerOrganizationID", "lopLocationID", "lopContactID", "lopShipOrganizationID", "lopShipLocationID", "lopShipContactID", "lopLeadDate", "lopQuoteLocationID", "lopQuoteContactID", "lopPlantID",
			"lopPlantDepartmentID", "lopCurrencyRateID", "lopCustomRate", "lopExchangeRate", "lopProjectID"
		};
		HeaderDestinationFields = new string[15]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "ompShipOrganizationID", "ompShipLocationID", "ompShipContactID", "ompOrderDate", "ompQuoteLocationID", "ompQuoteContactID", "ompPlantID",
			"ompPlantDepartmentID", "ompCurrencyRateID", "ompCustomRate", "ompExchangeRate", "ompProjectID"
		};
		DefaultValueFieldNames = new string[2] { "ompCustomerPO", "ompRequestedShipDate" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary obj = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("Leads", "SalesOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("LeadLines, Leads", "SalesOrderLines", new string[9] { "lolLeadID", "lolLeadLineID", "lolPartID", "lolPartRevisionID", "lolOrgPartID", "lolOrgPartShortDescription", "lolUnitOfMeasure", "lolDescription", "lolPartGroupID" }, new string[9] { "omlLeadID", "omlLeadLineID", "omlPartID", "omlPartRevisionID", "omlOrgPartID", "omlOrgPartShortDescription", "omlUnitOfMeasure", "omlPartShortDescription", "omlPartGroupID" });
		DataTable dataTable = databaseForRow.GetDataTable("select lolQuantity, lolGrossAmountForeign, lolDiscountPercent " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from LeadLines  Inner Join Leads On lopLeadID = lolLeadID  where " + text + " order by lolLeadID, lolLeadLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("SalesOrderLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries");
		DataTable dataTable2 = childBindingSource2.GetDataTable();
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addOrderLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, childBindingSource2, dataTable2);
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		TransferSalespeopleToOrder(parm.BindingSource.Database, sourceHeaderRow.Field<string>("lolLeadID"), parm.BindingSource);
	}

	private void TransferSalespeopleToOrder(M1Database database, string sourceLeadID, M1BindingSource bsOrder)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select lojSalesEmployeeID, lojPercent From LeadSalesPeople Inner Join Employees on lmeEmployeeID = lojSalesEmployeeID Where lojLeadID = @LeadID And lmeSalesEmployee = 1 And lmeTerminationDate is null Order by lojSequenceID");
		sqlCommand.Parameters.Add(new SqlParameter("@LeadID", SqlDbType.NVarChar)).Value = sourceLeadID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = bsOrder.PrimaryTable.GetChildBindingSource("SalesOrderSalespeople");
		if (childBindingSource.Count != 0)
		{
			childBindingSource.RemoveWhere(string.Empty);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow obj = (DataRow)childBindingSource.AddNew();
			obj["omiSalesEmployeeID"] = row["lojSalesEmployeeID"];
			obj["omiPercent"] = row["lojPercent"];
		}
	}

	private void addOrderLine(M1Database database, DataRow soRow, M1BindingSource bsSOLines, DataRow lineRow, MatchingFieldsInfo lineMatches, M1BindingSource bsDeliveries, DataTable dtDeliveries)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsSOLines, lineMatches, soRow);
		decimal num = default(decimal);
		num = ((!(lineRow.Field<decimal>("lolQuantity") == 0m)) ? lineRow.Field<decimal>("lolQuantity") : 1m);
		dataRow["omlOrderQuantity"] = num;
		dataRow["omlFullUnitPriceForeign"] = M1Math.Round(lineRow.Field<decimal>("lolGrossAmountForeign") / num, 5);
		dataRow["omlDiscountPercent"] = lineRow.Field<decimal>("lolDiscountPercent");
		DataRow[] array = dtDeliveries.Select("omdSalesOrderID = " + dataRow.Field<string>("omlSalesOrderID").ToLinq() + " And omdSalesOrderLineID = " + dataRow.Field<short>("omlSalesOrderLineID").ToLinq());
		((array.Length != 0) ? array[0] : (bsDeliveries.AddNew(database, dataRow, null, null) as DataRow))?.SetField("omdDeliveryQuantity", dataRow.Field<decimal>("omlOrderQuantity"));
	}
}
