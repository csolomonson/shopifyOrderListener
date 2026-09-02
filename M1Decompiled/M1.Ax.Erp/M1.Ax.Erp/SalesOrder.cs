using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class SalesOrder
{
	public void AddDeliveryLine(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		try
		{
			bindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries")?.AddNew();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void DefaultDeliveryQuantity(M1BindingSource bindingSource, DataRow deliveryRow)
	{
		try
		{
			DataRow dataRow = bindingSource.Fields["omdSalesOrderLineID"].RelatedTableGetDataRow("omlOrderQuantity,omlDeliveryQuantityTotal,omlPartID,omlPartRevisionID", null, deliveryRow);
			decimal num = dataRow.Field<decimal>("omlDeliveryQuantityTotal");
			if (num == 0m)
			{
				deliveryRow.SetField("omdDeliveryQuantity", dataRow.Field<decimal>("omlOrderQuantity"));
			}
			else if (num < dataRow.Field<decimal>("omlOrderQuantity"))
			{
				deliveryRow.SetField("omdDeliveryQuantity", dataRow.Field<decimal>("omlOrderQuantity") - num);
			}
			if (string.IsNullOrWhiteSpace(deliveryRow.Field<string>("omdPartID")) && !string.IsNullOrWhiteSpace(dataRow.Field<string>("omlPartID")))
			{
				deliveryRow["omdPartID"] = dataRow["omlPartID"];
				deliveryRow["omdPartRevisionID"] = dataRow["omlPartRevisionID"];
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void UpdatePurchaseUnitCost(M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		string currencyID = "";
		DateTime? priceDate = DateTime.Today;
		if (currentAsDataRow == null)
		{
			return;
		}
		SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("select ompOrderDate,ompCurrencyRateID from SalesOrders where ompSalesOrderID = @OrderID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = currentAsDataRow.Field<string>("omdSalesOrderID");
		DataTable dataTable = bindingSource.Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			currencyID = dataTable.Rows[0].Field<string>("ompCurrencyRateID");
			priceDate = dataTable.Rows[0].Field<DateTime?>("ompOrderDate");
		}
		PriceCalculation purchasePrice = new Part().GetPurchasePrice(bindingSource.Database, currentAsDataRow.Field<string>("omdPartID"), currentAsDataRow.Field<string>("omdPartRevisionID"), currentAsDataRow.Field<string>("omdSupplierOrganizationID"), currentAsDataRow.Field<string>("omdPurchaseLocationID"), currentAsDataRow.Field<decimal>("omdDeliveryQuantity"), "MATERIAL", currencyID, priceDate, 0m, null);
		if (purchasePrice.PartPrice == null && purchasePrice.CalculationType != PriceCalculationType.PartCost)
		{
			return;
		}
		if (purchasePrice.IsForeignCurrency)
		{
			if (purchasePrice.FullPrice != currentAsDataRow.Field<decimal>("omdPurchaseUnitCostForeign"))
			{
				currentAsDataRow.SetField("omdPurchaseUnitCostForeign", purchasePrice.FullPrice);
			}
		}
		else if (purchasePrice.FullPrice != currentAsDataRow.Field<decimal>("omdPurchaseUnitCostBase"))
		{
			currentAsDataRow.SetField("omdPurchaseUnitCostBase", purchasePrice.FullPrice);
		}
	}

	public bool IsDeliveryTypeJobLinkValid(M1Database database, string orderID, decimal lineID, decimal deliveryID, object linkType)
	{
		int num = short.Parse(linkType.ToString());
		SqlCommand sqlCommand;
		if (deliveryID == 0m)
		{
			sqlCommand = database.NewSqlCommand("select Count(omdDeliveryType) from SalesOrderDeliveries where omdSalesOrderID = @OrderID and omdSalesOrderLineID = @LineID and omdDeliveryType = 1");
			sqlCommand.Parameters.AddWithValue("@OrderID", orderID);
			sqlCommand.Parameters.AddWithValue("@LineID", lineID);
			return short.Parse(database.ExecuteScalar(sqlCommand).ToString()) > 0;
		}
		sqlCommand = database.NewSqlCommand("select omdDeliveryType from SalesOrderDeliveries where omdSalesOrderID = @OrderID and omdSalesOrderLineID = @LineID and omdSalesOrderDeliveryID = @DeliveryID");
		sqlCommand.Parameters.AddWithValue("@OrderID", orderID);
		sqlCommand.Parameters.AddWithValue("@LineID", lineID);
		sqlCommand.Parameters.AddWithValue("@DeliveryID", deliveryID);
		if (short.Parse(database.ExecuteScalar(sqlCommand).ToString()) != 1)
		{
			return num != 1;
		}
		return true;
	}

	public string CreateSalesOrder(M1Database database, string customerID, string locationID, string currencyID, string partID, string revisionID, decimal orderQty = 1m, string orderID = "", string shipOrgID = "", string shipLocationID = "", string customerPO = "", string orgContactID = "", string shipContactID = "")
	{
		using M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "SalesOrders", null, true);
		DataRow dataRow = m1BindingSource.AddNew() as DataRow;
		if (!string.IsNullOrWhiteSpace(orderID))
		{
			dataRow.SetField("ompSalesOrderID", orderID);
		}
		else
		{
			m1BindingSource.SetKeyToNextAvailable(dataRow);
		}
		dataRow.SetField("ompCustomerOrganizationID", customerID);
		dataRow.SetField("ompARInvoiceLocationID", locationID);
		if (!string.IsNullOrWhiteSpace(orgContactID))
		{
			dataRow.SetField("ompARInvoiceContactID", orgContactID);
		}
		if (!string.IsNullOrWhiteSpace(shipOrgID))
		{
			dataRow.SetField("ompShipOrganizationID", shipOrgID);
			dataRow.SetField("ompShipLocationID", shipLocationID);
			dataRow.SetField("ompShipContactID", shipContactID);
		}
		if (!string.IsNullOrWhiteSpace(customerPO))
		{
			dataRow.SetField("ompCustomerPO", customerPO);
		}
		if (!string.IsNullOrWhiteSpace(currencyID))
		{
			dataRow.SetField("ompCurrencyRateID", currencyID);
		}
		m1BindingSource.SaveData();
		if (!string.IsNullOrWhiteSpace(partID))
		{
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("SalesOrderLines");
			DataRow dataRow2 = childBindingSource.AddNew() as DataRow;
			childBindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2.SetField("omlPartID", partID);
			dataRow2.SetField("omlPartRevisionID", revisionID);
			if (string.IsNullOrWhiteSpace(dataRow2.Field<string>("omlPartShortDescription")))
			{
				dataRow2.SetField("omlPartShortDescription", partID);
			}
			dataRow2.SetField("omlOrderQuantity", orderQty);
		}
		m1BindingSource.SaveData();
		return dataRow.Field<string>("ompSalesOrderID");
	}

	public void CreateSalesOrderJobLinks(M1Database database, SqlTransaction transaction, string orderID, int salesOrderLineID, int salesOrderJobLinkID, int linkType, int salesOrderDeliveryID, string jobID, bool closed, string createdBy, DateTime createdDate)
	{
		using M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
		m1BindingSource.LoadDefinition(string.Empty, "SalesOrderJobLinks", null, true);
		DataRow row = m1BindingSource.AddNew() as DataRow;
		row.SetField("omjSalesOrderID", orderID);
		row.SetField("omjSalesOrderLineID", salesOrderLineID);
		row.SetField("omjSalesOrderJobLinkID", salesOrderJobLinkID);
		row.SetField("omjLinkType", linkType);
		row.SetField("omjSalesOrderDeliveryID", salesOrderDeliveryID);
		row.SetField("omjJobID", jobID);
		row.SetField("omjClosed", closed);
		row.SetField("omjCreatedBy", createdBy);
		row.SetField("omjCreatedDate", createdDate);
		m1BindingSource.SaveData();
	}

	public void UpdateQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (!row.Table.Columns.Contains("ShippedComplete") || !row.Table.Columns.Contains("OpenQty") || !row.Table.Columns.Contains("QuantityShipped") || !row.Table.Columns.Contains("FieldSelected"))
		{
			return;
		}
		flag = ((!changedField.Equals("FieldSelected")) ? row.Field<bool>("ShippedComplete") : (row.Field<bool>("FieldSelected") ? true : false));
		num = row.Field<decimal>("OpenQty");
		if (flag)
		{
			if (row.Field<decimal>("QuantityShipped") == 0m)
			{
				row.SetField("QuantityShipped", num);
			}
		}
		else if (changedField.Equals("FieldSelected"))
		{
			row.SetField("QuantityShipped", 0m);
		}
		row.SetField("ShippedComplete", flag);
	}

	public void RefreshTaxSubtotal(M1Database database, M1BindingSource bsOrder, SqlTransaction transaction)
	{
		DataRow currentAsDataRow = bsOrder.CurrentAsDataRow;
		bool flag = false;
		if (string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("ompSalesOrderID")))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(sum(omlTaxAmountBase),0) As omlTaxAmountBase,IsNull(sum(omlTaxAmountBasePos),0) as omlTaxAmountBasePos,IsNull(sum(omlTaxAmountBaseNeg),0) as omlTaxAmountBaseNeg,IsNull(Sum(omlTaxAmountForeignPos),0) As omlTaxAmountForeignPos,IsNull(Sum(omlTaxAmountForeignNeg),0) As omlTaxAmountForeignNeg From(select omlTaxCodeID, Round(sum(omlTaxAmountBase), 2) As omlTaxAmountBase, Round(sum(omlTaxAmountBasePos), 2) as omlTaxAmountBasePos, Round(sum(omlTaxAmountBaseNeg), 2) as omlTaxAmountBaseNeg, Round(Sum(omlTaxAmountForeignPos), 2) As omlTaxAmountForeignPos, Round(Sum(omlTaxAmountForeignNeg), 2) As omlTaxAmountForeignNeg From(select omlTaxCodeID, sum(omlTaxAmountBase) as omlTaxAmountBase, sum(Case When omlTaxAmountBase > 0 Then omlTaxAmountBase Else 0 End) as omlTaxAmountBasePos, sum(Case When omlTaxAmountBase < 0 Then omlTaxAmountBase Else 0 End) as omlTaxAmountBaseNeg, sum(Case When omlTaxAmountForeign > 0 Then omlTaxAmountForeign Else 0 End) As omlTaxAmountForeignPos, sum(Case When omlTaxAmountForeign < 0 Then omlTaxAmountForeign Else 0 End) As omlTaxAmountForeignNeg, (Case When IsNull(TaxCodePlants.xtpAccrualGLAccountID, '') <> '' Then TaxCodePlants.xtpAccrualGLAccountID Else TaxCodes.xaxAccrualGLAccountID End) As xaxAccrualGLAccountID From(Select ompPlantID, omlTaxCodeID, omlTaxAmountBase, omlTaxAmountForeign From SalesOrders Inner Join SalesOrderLines On ompSalesOrderID = omlSalesOrderID where omlSalesOrderID = @orderID And omlTaxAmountBase <> 0 Union All Select ompPlantID, omlSecondTaxCodeID As omlTaxCodeID, omlSecondTaxAmountBase As omlTaxAmountBase, omlSecondTaxAmountForeign As omlTaxAmountForeign From SalesOrders Inner Join SalesOrderLines On ompSalesOrderID = omlSalesOrderID where omlSalesOrderID = @orderID And omlSecondTaxAmountBase <> 0 ) As Test left outer join TaxCodes on omlTaxCodeID = xaxTaxCodeID Left Outer Join TaxCodePlants On omlTaxCodeID = xtpTaxCodeID And ompPlantID = xtpPlantID group by ompPlantID, omlTaxCodeID, xaxAccrualGLAccountID, xtpAccrualGLAccountID) as Test2 Group By omlTaxCodeID) As Test3");
		sqlCommand.Parameters.Add(new SqlParameter("@orderID", SqlDbType.NVarChar)).Value = currentAsDataRow.Field<string>("ompSalesOrderID");
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0 && bsOrder.CurrentAsDataRow.Field<decimal>("ompTaxSubtotalForeign") != Convert.ToDecimal(dataTable.Rows[0]["omlTaxAmountForeignPos"]) + Convert.ToDecimal(dataTable.Rows[0]["omlTaxAmountForeignNeg"]))
		{
			bsOrder.CurrentAsDataRow.SetField("ompTaxSubtotalForeign", Convert.ToDecimal(dataTable.Rows[0]["omlTaxAmountForeignPos"]) + Convert.ToDecimal(dataTable.Rows[0]["omlTaxAmountForeignNeg"]));
			bsOrder.CurrentAsDataRow.SetField("ompTaxSubtotalBase", Convert.ToDecimal(dataTable.Rows[0]["omlTaxAmountBasePos"]) + Convert.ToDecimal(dataTable.Rows[0]["omlTaxAmountBaseNeg"]));
			flag = true;
		}
		if (!flag || bsOrder.InSaveData)
		{
			return;
		}
		bool flag2 = false;
		if (bsOrder.Errors != null)
		{
			foreach (ValidationInfo error in bsOrder.Errors)
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
			bsOrder.SaveData();
		}
	}

	public bool RefreshOrderTotal(M1Database database, string orderId, bool forceApprovalCheck)
	{
		if (!string.IsNullOrEmpty(orderId))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT ISNULL(SUM(omlFullExtendedPriceBase), 0) AS omlFullExtendedPriceBase, \r\n                                                             ISNULL(SUM(omlFullExtendedPriceForeign), 0) AS omlFullExtendedPriceForeign,\r\n                                                             ISNULL(SUM(omlExtendedDiscountBase), 0) AS omlExtendedDiscountBase,\r\n                                                             ISNULL(SUM(omlExtendedDiscountForeign), 0) AS omlExtendedDiscountForeign,\r\n                                                             ISNULL(SUM(omlExtendedPriceBase), 0) AS omlExtendedPriceBase,\r\n                                                             ISNULL(SUM(omlExtendedPriceForeign), 0) AS omlExtendedPriceForeign,\r\n                                                             ISNULL(SUM(omlFreightAmountBase), 0) AS omlFreightAmountBase,\r\n                                                             ISNULL(SUM(omlFreightAmountForeign), 0) AS omlFreightAmountForeign,\r\n                                                             ISNULL(SUM(omlTaxAmountBase), 0) AS omlTaxAmountBase,\r\n                                                             ISNULL(SUM(omlTaxAmountForeign), 0) AS omlTaxAmountForeign,\r\n                                                             ISNULL(SUM(omlSecondTaxAmountBase), 0) AS omlSecondTaxAmountBase,\r\n                                                             ISNULL(SUM(omlSecondTaxAmountForeign), 0) AS omlSecondTaxAmountForeign,\r\n                                                             ISNULL(SUM(omlWeight*omlOrderQuantity), 0) AS omlWeight,\r\n                                                             ISNULL(SUM(omlDepositAmountBase), 0) AS omlDepositAmountBase,\r\n                                                             ISNULL(SUM(omlDepositAmountForeign), 0) AS omlDepositAmountForeign\r\n                                                    FROM SalesOrderLines\r\n                                                    WHERE omlSalesOrderID = @OrderId");
			sqlCommand.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.NVarChar)).Value = orderId;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				decimal num = row.Field<decimal>("omlExtendedPriceBase");
				decimal num2 = row.Field<decimal>("omlExtendedPriceForeign");
				decimal num3 = row.Field<decimal>("omlFullExtendedPriceBase");
				decimal num4 = row.Field<decimal>("omlFullExtendedPriceForeign");
				decimal num5 = row.Field<decimal>("omlExtendedDiscountBase");
				decimal num6 = row.Field<decimal>("omlExtendedDiscountForeign");
				decimal num7 = row.Field<decimal>("omlFreightAmountBase");
				decimal num8 = row.Field<decimal>("omlFreightAmountForeign");
				decimal num9 = row.Field<decimal>("omlTaxAmountBase") + row.Field<decimal>("omlSecondTaxAmountBase");
				decimal num10 = row.Field<decimal>("omlTaxAmountForeign") + row.Field<decimal>("omlSecondTaxAmountForeign");
				decimal num11 = row.Field<decimal>("omlWeight");
				decimal num12 = row.Field<decimal>("omlDepositAmountBase");
				decimal num13 = row.Field<decimal>("omlDepositAmountForeign");
				SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT ompExchangeRate,ompOrderTotalBase,ompFreightAmountBase,ompFreightAmountForeign,ompFreightTaxCodeID, \r\n                                                            ompSecondFreightTaxCodeID,ompStatus,ompOrderDate,ompAvalaraTaxCalculated,ompFreightTaxAmountBase,\r\n                                                            ompSecondFreightTaxAmtBase,ompOrderTaxAmountBase\r\n                                                    FROM SalesOrders\r\n                                                    WHERE ompSalesOrderID = @OrderId");
				sqlCommand2.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.NVarChar)).Value = orderId;
				DataTable dataTable2 = database.GetDataTable(sqlCommand2);
				if (dataTable2 != null && dataTable2.Rows.Count != 0)
				{
					DataRow row2 = dataTable2.Rows[0];
					bool num14 = row2.Field<bool>("ompAvalaraTaxCalculated");
					decimal num15 = row2.Field<decimal>("ompFreightAmountBase");
					decimal num16 = row2.Field<decimal>("ompFreightAmountForeign");
					decimal num17 = default(decimal);
					decimal num18 = default(decimal);
					decimal num19 = default(decimal);
					if (!num14)
					{
						num17 = Convert.ToDecimal(new AppAxFinancial(database).CalculateTaxOnSubTotal(row2.Field<string>("ompFreightTaxCodeID"), Convert.ToDouble(num15 + num7), row2.Field<DateTime>("ompOrderDate"), 0.0, 4));
						num18 = Convert.ToDecimal(new AppAxFinancial(database).CalculateTaxOnSubTotal(row2.Field<string>("ompSecondFreightTaxCodeID"), Convert.ToDouble(num15 + num7), row2.Field<DateTime>("ompOrderDate"), Convert.ToDouble(num17), 4));
						num19 = M1Math.Round(num9 + num17 + num18, 2);
					}
					else
					{
						num17 = row2.Field<decimal>("ompFreightTaxAmountBase");
						num18 = row2.Field<decimal>("ompSecondFreightTaxAmtBase");
						num19 = M1Math.Round(num9 + num17 + num18, 2);
					}
					decimal num20 = row2.Field<decimal>("ompExchangeRate");
					decimal num21 = M1Math.Round(num17 * num20, 4);
					decimal num22 = M1Math.Round(num18 * num20, 4);
					decimal num23 = M1Math.Round(num10 + num21 + num22, 2);
					decimal num24 = num + num7 + num15 + num19;
					decimal num25 = num2 + num8 + num16 + num23;
					string text = string.Empty;
					double salesPersonAmount = new AppAxProduction(database).SalesPersonAmount;
					if (num24 != row2.Field<decimal>("ompOrderTotalBase") || forceApprovalCheck)
					{
						if (salesPersonAmount != 0.0 && Convert.ToDecimal(salesPersonAmount) < num24)
						{
							text = ", ompStatus = 1, ompReadyToPrint = 0, ompApprovalDecisionDate = Null ";
						}
						else if (row2.Field<byte>("ompStatus").Equals(2))
						{
							text = ", ompStatus = 3 ";
						}
						text += ", ompNextApprovalEmployeeID = '', ompApprovalRequestDate = NULL ";
						SqlCommand sqlCommand3 = database.NewSqlCommand("DELETE FROM SalesOrderApprovals Where omaSalesOrderID = @OrderId");
						sqlCommand3.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.NVarChar)).Value = orderId;
						database.ExecuteCommand(sqlCommand3);
					}
					string queryString = "UPDATE SalesOrders SET ompFullOrderSubtotalBase = @FullOrderSubtotalBase, ompFullOrderSubtotalForeign = @FullOrderSubtotalForeign,\r\n                                                                             ompDiscountTotalBase = @DiscountTotalBase, ompDiscountTotalForeign = @DiscountTotalForeign,\r\n                                                                             ompOrderSubtotalBase = @OrderSubtotalBase, ompOrderSubtotalForeign = @OrderSubtotalForeign,\r\n                                                                             ompFreightSubtotalBase = @FreightSubtotalBase, ompFreightSubtotalForeign = @FreightSubtotalForeign,\r\n                                                                             ompFreightTotalBase = @FreightTotalBase + ompFreightAmountBase, ompFreightTotalForeign = @FreightTotalForeign + ompFreightAmountForeign,\r\n                                                                             ompFreightTaxAmountBase = @FreightTaxAmountBase, ompFreightTaxAmountForeign = @FreightTaxAmountForeign,\r\n                                                                             ompSecondFreightTaxAmtBase = @SecondFreightTaxAmtBase, ompSecondFreightTaxAmtForeign = @SecondFreightTaxAmtForeign,\r\n                                                                             ompOrderTaxAmountBase = @OrderTaxAmountBase, ompOrderTaxAmountForeign = @OrderTaxAmountForeign,\r\n                                                                             ompOrderTotalBase = @OrderTotalBase, ompOrderTotalForeign = @OrderTotalForeign,\r\n                                                                             ompTotalOrderWeight = @TotalOrderWeight, ompDepositAmountBase = @DepositAmountBase,\r\n                                                                             ompDepositAmountForeign = @DepositAmountForeign\r\n                                                                        WHERE ompSalesOrderID = @OrderId";
					SqlCommand sqlCommand4 = database.NewSqlCommand(queryString);
					sqlCommand4.Parameters.Add(new SqlParameter("@FullOrderSubtotalBase", SqlDbType.Money)).Value = num3;
					sqlCommand4.Parameters.Add(new SqlParameter("@FullOrderSubtotalForeign", SqlDbType.Money)).Value = num4;
					sqlCommand4.Parameters.Add(new SqlParameter("@DiscountTotalBase", SqlDbType.Money)).Value = num5;
					sqlCommand4.Parameters.Add(new SqlParameter("@DiscountTotalForeign", SqlDbType.Money)).Value = num6;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderSubtotalBase", SqlDbType.Money)).Value = num;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderSubtotalForeign", SqlDbType.Money)).Value = num2;
					sqlCommand4.Parameters.Add(new SqlParameter("@FreightSubtotalBase", SqlDbType.Money)).Value = num7;
					sqlCommand4.Parameters.Add(new SqlParameter("@FreightSubtotalForeign", SqlDbType.Money)).Value = num8;
					sqlCommand4.Parameters.Add(new SqlParameter("@FreightTotalBase", SqlDbType.Money)).Value = num7;
					sqlCommand4.Parameters.Add(new SqlParameter("@FreightTotalForeign", SqlDbType.Money)).Value = num8;
					sqlCommand4.Parameters.Add(new SqlParameter("@FreightTaxAmountBase", SqlDbType.Money)).Value = num17;
					sqlCommand4.Parameters.Add(new SqlParameter("@FreightTaxAmountForeign", SqlDbType.Money)).Value = num21;
					sqlCommand4.Parameters.Add(new SqlParameter("@SecondFreightTaxAmtBase", SqlDbType.Money)).Value = num18;
					sqlCommand4.Parameters.Add(new SqlParameter("@SecondFreightTaxAmtForeign", SqlDbType.Money)).Value = num22;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderTaxAmountBase", SqlDbType.Money)).Value = num19;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderTaxAmountForeign", SqlDbType.Money)).Value = num23;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderTotalBase", SqlDbType.Money)).Value = num24;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderTotalForeign", SqlDbType.Money)).Value = num25;
					sqlCommand4.Parameters.Add(new SqlParameter("@TotalOrderWeight", SqlDbType.Money)).Value = num11;
					sqlCommand4.Parameters.Add(new SqlParameter("@DepositAmountBase", SqlDbType.Money)).Value = num12;
					sqlCommand4.Parameters.Add(new SqlParameter("@DepositAmountForeign", SqlDbType.Money)).Value = num13;
					sqlCommand4.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.NVarChar)).Value = orderId;
					database.ExecuteCommand(sqlCommand4);
					return true;
				}
			}
		}
		return false;
	}
}
