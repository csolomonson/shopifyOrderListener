using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferSalesOrderToPickListProcess : ProcessParameters
{
	private static readonly byte[] AllowedDeliveryTypes = new byte[3] { 2, 4, 5 };

	public TransferSalesOrderToPickListProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "omdSalesOrderID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[3] { "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID" };
		KeyValueTableName = "SalesOrderDeliveries";
		Description = "Select the sales order deliveries to be picked.";
		GridID = "M1ADDFROMPICKLISTSO";
		BindingSourceTable = "SalesOrderPickListSessions";
		HelpLink = "OM_TransferSalesOrderToPickList.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("ompClosed", fieldValue: false, "Sales Order is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Add Firm Deliveries Only?")
		{
			AdoFilterExpression = "omdFirm <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "omdFirm"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Delivery Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "omdDeliveryDate",
			AdditionalFields = "omdDeliveryDate"
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		MatchingFieldsInfo matchingFieldsInfo = (databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary).FindMatchingFields("SalesOrderDeliveries", "SalesOrderPickListLines", new string[9] { "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omdPartID", "omdPartRevisionID", "omdPartWarehouseLocationID", "omdPartBinID", "omdDeliveryDate", "OpenQuantity=omdDeliveryQuantity - omdQuantityShipped" }, new string[9] { "omySalesOrderID", "omySalesOrderLineID", "omySalesOrderDeliveryID", "omyPartID", "omyPartRevisionID", "omyPartWarehouseLocationID", "omyPartBinID", "omyDeliveryDate", "omyOpenQuantity" });
		DataTable dataTable = databaseForRow.GetDataTable("select " + matchingFieldsInfo.GetSourceFieldList(string.Empty, string.Empty) + ", omdDeliveryType from SalesOrderDeliveries inner join SalesOrderLines on omdSalesOrderID=omlSalesOrderID and omdSalesOrderLineID=omlSalesOrderLineID inner join SalesOrders on omdSalesOrderID = ompSalesOrderID where " + text + " order by omdDeliveryDate,omdSalesOrderID,omdSalesOrderLineID,omdSalesOrderDeliveryID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("SalesOrderPickListLines");
		DataTable dataTable2 = childBindingSource.GetDataTable();
		foreach (DataRow row in dataTable.Rows)
		{
			byte value = row.Field<byte>("omdDeliveryType");
			if (AllowedDeliveryTypes.Contains(value))
			{
				if (dataTable2.Select("omySalesOrderID = " + row.Field<string>("omdSalesOrderID").ToLinq() + " And omySalesOrderLineID = " + row.Field<short>("omdSalesOrderLineID").ToLinq() + " And omySalesOrderDeliveryID = " + row.Field<short>("omdSalesOrderDeliveryID").ToLinq()).Length == 0)
				{
					TransferLineInfo(this, row, childBindingSource, matchingFieldsInfo);
				}
				continue;
			}
			string text2 = "Pull From Stock, Kit Part, or Purchase To Order";
			messages.Add(string.Format("Order {0} / Line {1} / Delivery {2} was not added because the delivery type is not one of the following: {3}.", row.Field<string>("omdSalesOrderID"), row.Field<short>("omdSalesOrderLineID"), row.Field<short>("omdSalesOrderDeliveryID"), text2));
		}
	}
}
