using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PurchaseOrders
{
	public bool IsPurchaseOrderLandedCost(M1Database database, string purchaseOrderID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(pmpLandedCost,0) From PurchaseOrders Where pmpPurchaseOrderID = @PurchaseOrderID");
		sqlCommand.Parameters.Add(new SqlParameter("PurchaseOrderID", SqlDbType.NVarChar)).Value = purchaseOrderID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand));
	}

	public void UpdateQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (!row.Table.Columns.Contains("ReceivedComplete") || !row.Table.Columns.Contains("OpenQty") || !row.Table.Columns.Contains("PurchaseQtyRecd") || !row.Table.Columns.Contains("FieldSelected"))
		{
			return;
		}
		flag = ((!changedField.Equals("FieldSelected")) ? row.Field<bool>("ReceivedComplete") : (row.Field<bool>("FieldSelected") ? true : false));
		num = row.Field<decimal>("OpenQty");
		if (flag)
		{
			if (row.Field<decimal>("PurchaseQtyRecd") == 0m)
			{
				row.SetField("PurchaseQtyRecd", num);
			}
		}
		else if (changedField.Equals("FieldSelected"))
		{
			row.SetField("PurchaseQtyRecd", 0m);
		}
		row.SetField("ReceivedComplete", flag);
	}

	public byte GetPurchaseOrderLineType(M1Database database, SqlTransaction transaction, string poID, int poLineID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pmlPurchaseType FROM PurchaseOrderLines WHERE pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @PoLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = poID;
		sqlCommand.Parameters.Add(new SqlParameter("@PoLineID", SqlDbType.Int)).Value = poLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<byte>("pmlPurchaseType");
		}
		return 0;
	}

	public string GetPurchaseOrderLineFixedAssetType(M1Database database, SqlTransaction transaction, string poID, int poLineID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pmlAssetTypeID FROM PurchaseOrderLines WHERE pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @PoLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = poID;
		sqlCommand.Parameters.Add(new SqlParameter("@PoLineID", SqlDbType.Int)).Value = poLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<string>("pmlAssetTypeID");
		}
		return string.Empty;
	}

	public decimal GetPurchaseOrderLineExtendedCost(M1Database database, SqlTransaction transaction, string poID, int poLineID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pmlExtendedCostBase FROM PurchaseOrderLines WHERE pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @PoLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = poID;
		sqlCommand.Parameters.Add(new SqlParameter("@PoLineID", SqlDbType.Int)).Value = poLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<decimal>("pmlExtendedCostBase");
		}
		return 0m;
	}

	public decimal GetTotalComponentsCost(M1BindingSource bindingSource, DataRow currentRow)
	{
		decimal result = default(decimal);
		if (bindingSource != null)
		{
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderComponents");
			if (childBindingSource != null)
			{
				foreach (DataRow row in childBindingSource.GetDataView(currentRow).ToTable().Rows)
				{
					result += (row.Field<decimal>("pmoDeliveryQuantity") + row.Field<decimal>("pmoAdditionalQuantity")) * row.Field<decimal>("pmoPurchaseUnitCost");
				}
			}
		}
		return result;
	}

	public void RefreshTaxSubtotal(M1Database database, M1BindingSource bsOrder, SqlTransaction transaction)
	{
		DataRow currentAsDataRow = bsOrder.CurrentAsDataRow;
		bool flag = false;
		if (string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("pmpPurchaseOrderID")))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(sum(pmlTaxAmountBase),0) As pmlTaxAmountBase,IsNull(sum(pmlTaxAmountBasePos),0) as pmlTaxAmountBasePos,IsNull(sum(pmlTaxAmountBaseNeg),0) as pmlTaxAmountBaseNeg,IsNull(Sum(pmlTaxAmountForeignPos),0) As pmlTaxAmountForeignPos,IsNull(Sum(pmlTaxAmountForeignNeg),0) As pmlTaxAmountForeignNeg From(select pmlTaxCodeID, Round(sum(pmlTaxAmountBase), 2) As pmlTaxAmountBase, Round(sum(pmlTaxAmountBasePos), 2) as pmlTaxAmountBasePos, Round(sum(pmlTaxAmountBaseNeg), 2) as pmlTaxAmountBaseNeg, Round(Sum(pmlTaxAmountForeignPos), 2) As pmlTaxAmountForeignPos, Round(Sum(pmlTaxAmountForeignNeg), 2) As pmlTaxAmountForeignNeg From(select pmlTaxCodeID, sum(pmlTaxAmountBase) as pmlTaxAmountBase, sum(Case When pmlTaxAmountBase > 0 Then pmlTaxAmountBase Else 0 End) as pmlTaxAmountBasePos, sum(Case When pmlTaxAmountBase < 0 Then pmlTaxAmountBase Else 0 End) as pmlTaxAmountBaseNeg, sum(Case When pmlTaxAmountForeign > 0 Then pmlTaxAmountForeign Else 0 End) As pmlTaxAmountForeignPos, sum(Case When pmlTaxAmountForeign < 0 Then pmlTaxAmountForeign Else 0 End) As pmlTaxAmountForeignNeg, (Case When IsNull(TaxCodePlants.xtpAccrualGLAccountID, '') <> '' Then TaxCodePlants.xtpAccrualGLAccountID Else TaxCodes.xaxAccrualGLAccountID End) As xaxAccrualGLAccountID From(Select pmpPlantID, pmlTaxCodeID, pmlTaxAmountBase, pmlTaxAmountForeign From PurchaseOrders Inner Join PurchaseOrderLines On pmpPurchaseOrderID = pmlPurchaseOrderID where pmlPurchaseOrderID = @orderID And pmlTaxAmountBase <> 0 Union All Select pmpPlantID, pmlSecondTaxCodeID As pmlTaxCodeID, pmlSecondTaxAmountBase As pmlTaxAmountBase, pmlSecondTaxAmountForeign As pmlTaxAmountForeign From PurchaseOrders Inner Join PurchaseOrderLines On pmpPurchaseOrderID = pmlPurchaseOrderID where pmlPurchaseOrderID = @orderID And pmlSecondTaxAmountBase <> 0 ) As Test left outer join TaxCodes on pmlTaxCodeID = xaxTaxCodeID Left Outer Join TaxCodePlants On pmlTaxCodeID = xtpTaxCodeID And pmpPlantID = xtpPlantID group by pmpPlantID, pmlTaxCodeID, xaxAccrualGLAccountID, xtpAccrualGLAccountID) as Test2 Group By pmlTaxCodeID) As Test3");
		sqlCommand.Parameters.Add(new SqlParameter("@orderID", SqlDbType.NVarChar)).Value = currentAsDataRow.Field<string>("pmpPurchaseOrderID");
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0 && bsOrder.CurrentAsDataRow.Field<decimal>("pmpOrderTaxAmountForeign") != Convert.ToDecimal(dataTable.Rows[0]["pmlTaxAmountForeignPos"]) + Convert.ToDecimal(dataTable.Rows[0]["pmlTaxAmountForeignNeg"]))
		{
			bsOrder.CurrentAsDataRow.SetField("pmpOrderTaxAmountForeign", Convert.ToDecimal(dataTable.Rows[0]["pmlTaxAmountForeignPos"]) + Convert.ToDecimal(dataTable.Rows[0]["pmlTaxAmountForeignNeg"]));
			bsOrder.CurrentAsDataRow.SetField("pmpOrderTaxAmountBase", Convert.ToDecimal(dataTable.Rows[0]["pmlTaxAmountBasePos"]) + Convert.ToDecimal(dataTable.Rows[0]["pmlTaxAmountForeignNeg"]));
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

	public decimal GetPurchaseOrderLineQuantity(M1Database database, SqlTransaction transaction, string poID, int poLineID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pmlInventoryQuantity FROM PurchaseOrderLines WHERE pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @PoLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = poID;
		sqlCommand.Parameters.Add(new SqlParameter("@PoLineID", SqlDbType.Int)).Value = poLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<decimal>("pmlInventoryQuantity");
		}
		return 0m;
	}

	public bool RefreshPurchaseOrderTotal(M1Database database, string purchaseOrderId, SqlTransaction transaction)
	{
		if (!string.IsNullOrEmpty(purchaseOrderId))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT ISNULL(SUM(pmlExtendedCostBase), 0) AS pmlExtendedCostBase, \r\n                                                             ISNULL(SUM(pmlExtendedCostForeign), 0) AS pmlExtendedCostForeign,\r\n                                                             ISNULL(SUM(pmlTaxAmountBase), 0) AS pmlTaxAmountBase,\r\n                                                             ISNULL(SUM(pmlTaxAmountForeign), 0) AS pmlTaxAmountForeign,\r\n                                                             ISNULL(SUM(pmlSecondTaxAmountBase), 0) AS pmlSecondTaxAmountBase,\r\n                                                             ISNULL(SUM(pmlSecondTaxAmountForeign), 0) AS pmlSecondTaxAmountForeign\r\n                                                    FROM PurchaseOrderLines\r\n                                                    WHERE pmlPurchaseOrderID = @PurchaseOrderId");
			sqlCommand.Parameters.Add(new SqlParameter("@PurchaseOrderId", SqlDbType.NVarChar)).Value = purchaseOrderId;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				decimal num = row.Field<decimal>("pmlExtendedCostBase");
				decimal num2 = row.Field<decimal>("pmlExtendedCostForeign");
				decimal num3 = row.Field<decimal>("pmlTaxAmountBase") + row.Field<decimal>("pmlSecondTaxAmountBase");
				decimal num4 = row.Field<decimal>("pmlTaxAmountForeign") + row.Field<decimal>("pmlSecondTaxAmountForeign");
				SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT pmpOrderSubtotalBase, pmpOrderSubtotalForeign, pmpOrderTaxAmountBase, pmpOrderTaxAmountForeign, \r\n                                                            pmpOrderTotalBase, pmpOrderTotalForeign \r\n                                                    FROM PurchaseOrders\r\n                                                    WHERE pmpPurchaseOrderID = @PurchaseOrderId");
				sqlCommand2.Parameters.Add(new SqlParameter("@PurchaseOrderId", SqlDbType.NVarChar)).Value = purchaseOrderId;
				DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
				if (dataTable2 != null && dataTable2.Rows.Count != 0)
				{
					decimal num5 = num + num3;
					decimal num6 = num2 + num4;
					string queryString = "UPDATE PurchaseOrders SET pmpOrderSubtotalBase = @OrderSubtotalBase, pmpOrderSubtotalForeign = @OrderSubtotalForeign,\r\n                                                                             pmpOrderTaxAmountBase = @OrderTaxAmountBase, pmpOrderTaxAmountForeign = @OrderTaxAmountForeign,\r\n                                                                             pmpOrderTotalBase = @OrderTotalBase, pmpOrderTotalForeign = @OrderTotalForeign\r\n                                                                        WHERE pmpPurchaseOrderID = @PurchaseOrderId";
					SqlCommand sqlCommand3 = database.NewSqlCommand(queryString);
					sqlCommand3.Parameters.Add(new SqlParameter("@OrderSubtotalBase", SqlDbType.Money)).Value = num;
					sqlCommand3.Parameters.Add(new SqlParameter("@OrderSubtotalForeign", SqlDbType.Money)).Value = num2;
					sqlCommand3.Parameters.Add(new SqlParameter("@OrderTaxAmountBase", SqlDbType.Money)).Value = num3;
					sqlCommand3.Parameters.Add(new SqlParameter("@OrderTaxAmountForeign", SqlDbType.Money)).Value = num4;
					sqlCommand3.Parameters.Add(new SqlParameter("@OrderTotalBase", SqlDbType.Money)).Value = num5;
					sqlCommand3.Parameters.Add(new SqlParameter("@OrderTotalForeign", SqlDbType.Money)).Value = num6;
					sqlCommand3.Parameters.Add(new SqlParameter("@PurchaseOrderId", SqlDbType.NVarChar)).Value = purchaseOrderId;
					database.ExecuteCommand(sqlCommand3, transaction);
					return true;
				}
			}
		}
		return false;
	}
}
