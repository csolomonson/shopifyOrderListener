using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

public class AR
{
	public void TransferOrderSalespeopleToInvoice(M1Database database, string orderID, M1BindingSource bsInvoice)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From SalesOrderSalespeople Where omiSalesOrderID = @OrderID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = bsInvoice.PrimaryTable.GetChildBindingSource("ARInvoiceSalespeople");
		if (childBindingSource.Count != 0)
		{
			childBindingSource.RemoveWhere(string.Empty);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow obj = (DataRow)childBindingSource.AddNew();
			obj["arjSalesEmployeeID"] = row["omiSalesEmployeeID"];
			obj["arjPercent"] = row["omiPercent"];
		}
	}

	public string PostInvoiceCheck(M1Database database, string invoiceID)
	{
		string text = string.Empty;
		SqlCommand sqlCommand = database.NewSqlCommand("Select distinct smpShipmentID from Shipments where smpPostedToGL = 0 and smpShipmentID in (select arlShipmentID from ARInvoiceLines where arlShipmentID = smpShipmentID and arlARInvoiceID =  @InvoiceID)");
		sqlCommand.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.NVarChar)).Value = invoiceID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				text = text + row["smpShipmentID"]?.ToString() + ", ";
			}
		}
		if (text != string.Empty)
		{
			text = text.Substring(0, text.Length - 2);
		}
		return text;
	}

	public void AddDepositsToInvoice(M1Database database, M1BindingSource bsInvoiceLines, DataRow invoiceRow, DataRow invoiceLineRow)
	{
		string value = invoiceLineRow.Field<string>("arlSalesOrderID");
		short num = invoiceLineRow.Field<short>("arlSalesOrderLineID");
		if (string.IsNullOrWhiteSpace(value) || num == 0 || invoiceLineRow.Field<short>("arlSalesOrderDeliveryID") == 0)
		{
			return;
		}
		bool flag = false;
		byte b = database.Props("FN").Field<byte>("xafARShowDeposits");
		switch (b)
		{
		case 1:
			flag = true;
			break;
		case 2:
			if (invoiceLineRow.Field<bool>("arlDeliveryInvoicedComplete"))
			{
				flag = true;
			}
			break;
		case 3:
			flag = true;
			break;
		}
		if (!flag)
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select arlARInvoiceID, arlARInvoiceLineID, arlExtendedPriceForeign, arlDepositTransferredForeign, arlPartShortDescription, arlPartGroupID, arlTaxCodeID,  arlTaxAmountForeign, arlSecondTaxCodeID, arlSecondTaxAmountForeign, arlFreightAmountForeign  From ARInvoices Inner Join ARInvoiceLines on arpARInvoiceID = arlARInvoiceID  Where (arpInvoiceType = 3 or (arpInvoiceType = 2 And arpDepositCredit = 1)) and arpDepositTransferredBase <> arpInvoiceTotalBase and arlDepositTransferredBase <> arlExtendedPriceBase and  arlSalesOrderID = @SalesOrderID and arlSalesOrderLineID = @SalesOrderLineID and arlSalesOrderDeliveryID = 0 and arlShipmentID = '' and arpARInvoiceID <> @InvoiceID and arpPostedToGL = 1  order by arpARInvoiceID, arlARInvoiceLineID ");
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.SmallInt)).Value = num;
		sqlCommand.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.NVarChar)).Value = invoiceLineRow.Field<string>("arlARInvoiceID");
		DataTable dataTable = database.GetDataTable(sqlCommand);
		SqlCommand sqlCommand2 = database.NewSqlCommand("Select IsNull(Sum(arlExtendedPriceForeign),0) As DepositAmountForeign, IsNull(Sum(arlTaxAmountForeign),0) As TaxAmountForeign, IsNull(Sum(arlSecondTaxAmountForeign),0) As SecondTaxAmountForeign, IsNull(Sum(arlFreightAmountForeign),0) As FreightAmountForeign From ARInvoiceLines Where arlDepositLine = 1 And arlSalesOrderID = @SalesOrderID and arlSalesOrderLineID = @SalesOrderLineID and arlSalesOrderDeliveryID = 0 and arlShipmentID = '' And (arlARInvoiceID <> @InvoiceID Or (arlArInvoiceID = @InvoiceID And arlARInvoiceLineID <> @InvoiceLine))");
		sqlCommand2.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.NVarChar)).Value = invoiceLineRow.Field<string>("arlARInvoiceID");
		sqlCommand2.Parameters.Add(new SqlParameter("@InvoiceLine", SqlDbType.SmallInt)).Value = invoiceLineRow.Field<short>("arlARInvoiceLineID");
		sqlCommand2.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = value;
		sqlCommand2.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.SmallInt)).Value = num;
		DataTable dataTable2 = database.GetDataTable(sqlCommand2);
		foreach (DataRow row3 in dataTable.Rows)
		{
			decimal num2 = row3.Field<decimal>("arlExtendedPriceForeign");
			decimal num3 = row3.Field<decimal>("arlTaxAmountForeign");
			decimal num4 = row3.Field<decimal>("arlSecondTaxAmountForeign");
			decimal num5 = row3.Field<decimal>("arlFreightAmountForeign");
			if (dataTable2.Rows.Count != 0)
			{
				num2 += dataTable2.Rows[0].Field<decimal>("DepositAmountForeign");
				num3 += dataTable2.Rows[0].Field<decimal>("TaxAmountForeign");
				num4 += dataTable2.Rows[0].Field<decimal>("SecondTaxAmountForeign");
				num5 += dataTable2.Rows[0].Field<decimal>("FreightAmountForeign");
			}
			if (b == 3)
			{
				decimal num6 = default(decimal);
				decimal num7 = default(decimal);
				decimal num8 = default(decimal);
				decimal num9 = default(decimal);
				if (!string.IsNullOrWhiteSpace(invoiceLineRow.Field<string>("arlShipmentID")))
				{
					SqlCommand sqlCommand3 = database.NewSqlCommand("Select smlQuantityShipped, smlJobQuantityShipped, omlOrderQuantity From ShipmentLines Inner Join SalesOrderLines On smlSalesOrderID = omlSalesOrderID And smlSalesOrderLineID = omlSalesOrderLineID Where smlShipmentID = @ShipmentID And smlSalesOrderID = @SalesOrderID And smlSalesOrderLineID = @SalesOrderLineID And smlSalesOrderDeliveryID = @SalesOrderDeliveryID And omlDeposit = 1");
					sqlCommand3.Parameters.Add(new SqlParameter("@ShipmentID", SqlDbType.NVarChar)).Value = invoiceLineRow.Field<string>("arlShipmentID");
					sqlCommand3.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = value;
					sqlCommand3.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.SmallInt)).Value = num;
					sqlCommand3.Parameters.Add(new SqlParameter("@SalesOrderDeliveryID", SqlDbType.SmallInt)).Value = invoiceLineRow.Field<short>("arlSalesOrderDeliveryID");
					DataTable dataTable3 = database.GetDataTable(sqlCommand3);
					if (dataTable3.Rows.Count != 0)
					{
						foreach (DataRow row4 in dataTable3.Rows)
						{
							decimal num10 = 1m;
							if (row4.Field<decimal>("omlOrderQuantity") != 0m)
							{
								num10 = (row4.Field<decimal>("smlQuantityShipped") + row4.Field<decimal>("smlJobQuantityShipped")) / row4.Field<decimal>("omlOrderQuantity");
							}
							num6 += M1Math.Round(num10 * row3.Field<decimal>("arlExtendedPriceForeign"), 2);
							num7 += M1Math.Round(num10 * row3.Field<decimal>("arlTaxAmountForeign"), 4);
							num8 += M1Math.Round(num10 * row3.Field<decimal>("arlSecondTaxAmountForeign"), 4);
							num9 += M1Math.Round(num10 * row3.Field<decimal>("arlFreightAmountForeign"), 2);
						}
					}
				}
				else
				{
					SqlCommand sqlCommand4 = database.NewSqlCommand("Select omlSalesOrderID, omlSalesOrderLineID, omlOrderQuantity, omlDepositAmountBase From SalesOrderLines Where omlSalesOrderID = @SalesOrderID And omlSalesOrderLineID = @SalesOrderLineID And omlDeposit = 1");
					sqlCommand4.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = value;
					sqlCommand4.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.SmallInt)).Value = num;
					DataTable dataTable4 = database.GetDataTable(sqlCommand4);
					decimal num10 = 1m;
					if (dataTable4.Rows.Count != 0)
					{
						DataRow row2 = dataTable4.Rows[0];
						if (invoiceLineRow.Field<decimal>("arlInvoiceQuantity") != 0m)
						{
							num10 = invoiceLineRow.Field<decimal>("arlInvoiceQuantity") / row2.Field<decimal>("omlOrderQuantity");
						}
					}
					num6 += M1Math.Round(num10 * row3.Field<decimal>("arlExtendedPriceForeign"), 2);
					num7 += M1Math.Round(num10 * row3.Field<decimal>("arlTaxAmountForeign"), 4);
					num8 += M1Math.Round(num10 * row3.Field<decimal>("arlSecondTaxAmountForeign"), 4);
					num9 += M1Math.Round(num10 * row3.Field<decimal>("arlFreightAmountForeign"), 2);
				}
				if (num6 != 0m && num6 < num2)
				{
					num2 = num6;
					num3 = num7;
					num4 = num8;
					num5 = num9;
				}
			}
			if (invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign") != 0m)
			{
				if (invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign") > 0m)
				{
					if (invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign") < num2 + num3 + num4 + num5)
					{
						if (database.Props("FN").Field<bool>("xafARCalculateTaxOnDeposit"))
						{
							num2 = invoiceLineRow.Field<decimal>("arlExtendedPriceForeign");
							num3 = invoiceLineRow.Field<decimal>("arlTaxAmountForeign");
							num4 = invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign");
							num5 = invoiceLineRow.Field<decimal>("arlFreightAmountForeign");
						}
						else
						{
							num2 = M1Math.Round(invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign"), 2);
							num3 = default(decimal);
							num4 = default(decimal);
							num5 = default(decimal);
						}
					}
				}
				else if (invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign") < 0m && invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign") > num2 + num3 + num4 + num5)
				{
					if (database.Props("FN").Field<bool>("xafARCalculateTaxOnDeposit"))
					{
						num2 = invoiceLineRow.Field<decimal>("arlExtendedPriceForeign");
						num3 = invoiceLineRow.Field<decimal>("arlTaxAmountForeign");
						num4 = invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign");
						num5 = invoiceLineRow.Field<decimal>("arlFreightAmountForeign");
					}
					else
					{
						num2 = M1Math.Round(invoiceLineRow.Field<decimal>("arlExtendedPriceForeign") + invoiceLineRow.Field<decimal>("arlTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlSecondTaxAmountForeign") + invoiceLineRow.Field<decimal>("arlFreightAmountForeign"), 2);
						num3 = default(decimal);
						num4 = default(decimal);
						num5 = default(decimal);
					}
				}
			}
			if (num2 != 0m)
			{
				DataRow dataRow2 = (DataRow)bsInvoiceLines.AddNew();
				dataRow2["arlSalesOrderID"] = value;
				dataRow2["arlSalesOrderLineID"] = num;
				dataRow2["arlSalesOrderDeliveryID"] = 0;
				dataRow2["arlShipmentID"] = string.Empty;
				dataRow2["arlDepositInvoiceID"] = row3["arlARInvoiceID"];
				dataRow2["arlDepositInvoiceLineID"] = row3["arlARInvoiceLineID"];
				dataRow2["arlDepositLine"] = true;
				dataRow2["arlOrderQuantity"] = -1;
				dataRow2["arlInvoiceQuantity"] = -1;
				dataRow2["arlPartID"] = "DEPOSIT";
				dataRow2["arlOrgPartID"] = string.Empty;
				dataRow2["arlOrgPartShortDescription"] = string.Empty;
				dataRow2["arlPartRevisionID"] = string.Empty;
				dataRow2["arlPartGroupID"] = row3["arlPartGroupID"];
				dataRow2["arlUnitOfMeasure"] = string.Empty;
				dataRow2["arlPartShortDescription"] = row3["arlPartShortDescription"];
				dataRow2["arlPartLongDescriptionRTF"] = DBNull.Value;
				dataRow2["arlPartLongDescriptionText"] = DBNull.Value;
				dataRow2["arlPayCommission"] = false;
				dataRow2["arlUnitPriceForeign"] = num2;
				dataRow2["arlFullUnitPriceForeign"] = num2;
				dataRow2["arlTaxCodeID"] = row3["arlTaxCodeID"];
				dataRow2["arlSecondTaxCodeID"] = row3["arlSecondTaxCodeID"];
				dataRow2["arlNonTaxReasonID"] = string.Empty;
				dataRow2["arlTaxAmountForeign"] = -num3;
				dataRow2["arlSecondTaxAmountForeign"] = -num4;
				dataRow2["arlFreightAmountForeign"] = -num5;
				dataRow2["arlDepositAmountForeign"] = -1m * (dataRow2.Field<decimal>("arlExtendedPriceForeign") + dataRow2.Field<decimal>("arlTaxAmountForeign") + dataRow2.Field<decimal>("arlSecondTaxAmountForeign") + dataRow2.Field<decimal>("arlFreightAmountForeign"));
			}
		}
	}

	public void SetARInvoiceAccounts(M1Database database, DataRow invoiceRow)
	{
		bool flag = false;
		if (!string.IsNullOrWhiteSpace(invoiceRow.Field<string>("arpPlantID")))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select Case When IsNull(xavARARGLAccountID,'') = '' Then xauARARGLAccountID Else xavARARGLAccountID End As xauARARGLAccountID,Case When IsNull(xavARSalesGLAccountID,'') = '' Then xauARSalesGLAccountID Else xavARSalesGLAccountID End As xauARSalesGLAccountID,Case When IsNull(xavARFreightGLAccountID,'') = '' Then xauARFreightGLAccountID Else xavARFreightGLAccountID End As xauARFreightGLAccountID,Case When IsNull(xavARDiscountGLAccountID,'') = '' Then xauARDiscountGLAccountID Else xavARDiscountGLAccountID End As xauARDiscountGLAccountID,Case When IsNull(xavARDepositGLAccountID,'') = '' Then xauARDepositGLAccountID Else xavARDepositGLAccountID End As xauARDepositGLAccountID,Case When IsNull(xavUseProperties,0) = 0 Then xauUseProperties Else xavUseProperties End As xauUseProperties From Plants Left Outer Join PlantDepartments On xauPlantID = xavPlantID And xavPlantDepartmentID = @PlantDepartmentID And xavUseProperties = 1 Where xauPlantID = @PlantID");
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = invoiceRow.Field<string>("arpPlantID");
			sqlCommand.Parameters.Add(new SqlParameter("@PlantDepartmentID", SqlDbType.NVarChar)).Value = invoiceRow.Field<string>("arpPlantDepartmentID");
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow dataRow = dataTable.Rows[0];
				if (dataRow.Field<bool>("xauUseProperties"))
				{
					flag = true;
					invoiceRow["arpARGLAccountID"] = dataRow["xauARARGLAccountID"];
					invoiceRow["arpSalesGLAccountID"] = dataRow["xauARSalesGLAccountID"];
					invoiceRow["arpFreightGLAccountID"] = dataRow["xauARFreightGLAccountID"];
					invoiceRow["arpDiscountGLAccountID"] = dataRow["xauARDiscountGLAccountID"];
					invoiceRow["arpDepositGLAccountID"] = dataRow["xauARDepositGLAccountID"];
				}
			}
		}
		if (!flag)
		{
			invoiceRow["arpARGLAccountID"] = database.Props("AR")["xafARARGLAccountID"];
			invoiceRow["arpSalesGLAccountID"] = database.Props("OM")["xapOMSalesGLAccountID"];
			invoiceRow["arpFreightGLAccountID"] = database.Props("AR")["xafARFreightGLAccountID"];
			invoiceRow["arpDiscountGLAccountID"] = database.Props("AR")["xafARDiscountGLAccountID"];
			invoiceRow["arpDepositGLAccountID"] = database.Props("AR")["xafARDepositGLAccountID"];
		}
		if (!string.IsNullOrWhiteSpace(invoiceRow.Field<string>("arpCurrencyRateID")))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select mcpARGLAccountID From CurrencyRates Where mcpCurrencyRateID = @CurrencyRateID");
			sqlCommand.Parameters.Add(new SqlParameter("@CurrencyRateID", SqlDbType.NVarChar)).Value = invoiceRow.Field<string>("arpCurrencyRateID");
			string value = Convert.ToString(database.ExecuteScalar(sqlCommand));
			if (!string.IsNullOrWhiteSpace(value))
			{
				invoiceRow["arpARGLAccountID"] = value;
			}
		}
	}

	public void SetARRecurringInvoiceAccounts(M1Database database, DataRow invoiceRow)
	{
		bool flag = false;
		if (!string.IsNullOrWhiteSpace(invoiceRow.Field<string>("arrPlantID")))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select Case When IsNull(xavARARGLAccountID,'') = '' Then xauARARGLAccountID Else xavARARGLAccountID End As xauARARGLAccountID,Case When IsNull(xavARSalesGLAccountID,'') = '' Then xauARSalesGLAccountID Else xavARSalesGLAccountID End As xauARSalesGLAccountID,Case When IsNull(xavARFreightGLAccountID,'') = '' Then xauARFreightGLAccountID Else xavARFreightGLAccountID End As xauARFreightGLAccountID,Case When IsNull(xavARDiscountGLAccountID,'') = '' Then xauARDiscountGLAccountID Else xavARDiscountGLAccountID End As xauARDiscountGLAccountID,Case When IsNull(xavARDepositGLAccountID,'') = '' Then xauARDepositGLAccountID Else xavARDepositGLAccountID End As xauARDepositGLAccountID,Case When IsNull(xavUseProperties,0) = 0 Then xauUseProperties Else xavUseProperties End As xauUseProperties From Plants Left Outer Join PlantDepartments On xauPlantID = xavPlantID And xavPlantDepartmentID = @PlantDepartmentID And xavUseProperties = 1 Where xauPlantID = @PlantID");
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = invoiceRow.Field<string>("arrPlantID");
			sqlCommand.Parameters.Add(new SqlParameter("@PlantDepartmentID", SqlDbType.NVarChar)).Value = invoiceRow.Field<string>("arrPlantDepartmentID");
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow dataRow = dataTable.Rows[0];
				if (dataRow.Field<bool>("xauUseProperties"))
				{
					flag = true;
					invoiceRow["arrARGLAccountID"] = dataRow["xauARARGLAccountID"];
					invoiceRow["arrSalesGLAccountID"] = dataRow["xauARSalesGLAccountID"];
					invoiceRow["arrFreightGLAccountID"] = dataRow["xauARFreightGLAccountID"];
					invoiceRow["arrDiscountGLAccountID"] = dataRow["xauARDiscountGLAccountID"];
				}
			}
		}
		if (!flag)
		{
			invoiceRow["arrARGLAccountID"] = database.Props("AR")["xafARARGLAccountID"];
			invoiceRow["arrSalesGLAccountID"] = database.Props("OM")["xapOMSalesGLAccountID"];
			invoiceRow["arrFreightGLAccountID"] = database.Props("AR")["xafARFreightGLAccountID"];
			invoiceRow["arrDiscountGLAccountID"] = database.Props("AR")["xafARDiscountGLAccountID"];
		}
		if (!string.IsNullOrWhiteSpace(invoiceRow.Field<string>("arrCurrencyRateID")))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select mcpARGLAccountID From CurrencyRates Where mcpCurrencyRateID = @CurrencyRateID");
			sqlCommand.Parameters.Add(new SqlParameter("@CurrencyRateID", SqlDbType.NVarChar)).Value = invoiceRow.Field<string>("arrCurrencyRateID");
			string value = Convert.ToString(database.ExecuteScalar(sqlCommand));
			if (!string.IsNullOrWhiteSpace(value))
			{
				invoiceRow["arrARGLAccountID"] = value;
			}
		}
	}

	public void AddTimeAndMaterial(M1Database database, M1BindingSource bsInvoiceLines, string orderID, short orderLine, string shipmentID, short shipmentLine, string callID)
	{
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		SqlCommand sqlCommand;
		if (string.IsNullOrWhiteSpace(callID))
		{
			if (string.IsNullOrWhiteSpace(orderID) || orderLine == 0)
			{
				return;
			}
			sqlCommand = database.NewSqlCommand("select distinct jmpJobID from Jobs Inner Join SalesOrderJobLinks On omjJobID = jmpJobID Inner Join SalesOrders On omjSalesOrderID = ompSalesOrderID Where omjSalesOrderID = @OrderID and omjSalesOrderLineID = @OrderLineID and jmpTimeAndMaterial = 1 Order by jmpJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
			sqlCommand.Parameters.Add(new SqlParameter("@OrderLineID", SqlDbType.SmallInt)).Value = orderLine;
		}
		else
		{
			sqlCommand = database.NewSqlCommand("Select jmpJobID From Jobs Where jmpCallID = @CallID and jmpTimeAndMaterial = 1");
			sqlCommand.Parameters.Add(new SqlParameter("@CallID", SqlDbType.NVarChar)).Value = callID;
		}
		DataTable dataTable = database.GetDataTable(sqlCommand);
		Part part = new Part();
		bool flag = database.Props("OM").Field<bool>("xapOMUseQuotingMarkupTM");
		bool flag2 = database.Props("AP").Field<bool>("xafAPUpdateJobCosts");
		foreach (DataRow row in dataTable.Rows)
		{
			sqlCommand = database.NewSqlCommand("select IsNull(sum(lmlLaborHours),0) as lmlLaborHours,Round(IsNull(sum(lmlLaborHours * xawQuotingRate),0),2) as TotalCost from TimecardLines Inner Join WorkCenters On lmlWorkCenterID = xawWorkCenterID Inner Join Processes On lmlProcessID = xacProcessID where lmlJobID = @JobID And xacExcludeFromTMJobs = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = row["jmpJobID"];
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			if (dataTable2.Rows.Count != 0)
			{
				decimal num = dataTable2.Rows[0].Field<decimal>("lmlLaborHours");
				DataRow dataRow2 = (DataRow)bsInvoiceLines.AddNew();
				dataRow2["arlLineType"] = 2;
				dataRow2["arlSalesOrderID"] = orderID;
				dataRow2["arlSalesOrderLineID"] = orderLine;
				dataRow2["arlShipmentID"] = shipmentID;
				dataRow2["arlShipmentLineID"] = shipmentLine;
				string value = database.Props("FN").Field<string>("xafARLaborPartID");
				string value2 = database.Props("FN").Field<string>("xafARLaborPartRevisionID");
				if (!string.IsNullOrWhiteSpace(value))
				{
					dataRow2["arlPartID"] = value;
					dataRow2["arlPartRevisionID"] = value2;
				}
				else
				{
					dataRow2["arlPartID"] = m1DataDictionary.Language.GetLocalString("LABOR").ToUpper();
				}
				dataRow2["arlUnitOfMeasure"] = "HR";
				dataRow2["arlPartShortDescription"] = m1DataDictionary.Language.GetLocalString("Labor");
				dataRow2["arlJobID"] = row["jmpJobID"];
				dataRow2["arlPartGroupID"] = database.Props("FN").Field<string>("xafARDefaultLaborPartGroupID");
				dataRow2["arlOrderQuantity"] = num;
				dataRow2["arlInvoiceQuantity"] = num;
				dataRow2["arlCallID"] = callID;
				dataRow2["arlPayCommission"] = true;
				if (num != 0m)
				{
					if (flag)
					{
						PartGroupMarkup partGroupMarkups = part.GetPartGroupMarkups(database, dataRow2.Field<string>("arlPartGroupID"));
						dataRow2["arlFullUnitPriceBase"] = M1Math.Round(M1Math.CalculateMarkup(partGroupMarkups.MarkupType, dataTable2.Rows[0].Field<decimal>("TotalCost"), partGroupMarkups.LaborMarkup, 2) / num, 5);
					}
					else
					{
						dataRow2["arlFullUnitPriceBase"] = M1Math.Round(dataTable2.Rows[0].Field<decimal>("TotalCost") / num, 5);
					}
				}
				else
				{
					dataRow2["arlFullUnitPriceBase"] = dataTable2.Rows[0]["TotalCost"];
					dataRow2["arlFullExtendedPriceBase"] = dataTable2.Rows[0]["TotalCost"];
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder = new StringBuilder("Select rmlJobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, rmlPartLongDescriptionRTF, rmlPartLongDescriptionText,rmlJobType, Sum(rmlInventoryCost) AS rmlInventoryCost, Sum(rmlJobMatQuantityReceived) As rmlJobMatQuantityReceived, impPartGroupID From (");
			if (flag2)
			{
				stringBuilder.Append("SELECT rmlJobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, Min(rmlPartLongDescriptionRTF) As rmlPartLongDescriptionRTF,Min(rmlPartLongDescriptionText) As rmlPartLongDescriptionText, rmlJobType, Sum((rmlInventoryUnitCost * (rmlJobMatQuantityReceived + rmlJobOprQuantityReceived + IsNull(qalJobMatQuantityAccepted, 0) + IsNull(qalJobOprQuantityAccepted, 0))) + rmlSetupCharge) As rmlInventoryCost, Sum(rmlJobMatQuantityReceived + rmlJobOprQuantityReceived + IsNull(qalJobMatQuantityAccepted, 0) + IsNull(qalJobOprQuantityAccepted, 0)) As rmlJobMatQuantityReceived, IsNull(impPartGroupID, '') As impPartGroupID FROM ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID Left Outer Join Parts On rmlPartID = impPartID WHERE RTrim(rmlReceiptID)+RTrim(convert(varchar,rmlReceiptLineID)) NOT IN(select RTrim(aplReceiptID)+RTrim(convert(varchar,aplReceiptLineID)) FROM APInvoiceLines where aplReceiptID = rmlReceiptID AND aplReceiptLineID = rmlReceiptLineID) AND rmlJobID = @JobID Group By rmljobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, impPartGroupID, rmlJobType Union All SELECT aplJobID, aplJobAssemblyID, aplJobMaterialID, aplPartID, aplPartRevisionId, aplReceivedUnitOfMeasure, aplPartDescription, Min(aplPartLongDescriptionRTF) As aplPartLongDescriptionRTF, Min(aplPartLongDescriptionText) AS aplPartLongDescriptionText, aplJobType, SUM(aplExtendedCostBase) As aplExtendedCostBase, Sum(aplReceivedQuantity) As aplReceivedQuantity, IsNull(impPartGroupID, '') AS impPartGroupID FROM  APInvoiceLines left outer join apinvoices on aplAPInvoiceID = appAPInvoiceID Left Outer Join Parts On aplPartID = impPartID where aplJobID = @JobID Group By apljobID, aplJobAssemblyID, aplJobMaterialID, aplPartID, aplPartRevisionID, aplReceivedUnitOfMeasure, aplPartDescription, impPartGroupID, aplJobType Union All SELECT rmlJobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, Min(rmlPartLongDescriptionRTF) As rmlPartLongDescriptionRTF, Min(rmlPartLongDescriptionText) As rmlPartLongDescriptionText, rmlJobType, Sum(((rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived + rmlJobOprQuantityReceived + IsNull(qalJobMatQuantityAccepted, 0) + IsNull(qalJobOprQuantityAccepted, 0))) + rmlSetupCharge) As rmlInventoryCost, 0 As rmlJobMatQuantityReceived, IsNull(impPartGroupID, '') As impPartGroupID FROM ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID Left Outer Join Parts On rmlPartID = impPartID WHERE rmlPurchaseOrderID <> '' AND rmlPurchaseOrderID NOT IN ( Select pmlPurchaseOrderID From APInvoiceExpenseAccounts Inner Join LandedCostChargeDetails on rmiUniqueID = apxSourceTableUniqueID Inner Join LandedCostCharges on rmiLandedCostID = rmhLandedCostID and rmiLandedCostChargeID = rmhLandedCostChargeID Inner Join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID Where pmlJobID = @JobID and pmlJobType in (1,3) And rmhAPInvoiceID <> '') AND rmlJobID = @JobID Group By rmljobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, impPartGroupID, rmlJobType Union All SELECT aplJobID, aplJobAssemblyID, aplJobMaterialID, aplPartID, aplPartRevisionId, aplReceivedUnitOfMeasure, aplPartDescription, Min(aplPartLongDescriptionRTF) As aplPartLongDescriptionRTF, Min(aplPartLongDescriptionText) AS aplPartLongDescriptionText, aplJobType, SUM(apxAmount) As aplExtendedCostBase, 0 As aplReceivedQuantity, IsNull(impPartGroupID, '') AS impPartGroupID FROM APInvoiceExpenseAccounts Inner Join APInvoiceLines On apxAPInvoiceID = aplAPInvoiceID And apxAPInvoiceLineID = aplAPInvoiceLineID left outer join apinvoices on aplAPInvoiceID = appAPInvoiceID Inner Join LandedCostChargeDetails on rmiUniqueID = apxSourceTableUniqueID Inner Join LandedCostCharges on rmiLandedCostID = rmhLandedCostID and rmiLandedCostChargeID = rmhLandedCostChargeID Inner Join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID Left Outer Join Parts On aplPartID = impPartID where pmlJobID = @JobID Group By apljobID, aplJobAssemblyID, aplJobMaterialID, aplPartID, aplPartRevisionID, aplReceivedUnitOfMeasure, aplPartDescription, impPartGroupID, aplJobType ");
			}
			else
			{
				stringBuilder.Append("SELECT rmlJobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, Min(rmlPartLongDescriptionRTF) As rmlPartLongDescriptionRTF,Min(rmlPartLongDescriptionText) As rmlPartLongDescriptionText, rmlJobType, Sum(((rmlInventoryUnitCost+rmlDutyUnitCost+rmlFreightUnitCost+rmlMiscUnitCost) * (rmlJobMatQuantityReceived + rmlJobOprQuantityReceived + IsNull(qalJobMatQuantityAccepted, 0) + IsNull(qalJobOprQuantityAccepted, 0))) + rmlSetupCharge) As rmlInventoryCost, Sum(rmlJobMatQuantityReceived + rmlJobOprQuantityReceived + IsNull(qalJobMatQuantityAccepted, 0) + IsNull(qalJobOprQuantityAccepted, 0)) As rmlJobMatQuantityReceived, IsNull(impPartGroupID, '') As impPartGroupID FROM ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID LEFT OUTER JOIN InspectionLines On qalSourceTableUniqueID = rmlUniqueID Left Outer Join Parts On rmlPartID = impPartID WHERE rmlPurchaseOrderID <> '' AND rmlJobID = @JobID Group By rmljobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, impPartGroupID, rmlJobType ");
			}
			stringBuilder.Append("Union All SELECT rmmJobID, rmmJobAssemblyID, rmmJobMaterialID, rmmPartID, rmmPartRevisionID, rmmInventoryUnitOfMeasure, imrShortDescription, Min(jmmPartLongDescriptionRTF) As rmmLongDescriptionRTF, Min(jmmPartLongDescriptionText) AS rmmLongDescriptionText, rmmJobType, SUM(((rmmUnitOverheadCost + rmmUnitLaborCost + rmmUnitMaterialCost + rmmUnitSubcontractCost) * (rmmScrapQuantity + rmmJobAsmQuantityReceived + rmmJobMatQuantityReceived)) + rmmSetupCharge) As rmmUnitMaterialCost, Sum(rmmScrapQuantity + rmmJobAsmQuantityReceived + rmmJobMatQuantityReceived) As rmmJobMatQuantityReceived, IsNull(impPartGroupID, '') AS impPartGroupID FROM MfgReceipts Left Outer Join Parts on rmmPartID = impPartID Left Outer Join PartRevisions On rmmPartID = imrPartID And rmmPartRevisionID = imrPartRevisionID Left Outer Join JobMaterials On rmmJobID = jmmJobID And rmmJobAssemblyID = jmmJobAssemblyID And rmmJobMaterialID = jmmJobMaterialID WHERE rmmJobID = @JobID AND rmmReceiptType = 1 And rmmKitPart = 0 Group By rmmjobID, rmmJobAssemblyID, rmmJobMaterialID, rmmPartID, rmmPartRevisionID, rmmInventoryUnitOfMeasure, imrShortDescription, impPartGroupID, rmmJobType Union All SELECT rmnJobID, rmnJobAssemblyID, rmnJobMaterialID, rmnPartID, rmnPartRevisionID, rmnUnitOfMeasure, imrShortDescription, Min(jmmPartLongDescriptionRTF) As rmmLongDescriptionRTF, Min(jmmPartLongDescriptionText) AS rmmLongDescriptionText, rmmJobType, SUM(rmnUnitCost * rmnJobMatReceiptQuantity) As rmnUnitCost, Sum(rmnJobMatReceiptQuantity) As rmnJobMatReceiptQuantity, IsNull(impPartGroupID, '') AS impPartGroupID FROM MfgReceiptComponents Inner Join MfgReceipts On rmnMfgReceiptID = rmmMfgReceiptID Left Outer Join Parts on rmnPartID = impPartID Left Outer Join PartRevisions On rmnPartID = imrPartID And rmnPartRevisionID = imrPartRevisionID Left Outer Join JobMaterials On rmnJobID = jmmJobID And rmnJobAssemblyID = jmmJobAssemblyID And rmnJobMaterialID = jmmJobMaterialID WHERE rmnJobID = @JobID AND rmmReceiptType = 1 Group By rmnjobID, rmnJobAssemblyID, rmnJobMaterialID, rmnPartID, rmnPartRevisionID, rmnUnitOfMeasure, imrShortDescription, impPartGroupID, rmmJobType ");
			stringBuilder.Append("Union All SELECT imtJobID, imtJobAssemblyID, imtJobMaterialID, imtPartID, imtPartRevisionID, (Case When imtJobMaterialComponentID <> 0 Then inkUnitOfMeasure Else jmmUnitOfMeasure End) As imtInventoryUnitOfMeasure, (Case When imtJobMaterialComponentID <> 0 Then imrShortDescription Else jmmPartShortDescription End) As jmmPartShortDescription, Min(IsNull(injLongDescriptionRTF, '')) As injLongDescriptionRTF, Min(IsNull(injLongDescriptionText, '')) As injLongDescriptionText, imtJobType, SUM((intUnitOverheadCost + intUnitLaborCost + intUnitMaterialCost + intUnitSubcontractCost + intUnitDutyCost + intUnitFreightCost + intUnitMiscCost) * ((Case When imtSource = 3 Then - 1 Else 1 End) * (intQuantity + imtScrapQuantity))) As intUnitMaterialCost, Sum(((Case When imtSource = 3 Then - 1 Else 1 End)*(intQuantity + imtScrapQuantity))) As intQuantity, IsNull(impPartGroupID, '') AS impPartGroupID FROM PartTransactions Inner Join PartTransactionCosts on imtPartTransactionID = intPartTransactionID Left Outer Join Warehouses On imtPartWarehouseLocationID = imwWarehouseID Left Outer Join MaterialIssueLines on imtTableUniqueID = injUniqueID Left Outer Join MaterialIssueComponents On imtTableUniqueID = inkUniqueID Left Outer Join Parts On imtPartID = impPartID Left Outer Join JobMaterials On imtJobID = jmmJobID And imtJobAssemblyID = jmmJobAssemblyID And imtJobMaterialID = jmmJobMaterialID Left Outer Join PartRevisions On imtPartID = imrPartID And imtPartRevisionID = imrPartRevisionID WHERE intCostType = (Case When 4 = 1 Then 1 When 4 = 2 Then 2 When 4 = 3 Then 3 Else 4 End) AND imtJobID = @JobID AND(imtSource = 3 or imtSource = 2) AND imtReceiptID = '' AND Upper(imtTableName)Not In('RECEIPTLINES', 'RECEIPTCOMPONENTS', 'MFGRECEIPTS', 'MFGRECEIPTCOMPONENTS') AND imtNonInventoryTransaction = 0 AND imtJobType in (1,3) And(imtNonNettable = 0 Or(imtNonNettable <> 0 And IsNull(imwDoNotIncludeInJobCosts, 0) = 0)) Group By imtjobID, imtJobAssemblyID, imtJobMaterialID, imtJobMaterialComponentID, imtPartID, imtPartRevisionID, inkUnitOfMeasure, jmmUnitOfMeasure, jmmPartShortDescription, imrShortDescription, impPartGroupID, imtJobType, imtSource ");
			stringBuilder.Append(") As TimeAndMaterials Group By rmlJobID, rmlJobAssemblyID, rmlJobMaterialID, rmlPartID, rmlPartRevisionID, rmlInventoryUnitOfMeasure, rmlDescription, rmlPartLongDescriptionRTF, rmlPartLongDescriptionText,rmlJobType, impPartGroupID");
			sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = row["jmpJobID"];
			DataTable dataTable3 = database.GetDataTable(sqlCommand);
			if (dataTable3.Rows.Count == 0)
			{
				continue;
			}
			foreach (DataRow row2 in dataTable3.Rows)
			{
				PartGroupMarkup partGroupMarkups = part.GetPartGroupMarkups(database, row2.Field<string>("impPartGroupID"));
				DataRow dataRow2 = (DataRow)bsInvoiceLines.AddNew();
				dataRow2["arlLineType"] = 3;
				dataRow2["arlSalesOrderID"] = orderID;
				dataRow2["arlSalesOrderLineID"] = orderLine;
				dataRow2["arlShipmentID"] = shipmentID;
				dataRow2["arlShipmentLineID"] = shipmentLine;
				dataRow2["arlPartLongDescriptionRTF"] = row2["rmlPartLongDescriptionRTF"];
				dataRow2["arlPartLongDescriptionText"] = row2["rmlPartLongDescriptionText"];
				dataRow2["arlPartID"] = row2["rmlPartID"];
				dataRow2["arlPartRevisionID"] = row2["rmlPartRevisionID"];
				dataRow2["arlUnitOfMeasure"] = row2["rmlInventoryUnitOfMeasure"];
				dataRow2["arlPartShortDescription"] = row2["rmlDescription"];
				dataRow2["arlOrderQuantity"] = row2["rmlJobMatQuantityReceived"];
				dataRow2["arlInvoiceQuantity"] = row2["rmlJobMatQuantityReceived"];
				dataRow2["arlCallID"] = callID;
				dataRow2["arlPayCommission"] = true;
				dataRow2["arlJobID"] = row2["rmlJobID"];
				dataRow2["arlJobAssemblyID"] = row2["rmlJobAssemblyID"];
				dataRow2["arlJobMaterialID"] = row2["rmlJobMaterialID"];
				if (dataRow2.Field<decimal>("arlFullUnitPriceBase") == 0m)
				{
					decimal num2 = M1Math.CalculateMarkup(partGroupMarkups.MarkupType, row2.Field<decimal>("rmlInventoryCost"), (row2.Field<byte>("rmlJobType") == 2) ? partGroupMarkups.SubcontractMarkup : partGroupMarkups.MaterialMarkup, 2);
					if (row2.Field<decimal>("rmlJobMatQuantityReceived") != 0m)
					{
						dataRow2["arlFullUnitPriceBase"] = M1Math.Round(num2 / row2.Field<decimal>("rmlJobMatQuantityReceived"), 5);
						continue;
					}
					dataRow2["arlFullUnitPriceBase"] = num2;
					dataRow2["arlFullExtendedPriceBase"] = num2;
				}
			}
		}
	}

	public ARPaymentInfo GetARPaymentInfo(M1Database database, int sessionID)
	{
		ARPaymentInfo aRPaymentInfo = new ARPaymentInfo();
		if (sessionID != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select IsNull(sum((Case when arnAPInvoiceID = '' Then arnPaymentAmount Else -1 * arnPaymentAmount End) + case when artReceiptType = 4 then (Case when arnAPInvoiceID = '' Then (arnTaxAmount+arnSecondTaxAmount) Else (-1 * (arnTaxAmount+arnSecondTaxAmount)) End) else 0 end),0) as arnPaymentAmount, IsNull(sum((Case when arnAPInvoiceID = '' Then arnPaymentAmountForeign Else -1 * arnPaymentAmountForeign End) + case when artReceiptType = 4 then (Case when arnAPInvoiceID = '' Then (arnTaxAmountForeign+arnSecondTaxAmountForeign) Else (-1 * (arnTaxAmountForeign+arnSecondTaxAmountForeign)) End) else 0 end),0) as arnPaymentAmountForeign from ARPaymentLines inner join ARPaymentHeaders on artARPaymentSessionID=arnARPaymentSessionID and artARPaymentHeaderID=arnARPaymentHeaderID where artARPaymentSessionID = @SessionID and (artReceiptType = 1 or artReceiptType = 4 or artReceiptType = 6)");
			sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				aRPaymentInfo.AmountClearedBase = dataTable.Rows[0].Field<decimal>("arnPaymentAmount");
				aRPaymentInfo.AmountClearedForeign = dataTable.Rows[0].Field<decimal>("arnPaymentAmountForeign");
			}
			sqlCommand = database.NewSqlCommand("Select IsNull(Sum(artExchangeAmount),0) As artExchangeAmount From ARPaymentHeaders Where artARPaymentSessionID = @SessionID");
			sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
			dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				aRPaymentInfo.ExchangeAmount = dataTable.Rows[0].Field<decimal>("artExchangeAmount");
			}
		}
		return aRPaymentInfo;
	}

	public int GetPaymentHeaderCount(M1Database database, int sessionID)
	{
		if (sessionID == 0)
		{
			return 0;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select count(*) as rec_Count from ARPaymentHeaders where artARPaymentSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand));
	}

	public string PostInvoice(M1BindingSource bindingSource, bool fromPOS = false, bool forceNoMsg = false)
	{
		string text = string.Empty;
		clsARFunctionsClass clsARFunctionsClass2 = new clsARFunctionsClass();
		clsARFunctionsClass2.SetReferences(bindingSource.GetService(typeof(ScriptApp)), bindingSource.GetService(typeof(IForms)));
		if (clsARFunctionsClass2 != null && bindingSource.CurrentAsDataRow != null)
		{
			string cInvoice = bindingSource.CurrentAsDataRow.Field<string>("arpARInvoiceID");
			text = clsARFunctionsClass2.PostInvoice(cInvoice, fromPOS, forceNoMsg);
			if (text != null && bindingSource.Database.Props("GL").Field<bool>("xafGLCreateStockJournals"))
			{
				M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
				foreach (DataRow row in childBindingSource.GetDataTable().Rows)
				{
					DateTime dateTime = bindingSource.CurrentAsDataRow.Field<DateTime>("arpInvoiceDate");
					byte b = bindingSource.CurrentAsDataRow.Field<byte>("arpInvoiceType");
					short year = new Financial().GetYearAndPeriod(bindingSource.Database, dateTime, "GL", IgnoreClosed: true, bindingSource.Transaction).Year;
					byte period = new Financial().GetYearAndPeriod(bindingSource.Database, dateTime, "GL", IgnoreClosed: true, bindingSource.Transaction).Period;
					if (!string.IsNullOrWhiteSpace(row.Field<string>("arlShipmentID")) && row.Field<byte>("arlLineType") <= 1)
					{
						bool flag = true;
						SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("SELECT smpReversalEntry FROM Shipments where smpShipmentID = @ShipmentId");
						sqlCommand.Parameters.Add(new SqlParameter("@ShipmentId", SqlDbType.NVarChar)).Value = row.Field<string>("arlShipmentID");
						bool flag2 = (bool)bindingSource.Database.ExecuteScalar(sqlCommand, bindingSource.Transaction);
						flag = b != 2 || flag2;
						CostOfGoodSoldDefinition costOfGoodSoldDefinition = new CostOfGoodSoldDefinition(childBindingSource, "arlInvoiceQuantity", "arlPartRevisionID", dateTime, 9, 2, flag, row.Field<decimal>("arlInvoiceQuantity"), "ManualJournalCreation,", string.Empty, string.Empty, "arlJobMaterialID");
						costOfGoodSoldDefinition.UseFiscalYearAndPeriodOnLinesFromJournal = false;
						costOfGoodSoldDefinition.UseFiscalYearAndPeriodFromJournal = false;
						costOfGoodSoldDefinition.ProvidedFiscalYear = year;
						costOfGoodSoldDefinition.ProvidedFiscalPeriod = period;
						costOfGoodSoldDefinition.AddJournal(bindingSource.Database, row, DataRowVersion.Current, bindingSource.Transaction);
					}
					else if (!string.IsNullOrWhiteSpace(row.Field<string>("arlRMAReceiptID")))
					{
						CostOfGoodSoldDefinition costOfGoodSoldDefinition2 = new CostOfGoodSoldDefinition(childBindingSource, "arlInvoiceQuantity", "arlPartRevisionID", dateTime, 36, 2, b != 2, row.Field<decimal>("arlInvoiceQuantity"), "ManualJournalCreation,", string.Empty, string.Empty, "arlJobMaterialID");
						costOfGoodSoldDefinition2.UseFiscalYearAndPeriodOnLinesFromJournal = false;
						costOfGoodSoldDefinition2.UseFiscalYearAndPeriodFromJournal = false;
						costOfGoodSoldDefinition2.ProvidedFiscalYear = year;
						costOfGoodSoldDefinition2.ProvidedFiscalPeriod = period;
						costOfGoodSoldDefinition2.AddJournal(bindingSource.Database, row, DataRowVersion.Current, bindingSource.Transaction);
					}
				}
			}
		}
		return text;
	}

	public void RefreshTaxSubtotal(M1Database database, M1BindingSource bsInvoice, SqlTransaction transaction)
	{
		DataRow currentAsDataRow = bsInvoice.CurrentAsDataRow;
		bool flag = false;
		if (string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("arpARInvoiceID")))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(sum(arlTaxAmountBase),0) As arlTaxAmountBase,IsNull(sum(arlTaxAmountBasePos),0) as arlTaxAmountBasePos,IsNull(sum(arlTaxAmountBaseNeg),0) as arlTaxAmountBaseNeg,IsNull(Sum(arlTaxAmountForeignPos),0) As arlTaxAmountForeignPos,IsNull(Sum(arlTaxAmountForeignNeg),0) As arlTaxAmountForeignNeg From(select arlTaxCodeID, Round(sum(arlTaxAmountBase), 2) As arlTaxAmountBase, Round(sum(arlTaxAmountBasePos), 2) as arlTaxAmountBasePos, Round(sum(arlTaxAmountBaseNeg), 2) as arlTaxAmountBaseNeg, Round(Sum(arlTaxAmountForeignPos), 2) As arlTaxAmountForeignPos, Round(Sum(arlTaxAmountForeignNeg), 2) As arlTaxAmountForeignNeg From(select arlTaxCodeID, sum(arlTaxAmountBase) as arlTaxAmountBase, sum(Case When arlTaxAmountBase > 0 Then arlTaxAmountBase Else 0 End) as arlTaxAmountBasePos, sum(Case When arlTaxAmountBase < 0 Then arlTaxAmountBase Else 0 End) as arlTaxAmountBaseNeg, sum(Case When arlTaxAmountForeign > 0 Then arlTaxAmountForeign Else 0 End) As arlTaxAmountForeignPos, sum(Case When arlTaxAmountForeign < 0 Then arlTaxAmountForeign Else 0 End) As arlTaxAmountForeignNeg, (Case When IsNull(TaxCodePlants.xtpAccrualGLAccountID, '') <> '' Then TaxCodePlants.xtpAccrualGLAccountID Else TaxCodes.xaxAccrualGLAccountID End) As xaxAccrualGLAccountID From(Select arpPlantID, arlTaxCodeID, arlTaxAmountBase, arlTaxAmountForeign From ARInvoices Inner Join ARInvoiceLines On arpARInvoiceID = arlARInvoiceID where arlARInvoiceID = @InvoiceID And arlTaxAmountBase <> 0 And arlDepositLine = 0 Union All Select arpPlantID, arlSecondTaxCodeID As arlTaxCodeID, arlSecondTaxAmountBase As arlTaxAmountBase, arlSecondTaxAmountForeign As arlTaxAmountForeign From ARInvoices Inner Join ARInvoiceLines On arpARInvoiceID = arlARInvoiceID where arlARInvoiceID = @InvoiceID And arlSecondTaxAmountBase <> 0 And arlDepositLine = 0 ) As Test left outer join TaxCodes on arlTaxCodeID = xaxTaxCodeID Left Outer Join TaxCodePlants On arlTaxCodeID = xtpTaxCodeID And arpPlantID = xtpPlantID group by arpPlantID, arlTaxCodeID, xaxAccrualGLAccountID, xtpAccrualGLAccountID) as Test2 Group By arlTaxCodeID) As Test3");
		sqlCommand.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.NVarChar)).Value = currentAsDataRow.Field<string>("arpARInvoiceID");
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0 && bsInvoice.CurrentAsDataRow.Field<decimal>("arpTaxSubtotalForeign") != Convert.ToDecimal(dataTable.Rows[0]["arlTaxAmountForeignPos"]) + Convert.ToDecimal(dataTable.Rows[0]["arlTaxAmountForeignNeg"]))
		{
			bsInvoice.CurrentAsDataRow.SetField("arpTaxSubtotalForeign", Convert.ToDecimal(dataTable.Rows[0]["arlTaxAmountForeignPos"]) + Convert.ToDecimal(dataTable.Rows[0]["arlTaxAmountForeignNeg"]));
			bsInvoice.CurrentAsDataRow.SetField("arpTaxSubtotalBase", Convert.ToDecimal(dataTable.Rows[0]["arlTaxAmountBasePos"]) + Convert.ToDecimal(dataTable.Rows[0]["arlTaxAmountBaseNeg"]));
			flag = true;
		}
		if (!flag || bsInvoice.InSaveData)
		{
			return;
		}
		bool flag2 = false;
		if (bsInvoice.Errors != null)
		{
			foreach (ValidationInfo error in bsInvoice.Errors)
			{
				if (error.ErrorCount > 0)
				{
					flag2 = true;
					break;
				}
			}
		}
		if (!flag2)
		{
			bsInvoice.SaveData();
		}
	}

	public bool ARInvoicePostedCheck(M1Database database, SqlTransaction transaction, string arInvoiceID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(arpPostedToGL,0) As arpPostedToGL From ARInvoices Where arpARInvoiceID = @ARInvoiceID");
		sqlCommand.Parameters.Add(new SqlParameter("@ARInvoiceID", SqlDbType.NVarChar)).Value = arInvoiceID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return false;
		}
		return (bool)obj;
	}
}
