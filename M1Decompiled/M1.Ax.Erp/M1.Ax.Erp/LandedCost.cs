using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class LandedCost
{
	public DateTime GetLandedCostDate(M1Database database, SqlTransaction transaction, string landedCostID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select rmcLandedCostDate from LandedCosts where rmcLandedCostID = @LandedCostID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = landedCostID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj != null)
		{
			return (DateTime)obj;
		}
		return DateTime.Now;
	}

	public void AddPOLineToLandedCost(M1BindingSource bindingSource, List<DataRow> selectedRows)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return;
		}
		string text = currentAsDataRow.Field<string>("rmcLandedCostID");
		decimal totalLineCost = default(decimal);
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		foreach (DataRow selectedRow in selectedRows)
		{
			SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("Update PurchaseOrderLines Set pmlLandedCostID = @LandedCostID Where pmlLandedCostID = '' And pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @LineID");
			sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = text;
			sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = selectedRow["pmlPurchaseOrderID"];
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = selectedRow["pmlPurchaseOrderLineID"];
			bindingSource.Database.ExecuteScalar(sqlCommand);
			totalLineCost = selectedRow.Field<decimal>("pmlPurchaseQuantity") * (selectedRow.Field<decimal>("pmlPurchaseUnitCostBase") + Math.Round(selectedRow.Field<decimal>("pmlSetupChargeBase") / selectedRow.Field<decimal>("pmlPurchaseQuantity"), 5));
		}
		decimal totalCharges = (decimal)bindingSource.Database.ExecuteScalar("Select IsNull(Sum(rmhTotalCost),0) As ChargeTotal From LandedCostCharges Where rmhLandedCostID = " + text.ToSql());
		decimal totalReceiptCost = (decimal)bindingSource.Database.ExecuteScalar("Select isnull(sum(rmlExtendedCostBase),0) as TotalReceiptsCost from ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID where rmpLandedCostID = " + text.ToSql());
		RefreshLandedCostTotals(bindingSource.Database, null, text, totalCharges, totalLineCost, totalReceiptCost);
		bindingSource.OnDataChanged(new DataChangedEventArgs(DataChangedFlag.CurrentRow));
	}

	public void RefreshLandedCostChargeDetails(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		if (bindingsource == null || bindingsource.CurrentAsDataRow == null)
		{
			return;
		}
		string text = bindingsource.CurrentAsDataRow.Field<string>("rmcLandedCostID");
		bool flag = bindingsource.CurrentAsDataRow.Field<bool>("rmcChargesJournalsCreated");
		M1Database database = bindingsource.Database;
		M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource("LandedCostCharges");
		if (childBindingSource == null)
		{
			return;
		}
		DataTable dataTable = database.GetDataTable("Select * From LandedCostChargeDetails Where rmiLandedCostID = " + text.ToSql());
		SqlCommand sqlCommand = database.NewSqlCommand("Delete From LandedCostChargeDetails Where rmiLandedCostID = @LandedCostID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = text;
		database.ExecuteCommand(sqlCommand, transaction);
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = default(decimal);
		decimal num4 = default(decimal);
		decimal totalCharges = default(decimal);
		sqlCommand = database.NewSqlCommand("select pmlPurchaseOrderID, pmlPurchaseOrderLineID, pmlInventoryQuantity, pmlPurchaseQuantity, pmlPurchaseUnitCostBase, pmlSetupChargeBase, IsNull(imrWeight, 0) As imrWeight, IsNull(imrVolume, 0) As imrVolume from PurchaseOrderLines Left Join PartRevisions on pmlPartID = imrPartID And pmlPartRevisionID = imrPartRevisionID Where pmlLandedCostID = @LandedCostID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = text;
		DataTable dataTable2 = database.GetDataTable(sqlCommand, transaction);
		if (dataTable2.Rows.Count != 0)
		{
			SqlDataAdapter adapter = null;
			DataTable dataTable3 = database.GetDataTable("Select * From LandedCostChargeDetails Where 0=1", fillSchema: false, out adapter, transaction);
			num = dataTable2.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("pmlPurchaseQuantity") * (x.Field<decimal>("pmlPurchaseUnitCostBase") + Math.Round(x.Field<decimal>("pmlSetupChargeBase") / x.Field<decimal>("pmlPurchaseQuantity"), 5)));
			num2 = dataTable2.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("pmlInventoryQuantity"));
			num3 = dataTable2.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("imrWeight") * x.Field<decimal>("pmlInventoryQuantity"));
			num4 = dataTable2.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("imrVolume") * x.Field<decimal>("pmlInventoryQuantity"));
			foreach (DataRow chargeRow in childBindingSource.GetDataView(bindingsource.CurrentAsDataRow).ToTable().Rows)
			{
				int num5 = 0;
				byte chargeType = chargeRow.Field<byte>("rmhLandedCostMethod");
				decimal num6 = chargeRow.Field<decimal>("rmhTotalCost");
				decimal num7 = chargeRow.Field<decimal>("rmhEstTotalCost");
				totalCharges += num6;
				foreach (DataRow row in dataTable2.Rows)
				{
					decimal tempPercent = 0.0m;
					decimal num8 = row.Field<decimal>("pmlInventoryQuantity");
					decimal tempTotal = (row.Field<decimal>("pmlPurchaseUnitCostBase") + Math.Round(row.Field<decimal>("pmlSetupChargeBase") / row.Field<decimal>("pmlPurchaseQuantity"), 5)) * row.Field<decimal>("pmlPurchaseQuantity");
					decimal tempWeight = row.Field<decimal>("imrWeight") * num8;
					decimal tempVolume = row.Field<decimal>("imrVolume") * num8;
					DataRow dataRow2 = dataTable3.NewRow().BlankRow();
					dataRow2["rmiLandedCostID"] = chargeRow["rmhLandedCostID"];
					dataRow2["rmiLandedCostChargeID"] = chargeRow["rmhLandedCostChargeID"];
					dataRow2["rmiLandedCostChargeDetailID"] = ++num5;
					dataRow2["rmiPurchaseOrderID"] = row["pmlPurchaseOrderID"];
					dataRow2["rmiPurchaseOrderLineID"] = row["pmlPurchaseOrderLineID"];
					tempPercent = getLinePercent(num, num2, num3, num4, chargeType, tempPercent, num8, tempTotal, tempWeight, tempVolume);
					dataRow2["rmiTotalCost"] = ((num8 <= 0m) ? 0m : Math.Round(tempPercent * num6, 2));
					dataRow2["rmiTotalCostForeign"] = Math.Round(dataRow2.Field<decimal>("rmiTotalCost") * chargeRow.Field<decimal>("rmhExchangeRate"), 2);
					dataRow2["rmiEstTotalCost"] = ((num8 <= 0m) ? 0m : Math.Round(tempPercent * num7, 2));
					dataRow2["rmiEstTotalCostForeign"] = Math.Round(dataRow2.Field<decimal>("rmiEstTotalCost") * chargeRow.Field<decimal>("rmhExchangeRate"), 2);
					dataRow2["rmiCreatedBy"] = database.User.ID;
					dataRow2["rmiCreatedDate"] = DateTime.Now;
					if (flag)
					{
						DataRow[] array = dataTable.Select(string.Format("rmiLandedCostID = '{0}' AND rmiLandedCostChargeID = '{1}' AND rmiLandedCostChargeDetailID = '{2}' ", dataRow2["rmiLandedCostID"].ToString(), dataRow2["rmiLandedCostChargeID"].ToString(), dataRow2["rmiLandedCostChargeDetailID"].ToString()));
						dataRow2["rmiUniqueID"] = array[0].Field<Guid>("rmiUniqueID");
					}
					dataTable3.Rows.Add(dataRow2);
				}
				decimal num9 = (from y in dataTable3.AsEnumerable()
					where y.Field<short>("rmiLandedCostChargeID").Equals(chargeRow.Field<short>("rmhLandedCostChargeID"))
					select y).Sum((DataRow x) => x.Field<decimal>("rmiTotalCost"));
				decimal num10 = (from y in dataTable3.AsEnumerable()
					where y.Field<short>("rmiLandedCostChargeID").Equals(chargeRow.Field<short>("rmhLandedCostChargeID"))
					select y).Sum((DataRow x) => x.Field<decimal>("rmiEstTotalCost"));
				decimal num11 = num6 - num9;
				decimal num12 = num7 - num10;
				if (!(num11 != 0m) && !(num12 != 0m))
				{
					continue;
				}
				DataRow dataRow3 = dataTable3.Rows[dataTable3.Rows.Count - 1];
				if (dataRow3 != null)
				{
					if (num11 != 0m)
					{
						dataRow3.SetField("rmiTotalCost", dataRow3.Field<decimal>("rmiTotalCost") + num11);
						dataRow3.SetField("rmiTotalCostForeign", dataRow3.Field<decimal>("rmiTotalCostForeign") + Math.Round(num11 * chargeRow.Field<decimal>("rmhExchangeRate"), 2));
					}
					if (num12 != 0m)
					{
						dataRow3.SetField("rmiEstTotalCost", dataRow3.Field<decimal>("rmiEstTotalCost") + num12);
						dataRow3.SetField("rmiEstTotalCostForeign", dataRow3.Field<decimal>("rmiEstTotalCostForeign") + Math.Round(num12 * chargeRow.Field<decimal>("rmhExchangeRate"), 2));
					}
				}
			}
			database.UpdateData(dataTable3, adapter, transaction);
		}
		else
		{
			totalCharges = childBindingSource.GetDataView(bindingsource.CurrentAsDataRow).ToTable().AsEnumerable()
				.Sum((DataRow x) => x.Field<decimal>("rmhTotalCost"));
		}
		decimal totalReceiptCost = (decimal)bindingsource.Database.ExecuteScalar("Select isnull(sum(rmlExtendedCostBase),0) as TotalReceiptsCost from ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID where rmpLandedCostID = " + text.ToSql());
		RefreshLandedCostTotals(database, transaction, text, totalCharges, num, totalReceiptCost);
		bindingsource.OnDataChanged(new DataChangedEventArgs(DataChangedFlag.CurrentRow));
	}

	private static decimal getLinePercent(decimal totalLinesCost, decimal totalLinesQty, decimal totalLinesWeight, decimal totalLinesVolume, byte chargeType, decimal tempPercent, decimal tempQty, decimal tempTotal, decimal tempWeight, decimal tempVolume)
	{
		switch (chargeType)
		{
		case 1:
			if (totalLinesCost != 0m)
			{
				tempPercent = tempTotal / totalLinesCost;
			}
			break;
		case 2:
			if (totalLinesQty != 0m)
			{
				tempPercent = tempQty / totalLinesQty;
			}
			break;
		case 3:
			if (totalLinesWeight != 0m)
			{
				tempPercent = tempWeight / totalLinesWeight;
			}
			break;
		case 4:
			if (totalLinesVolume != 0m)
			{
				tempPercent = tempVolume / totalLinesVolume;
			}
			break;
		}
		return tempPercent;
	}

	public string AddReceiptToLandedCost(M1BindingSource bindingSource, List<DataRow> selectedRows)
	{
		_ = string.Empty;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		M1Database database = bindingSource.Database;
		if (currentAsDataRow != null)
		{
			string text = currentAsDataRow.Field<string>("rmcLandedCostID");
			if (!string.IsNullOrWhiteSpace(text))
			{
				foreach (DataRow selectedRow in selectedRows)
				{
					string text2 = selectedRow.Field<string>("rmpReceiptID");
					if (string.IsNullOrWhiteSpace(text2))
					{
						continue;
					}
					SqlCommand sqlCommand = database.NewSqlCommand("select pmpPurchaseOrderID,pmpLandedCost,rmlPurchaseOrderID,rmlPurchaseOrderLineID,isnull(imtPartTransactionID,0) as imtPartTransactionID,rmpPostedToGL,rmpReversed,rmpReversalEntry From ReceiptLines Left Outer Join PurchaseOrders on rmlPurchaseOrderID=pmpPurchaseOrderID Left Outer Join PartTransactions on imtReceiptID=rmlReceiptID And imtReceiptLineID=rmlReceiptLineID Left Join Receipts On rmlReceiptID=rmpReceiptID Where rmlReceiptID = @ReceiptID");
					sqlCommand.Parameters.Add(new SqlParameter("@ReceiptID", SqlDbType.NVarChar)).Value = text2;
					DataTable dataTable = database.GetDataTable(sqlCommand);
					if (dataTable.Rows.Count != 0)
					{
						StringBuilder stringBuilder = new StringBuilder();
						foreach (DataRow row in dataTable.Rows)
						{
							if (!row.Field<bool>("pmpLandedCost"))
							{
								stringBuilder.AppendLine(row.Field<string>("pmpPurchaseOrderID"));
							}
							if (row["imtPartTransactionID"] != DBNull.Value && row.Field<int>("imtPartTransactionID") != 0)
							{
								stringBuilder.AppendLine(row.Field<string>("pmpPurchaseOrderID"));
							}
							if (row.Field<bool>("rmpPostedToGL") || row.Field<bool>("rmpReversed") || row.Field<bool>("rmpReversalEntry"))
							{
								stringBuilder.AppendLine(row.Field<string>("pmpPurchaseOrderID"));
							}
							database.Props("FN").Field<bool>("xafGLCreateStockJournals");
							if (row["pmpPurchaseOrderID"] != DBNull.Value)
							{
								SqlCommand sqlCommand2 = database.NewSqlCommand("Select top 1 rmiLandedCostID From PurchaseOrders Left Join LandedCostChargeDetails On pmpPurchaseOrderID = rmiPurchaseOrderID Where pmpPurchaseOrderID = @PurchaseOrderID");
								sqlCommand2.Parameters.Add(new SqlParameter("@PurchaseOrderID", SqlDbType.NVarChar)).Value = row.Field<string>("pmpPurchaseOrderID");
								DataTable dataTable2 = database.GetDataTable(sqlCommand2);
								if (dataTable2.Rows.Count != 0 && (string.IsNullOrEmpty(dataTable2.Rows[0].Field<string>("rmiLandedCostID")) ? "" : dataTable2.Rows[0].Field<string>("rmiLandedCostID")) != text)
								{
									return $"This receipt cannot be added as all receipt lines must be linked to a PO in the Purchase Order Transit Info grid for this landed cost record.";
								}
							}
						}
						if (stringBuilder.Length != 0)
						{
							return $"Receipt ID {text2} was not added because the following Purchase Orders are not marked as Landed Costs, the Receipt has transactions against it, or the Receipt has been posted/reversed:\n {stringBuilder.ToString()}";
						}
						sqlCommand = database.NewSqlCommand("Update Receipts Set rmpLandedCostID = @LandedCostID Where rmpReceiptID = @ReceiptID");
						sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = text;
						sqlCommand.Parameters.Add(new SqlParameter("@ReceiptID", SqlDbType.NVarChar)).Value = text2;
						database.ExecuteScalar(sqlCommand);
						database.OnTableChanged(new TableChangedEventArgs("Receipts", null, null, null));
						continue;
					}
					return "The entered Receipt ID does not exist";
				}
				decimal totalLineCost = (decimal)bindingSource.Database.ExecuteScalar("Select IsNull(Sum(pmlPurchaseQuantity * (pmlPurchaseUnitCostBase + round(pmlSetupChargeBase / pmlPurchaseQuantity, 5))), 0) as POLineCost from PurchaseOrderLines where pmlLandedCostID = " + text.ToSql());
				decimal totalCharges = (decimal)bindingSource.Database.ExecuteScalar("Select IsNull(Sum(rmhTotalCost),0) As ChargeTotal From LandedCostCharges Where rmhLandedCostID = " + text.ToSql());
				decimal totalReceiptCost = (decimal)bindingSource.Database.ExecuteScalar("Select isnull(sum(rmlExtendedCostBase),0) as TotalReceiptsCost from ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID where rmpLandedCostID = " + text.ToSql());
				RefreshLandedCostTotals(bindingSource.Database, null, text, totalCharges, totalLineCost, totalReceiptCost);
				bindingSource.OnDataChanged(new DataChangedEventArgs(DataChangedFlag.CurrentRow));
			}
		}
		return string.Empty;
	}

	public void RefreshReceiptLineDetails(M1BindingSource bindingsource, SqlTransaction transaction)
	{
		if (bindingsource == null || bindingsource.CurrentAsDataRow == null)
		{
			return;
		}
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		string value = bindingsource.CurrentAsDataRow.Field<string>("rmcLandedCostID");
		M1Database database = bindingsource.Database;
		M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource("LandedCostCharges");
		if (childBindingSource == null)
		{
			return;
		}
		decimal num3 = default(decimal);
		decimal num4 = default(decimal);
		decimal num5 = default(decimal);
		decimal num6 = default(decimal);
		decimal num7 = default(decimal);
		decimal num8 = default(decimal);
		SqlCommand sqlCommand = database.NewSqlCommand("select rmlReceiptID,rmlReceiptLineID,rmlPartID,rmlPartRevisionID,rmlInventoryUnitCost,rmlInventoryQuantityReceived,rmlJobOprQuantityReceived,rmlJobMatQuantityReceived,rmlQuantityToInspect,rmlDutyUnitCost,rmlFreightUnitCost,rmlMiscUnitCost,IsNull(imrWeight, 0) As imrWeight, IsNull(imrVolume, 0) As imrVolume from Receipts inner join ReceiptLines on rmpReceiptID=rmlReceiptID Left Join PartRevisions on rmlPartID = imrPartID And rmlPartRevisionID = imrPartRevisionID where rmpLandedCostID = @LandedCostID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = value;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		sqlCommand = database.NewSqlCommand("UPDATE ReceiptLines SET rmlDutyUnitCost = 0, rmlFreightUnitCost = 0, rmlMiscUnitCost = 0 FROM ReceiptLines Inner Join Receipts on rmpReceiptID=rmlReceiptID WHERE rmpLandedCostID = @LandedCost");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCost", SqlDbType.NVarChar)).Value = value;
		database.ExecuteCommand(sqlCommand, transaction);
		num2 = dataTable.AsEnumerable().Sum((DataRow x) => (x.Field<decimal>("rmlInventoryQuantityReceived") + x.Field<decimal>("rmlJobOprQuantityReceived") + x.Field<decimal>("rmlJobMatQuantityReceived") + x.Field<decimal>("rmlQuantityToInspect")) * x.Field<decimal>("rmlInventoryUnitCost"));
		num3 = dataTable.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("rmlInventoryQuantityReceived") + x.Field<decimal>("rmlJobOprQuantityReceived") + x.Field<decimal>("rmlJobMatQuantityReceived") + x.Field<decimal>("rmlQuantityToInspect"));
		num4 = dataTable.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("imrWeight") * (x.Field<decimal>("rmlInventoryQuantityReceived") + x.Field<decimal>("rmlJobOprQuantityReceived") + x.Field<decimal>("rmlJobMatQuantityReceived") + x.Field<decimal>("rmlQuantityToInspect")));
		num5 = dataTable.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("imrVolume") * (x.Field<decimal>("rmlInventoryQuantityReceived") + x.Field<decimal>("rmlJobOprQuantityReceived") + x.Field<decimal>("rmlJobMatQuantityReceived") + x.Field<decimal>("rmlQuantityToInspect")));
		foreach (DataRow row in childBindingSource.GetDataView(bindingsource.CurrentAsDataRow).ToTable().Rows)
		{
			byte chargeType = row.Field<byte>("rmhLandedCostMethod");
			byte landedCostChargeCategoryType = getLandedCostChargeCategoryType(database, transaction, row.Field<string>("rmhLandedCostCategoryID"));
			decimal num9 = row.Field<decimal>("rmhTotalCost");
			num += num9;
			foreach (DataRow row2 in dataTable.Rows)
			{
				decimal tempPercent = default(decimal);
				decimal num10 = row2.Field<decimal>("rmlInventoryQuantityReceived") + row2.Field<decimal>("rmlJobOprQuantityReceived") + row2.Field<decimal>("rmlJobMatQuantityReceived") + row2.Field<decimal>("rmlQuantityToInspect");
				decimal tempTotal = row2.Field<decimal>("rmlInventoryUnitCost") * num10;
				decimal tempWeight = row2.Field<decimal>("imrWeight") * num10;
				decimal tempVolume = row2.Field<decimal>("imrVolume") * num10;
				num6 = default(decimal);
				num7 = default(decimal);
				num8 = default(decimal);
				tempPercent = getLinePercent(num2, num3, num4, num5, chargeType, tempPercent, num10, tempTotal, tempWeight, tempVolume);
				switch (landedCostChargeCategoryType)
				{
				case 1:
					num6 = ((num10 <= 0m) ? 0m : Math.Round(tempPercent * num9 / num10, 5));
					break;
				case 2:
					num7 = ((num10 <= 0m) ? 0m : Math.Round(tempPercent * num9 / num10, 5));
					break;
				case 3:
					num8 = ((num10 <= 0m) ? 0m : Math.Round(tempPercent * num9 / num10, 5));
					break;
				}
				sqlCommand = database.NewSqlCommand("UPDATE ReceiptLines SET rmlDutyUnitCost = rmlDutyUnitCost + @DutyCost, rmlFreightUnitCost = rmlFreightUnitCost + @FreightCost, rmlMiscUnitCost = rmlMiscUnitCost + @MiscCost FROM ReceiptLines Inner Join Receipts on rmpReceiptID=rmlReceiptID WHERE rmlReceiptID = @ReceiptID AND rmlReceiptLineID = @ReceiptLineID");
				sqlCommand.Parameters.Add(new SqlParameter("@ReceiptID", SqlDbType.NVarChar)).Value = row2["rmlReceiptID"];
				sqlCommand.Parameters.Add(new SqlParameter("@ReceiptLineID", SqlDbType.Int)).Value = row2["rmlReceiptLineID"];
				sqlCommand.Parameters.Add(new SqlParameter("@DutyCost", SqlDbType.Decimal)).Value = num6;
				sqlCommand.Parameters.Add(new SqlParameter("@FreightCost", SqlDbType.Decimal)).Value = num7;
				sqlCommand.Parameters.Add(new SqlParameter("@MiscCost", SqlDbType.Decimal)).Value = num8;
				database.ExecuteCommand(sqlCommand, transaction);
			}
		}
	}

	private void RefreshLandedCostTotals(M1Database database, SqlTransaction transaction, string landedCostID, decimal totalCharges, decimal totalLineCost, decimal totalReceiptCost)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE LANDEDCOSTS SET rmcLandedCostPurchasesTotal=@totalLineCost, rmcLandedCostChargesTotal=@totalCharges,rmcLandedCostTotal=@landedCostTotal, rmcLandedCostReceiptsTotal=@totalReceiptCost WHERE rmcLandedCostID=@LandedCostID");
		sqlCommand.Parameters.AddWithValue("@totalLineCost", totalLineCost);
		sqlCommand.Parameters.AddWithValue("@totalCharges", totalCharges);
		sqlCommand.Parameters.AddWithValue("@totalReceiptCost", totalReceiptCost);
		sqlCommand.Parameters.AddWithValue("@landedCostTotal", totalCharges + totalReceiptCost);
		sqlCommand.Parameters.AddWithValue("@LandedCostID", landedCostID);
		database.ExecuteCommand(sqlCommand, transaction);
	}

	private byte getLandedCostChargeCategoryType(M1Database database, SqlTransaction transaction, string categoryID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select rmaCategoryType From LandedCostCategories Where rmaLandedCostCategoryID = @CategoryID");
		sqlCommand.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.NVarChar)).Value = categoryID;
		return (byte)database.ExecuteScalar(sqlCommand, transaction);
	}

	public string CreateLandedCostTransitJournals(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		M1Database database = bindingSource.Database;
		DateTime now = DateTime.Now;
		short year = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Year;
		byte period = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Period;
		CostOfGoodSoldDefinition.JournalSource headerSource = (CostOfGoodSoldDefinition.JournalSource)new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostPOInTransit).getHeaderSource();
		CostOfGoodSoldDefinition.DetailSource detailSource = new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostPOInTransit).getDetailSource();
		if (bindingSource != null && bindingSource.CurrentAsDataRow != null)
		{
			string value = bindingSource.CurrentAsDataRow.Field<string>("rmcLandedCostID");
			string plantID = bindingSource.CurrentAsDataRow.Field<string>("rmcPlantID");
			SqlCommand sqlCommand = database.NewSqlCommand("select pmlPurchaseOrderID, pmlPurchaseOrderLineID, pmlUniqueID, pmlPartID, pmlJobID, pmlJobAssemblyID, pmlJobMaterialID, pmlJobOperationID, pmlInventoryQuantity, pmlPurchaseQuantity, pmlPurchaseUnitCostBase, pmlInTransitJournalsCreated, pmlSetupChargeBase, IsNull(imrWeight, 0) As imrWeight, IsNull(imrVolume, 0) As imrVolume from PurchaseOrderLines Left Join PartRevisions on pmlPartID = imrPartID And pmlPartRevisionID = imrPartRevisionID Where pmlLandedCostID = @LandedCostID");
			sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count == 0)
			{
				return "There are no linked purchase order lines.";
			}
			IList<CostOfGoodSoldDefinition.Journal> list = new List<CostOfGoodSoldDefinition.Journal>();
			IList<CostOfGoodSoldDefinition.JournalLine> list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
			int num = 1;
			CostOfGoodSoldDefinition.Journal journal = new COGS().BuildJournalObject(database, transaction, now, year, period, headerSource, detailSource, "Landed Cost PO In-Transit Journal");
			foreach (DataRow row in dataTable.Rows)
			{
				decimal num2 = row.Field<decimal>("pmlPurchaseQuantity");
				decimal num3 = (row.Field<decimal>("pmlPurchaseUnitCostBase") + Math.Round(row.Field<decimal>("pmlSetupChargeBase") / num2, 5)) * num2;
				string partID = row.Field<string>("pmlPartID");
				Guid sourceUniqueId = row.Field<Guid>("pmlUniqueID");
				COGS.JobInfo jobInfo = new COGS.JobInfo();
				jobInfo.JobID = row.Field<string>("pmlJobID");
				jobInfo.JobAssemblyID = row.Field<int>("pmlJobAssemblyID");
				jobInfo.JobMaterialID = row.Field<int>("pmlJobMaterialID");
				jobInfo.JobOperationID = row.Field<int>("pmlJobOperationID");
				COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, partID, plantID, string.Empty, string.Empty);
				if (cOGSAccounts == null)
				{
					return "No COGS accounts found.";
				}
				CostOfGoodSoldDefinition.JournalLine item = new COGS().BuildJournalLineObject(database, transaction, journal, num, num3, cOGSAccounts.StockInTransitGLAccountID, sourceUniqueId, "Landed Cost PO In-Transit Journal", CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostPOInTransit, jobInfo);
				list2.Add(item);
				num++;
				item = new COGS().BuildJournalLineObject(database, transaction, journal, num, -num3, cOGSAccounts.AccruedCreditorsGLAccountID, sourceUniqueId, "Landed Cost PO In-Transit Journal", CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostPOInTransit, jobInfo);
				list2.Add(item);
				num++;
				if (!row.Field<bool>("pmlInTransitJournalsCreated") && !string.IsNullOrWhiteSpace(row.Field<string>("pmlPurchaseOrderID")))
				{
					sqlCommand = bindingSource.Database.NewSqlCommand("Update PurchaseOrderLines Set pmlInTransitJournalsCreated = 1 Where pmlLandedCostID <> '' And pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @LineID");
					sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = row["pmlPurchaseOrderID"];
					sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = row["pmlPurchaseOrderLineID"];
					bindingSource.Database.ExecuteScalar(sqlCommand, transaction);
				}
			}
			if (list2.Count > 0)
			{
				journal.JournalLines = list2.ToList();
				journal.TotalDebits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.DebitAmount);
				journal.TotalCredits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.CreditAmount);
				list.Add(journal);
				list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
			}
			if (list.Count > 0)
			{
				foreach (CostOfGoodSoldDefinition.Journal item2 in list)
				{
					new COGS().AddJournal(database, transaction, item2, "PurchaseOrderLines", bindingSource.CurrentAsDataRow, bindingSource.PrimaryTable.FieldPrefix);
				}
			}
		}
		return string.Empty;
	}

	public string CreateLandedCostChargesJournals(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		M1Database database = bindingSource.Database;
		DateTime now = DateTime.Now;
		short year = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Year;
		byte period = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Period;
		CostOfGoodSoldDefinition.JournalSource headerSource = (CostOfGoodSoldDefinition.JournalSource)new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostCharges).getHeaderSource();
		CostOfGoodSoldDefinition.DetailSource detailSource = new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostCharges).getDetailSource();
		string text = validateCharges(bindingSource);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return $"There is no total cost value for charge {text}";
		}
		if (bindingSource != null && bindingSource.CurrentAsDataRow != null)
		{
			DataRow parentDataRow = bindingSource.PrimaryTable.GetParentDataRow(bindingSource.CurrentAsDataRow);
			if (parentDataRow != null)
			{
				if (!parentDataRow.Field<bool>("rmcPOInTransitJournalsCreated"))
				{
					return "Purchase order in-transit journals have not been created.";
				}
				string value = parentDataRow.Field<string>("rmcLandedCostID");
				string plantID = parentDataRow.Field<string>("rmcPlantID");
				IList<CostOfGoodSoldDefinition.Journal> list = new List<CostOfGoodSoldDefinition.Journal>();
				IList<CostOfGoodSoldDefinition.JournalLine> list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
				foreach (DataRow row in bindingSource.GetDataView().ToTable().Rows)
				{
					int num = 1;
					if (row.Field<bool>("rmhInTransitJournalsCreated"))
					{
						continue;
					}
					CostOfGoodSoldDefinition.Journal journal = new COGS().BuildJournalObject(database, transaction, now, year, period, headerSource, detailSource, "Landed Cost Charges Journal");
					SqlCommand sqlCommand = database.NewSqlCommand("Select rmiUniqueID, rmiEstTotalCost, rmiPurchaseOrderID, rmiPurchaseOrderLineID from LandedCostChargeDetails Where rmiLandedCostID = @LandedCostID and rmiLandedCostChargeID = @ChargeID");
					sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = row["rmhLandedCostID"];
					sqlCommand.Parameters.Add(new SqlParameter("@ChargeID", SqlDbType.Int)).Value = row["rmhLandedCostChargeID"];
					foreach (DataRow row2 in database.GetDataTable(sqlCommand, transaction).Rows)
					{
						Guid sourceUniqueId = row2.Field<Guid>("rmiUniqueID");
						decimal num2 = row2.Field<decimal>("rmiEstTotalCost");
						SqlCommand sqlCommand2 = bindingSource.Database.NewSqlCommand("Select pmlPartID, pmlJobID, pmlJobAssemblyID, pmlJobMaterialID, pmlJobOperationID From PurchaseOrderLines Where pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @LineID");
						sqlCommand2.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = row2["rmiPurchaseOrderID"];
						sqlCommand2.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = row2["rmiPurchaseOrderLineID"];
						DataTable dataTable = bindingSource.Database.GetDataTable(sqlCommand2, transaction);
						string partID = string.Empty;
						COGS.JobInfo jobInfo = new COGS.JobInfo();
						if (dataTable.Rows.Count != 0)
						{
							partID = dataTable.Rows[0].Field<string>("pmlPartID");
							jobInfo.JobID = dataTable.Rows[0].Field<string>("pmlJobID");
							jobInfo.JobAssemblyID = dataTable.Rows[0].Field<int>("pmlJobAssemblyID");
							jobInfo.JobMaterialID = dataTable.Rows[0].Field<int>("pmlJobMaterialID");
							jobInfo.JobOperationID = dataTable.Rows[0].Field<int>("pmlJobOperationID");
						}
						COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, partID, plantID, string.Empty, string.Empty);
						if (cOGSAccounts == null)
						{
							return "No COGS accounts found.";
						}
						CostOfGoodSoldDefinition.JournalLine item = new COGS().BuildJournalLineObject(database, transaction, journal, num, num2, cOGSAccounts.StockInTransitGLAccountID, sourceUniqueId, "Landed Cost Charges Journal", CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostCharges, jobInfo);
						list2.Add(item);
						num++;
						item = new COGS().BuildJournalLineObject(database, transaction, journal, num, -num2, cOGSAccounts.AccruedCreditorsGLAccountID, sourceUniqueId, "Landed Cost Charges Journal", CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostCharges, jobInfo);
						list2.Add(item);
						num++;
					}
					if (list2.Count > 0)
					{
						journal.JournalLines = list2.ToList();
						journal.TotalDebits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.DebitAmount);
						journal.TotalCredits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.CreditAmount);
						list.Add(journal);
						list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
					}
					SqlCommand sqlCommand3 = bindingSource.Database.NewSqlCommand("Update LandedCostCharges Set rmhInTransitJournalsCreated = 1 Where rmhLandedCostID = @LandedCostID And rmhLandedCostChargeID = @LandedCostChargeID");
					sqlCommand3.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = row["rmhLandedCostID"];
					sqlCommand3.Parameters.Add(new SqlParameter("@LandedCostChargeID", SqlDbType.Int)).Value = row["rmhLandedCostChargeID"];
					bindingSource.Database.ExecuteScalar(sqlCommand3, transaction);
				}
				if (list.Count > 0)
				{
					foreach (CostOfGoodSoldDefinition.Journal item2 in list)
					{
						new COGS().AddJournal(database, transaction, item2, "LandedCostChargeDetails", bindingSource.CurrentAsDataRow, bindingSource.PrimaryTable.FieldPrefix);
					}
				}
				SqlCommand sqlCommand4 = bindingSource.Database.NewSqlCommand("Update LandedCosts Set rmcChargesJournalsCreated = 1 Where rmcLandedCostID = @LandedCostID");
				sqlCommand4.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = value;
				bindingSource.Database.ExecuteScalar(sqlCommand4, transaction);
			}
		}
		return string.Empty;
	}

	private string validateCharges(M1BindingSource bindingSource)
	{
		string text = string.Empty;
		foreach (DataRow row in bindingSource.GetDataView().ToTable().Rows)
		{
			if (row.Field<decimal>("rmhEstTotalCost") == 0m || row.Field<decimal>("rmhEstTotalCostForeign") == 0m)
			{
				text = text + "," + row["rmhLandedCostChargeID"];
			}
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			text = text.Substring(1);
		}
		return text;
	}

	public string PostLandedCost(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		string text = string.Empty;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (currentAsDataRow != null)
		{
			string text2 = currentAsDataRow.Field<string>("rmcLandedCostID");
			string plantID = currentAsDataRow.Field<string>("rmcPlantID");
			M1Database database = bindingSource.Database;
			DateTime now = DateTime.Now;
			short year = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Year;
			byte period = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Period;
			CostOfGoodSoldDefinition.JournalSource headerSource = (CostOfGoodSoldDefinition.JournalSource)new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCost).getHeaderSource();
			CostOfGoodSoldDefinition.DetailSource detailSource = new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCost).getDetailSource();
			if (currentAsDataRow.Field<bool>("rmcPostedToGL"))
			{
				return $"Landed cost {text2} has already been posted.";
			}
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("LandedCostCharges");
			if (childBindingSource != null)
			{
				string text3 = string.Empty;
				foreach (DataRow row in childBindingSource.GetDataView().ToTable().Rows)
				{
					if ((row.Field<decimal>("rmhEstTotalCost") != 0m && row.Field<decimal>("rmhTotalCost") == 0m) || (row.Field<decimal>("rmhEstTotalCostForeign") != 0m && row.Field<decimal>("rmhTotalCostForeign") == 0m))
					{
						text3 = text3 + "," + row["rmhLandedCostChargeID"];
					}
				}
				if (!string.IsNullOrWhiteSpace(text3))
				{
					text3 = text3.Substring(1);
				}
				SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("Select IsNull(Count(*),0) As Count From Receipts Where rmpLandedCostID = @LandedCostID");
				sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = currentAsDataRow["rmcLandedCostID"];
				int num = Convert.ToInt32(bindingSource.Database.ExecuteScalar(sqlCommand, transaction));
				if (num <= 0)
				{
					text = "Please link Receipts to Landed Cost Entry before proceeding.";
				}
				if (num > 0 && !string.IsNullOrWhiteSpace(text3))
				{
					text = $"Please enter in Total Cost Value for Charge {text3} before continue to posting the current Landed Cost Entry.";
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
				if (!database.Props("FinancialProperties").Field<bool>("xafGLCreateStockJournals"))
				{
					sqlCommand = bindingSource.Database.NewSqlCommand("Update Receipts Set rmpLandedCostPosted = 1 Where rmpLandedCost <> 0 and rmpLandedCostID = @LandedCostID");
					sqlCommand.Parameters.AddWithValue("@LandedCostID", text2);
					bindingSource.Database.ExecuteScalar(sqlCommand, transaction);
					database.OnTableChanged(new TableChangedEventArgs("Receipts", null, null, null));
					database.OnTableChanged(new TableChangedEventArgs("ReceiptLines", null, null, null));
					database.OnTableChanged(new TableChangedEventArgs("LandedCostCharges", null, null, null));
					database.OnTableChanged(new TableChangedEventArgs("LandedCostChargeDetails", null, null, null));
					return string.Empty;
				}
				M1BindingSource childBindingSource2 = bindingSource.PrimaryTable.GetChildBindingSource("LandedCostChargeDetails");
				IList<CostOfGoodSoldDefinition.Journal> list = new List<CostOfGoodSoldDefinition.Journal>();
				IList<CostOfGoodSoldDefinition.JournalLine> list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
				foreach (DataRow row2 in bindingSource.GetDataView().ToTable().Rows)
				{
					int num2 = 1;
					CostOfGoodSoldDefinition.Journal journal = new COGS().BuildJournalObject(database, transaction, now, year, period, headerSource, detailSource, "Landed Cost Journal");
					foreach (DataRow row3 in childBindingSource2.GetDataView(row2).ToTable().Rows)
					{
						Guid sourceUniqueId = row3.Field<Guid>("rmiUniqueID");
						decimal num3 = row3.Field<decimal>("rmiEstTotalCost");
						decimal num4 = row3.Field<decimal>("rmiTotalCost") - num3;
						sqlCommand = bindingSource.Database.NewSqlCommand("Select pmlPartID, pmlJobID, pmlJobAssemblyID, pmlJobMaterialID, pmlJobOperationID From PurchaseOrderLines Where pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @LineID");
						sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = row3["rmiPurchaseOrderID"];
						sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = row3["rmiPurchaseOrderLineID"];
						DataTable dataTable = bindingSource.Database.GetDataTable(sqlCommand, transaction);
						string partID = string.Empty;
						COGS.JobInfo jobInfo = new COGS.JobInfo();
						if (dataTable.Rows.Count != 0)
						{
							partID = dataTable.Rows[0].Field<string>("pmlPartID");
							jobInfo.JobID = dataTable.Rows[0].Field<string>("pmlJobID");
							jobInfo.JobAssemblyID = dataTable.Rows[0].Field<int>("pmlJobAssemblyID");
							jobInfo.JobMaterialID = dataTable.Rows[0].Field<int>("pmlJobMaterialID");
							jobInfo.JobOperationID = dataTable.Rows[0].Field<int>("pmlJobOperationID");
						}
						COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, partID, plantID, string.Empty, string.Empty);
						if (cOGSAccounts == null)
						{
							return "No COGS accounts found.";
						}
						CostOfGoodSoldDefinition.JournalLine item = new COGS().BuildJournalLineObject(database, transaction, journal, num2, num4, cOGSAccounts.PurchaseVarianceGLAccountID, sourceUniqueId, "Landed Cost Journal", CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostCharges, jobInfo);
						list2.Add(item);
						num2++;
						item = new COGS().BuildJournalLineObject(database, transaction, journal, num2, -num4, cOGSAccounts.AccruedCreditorsGLAccountID, sourceUniqueId, "Landed Cost Journal", CostOfGoodSoldDefinition.JournalLineTransactionType.LandedCostCharges, jobInfo);
						list2.Add(item);
						num2++;
					}
					if (list2.Count > 0)
					{
						journal.JournalLines = list2.ToList();
						journal.TotalDebits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.DebitAmount);
						journal.TotalCredits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.CreditAmount);
						list.Add(journal);
						list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
					}
				}
				if (list.Count > 0)
				{
					foreach (CostOfGoodSoldDefinition.Journal item2 in list)
					{
						new COGS().AddJournal(database, transaction, item2, "LandedCostChargeDetails", bindingSource.CurrentAsDataRow, bindingSource.PrimaryTable.FieldPrefix);
					}
				}
				sqlCommand = bindingSource.Database.NewSqlCommand("Update Receipts Set rmpLandedCostPosted = 1 Where rmpLandedCost <> 0 and rmpLandedCostID = @LandedCostID");
				sqlCommand.Parameters.AddWithValue("@LandedCostID", text2);
				bindingSource.Database.ExecuteScalar(sqlCommand, transaction);
				database.OnTableChanged(new TableChangedEventArgs("Receipts", null, null, null));
				database.OnTableChanged(new TableChangedEventArgs("ReceiptLines", null, null, null));
				database.OnTableChanged(new TableChangedEventArgs("LandedCostCharges", null, null, null));
				database.OnTableChanged(new TableChangedEventArgs("LandedCostChargeDetails", null, null, null));
			}
			return string.Empty;
		}
		return "M1 is unable to post this landed cost because it does not exist in the landed costs table.";
	}

	public string AddLandedCostChargeToInvoice(M1Database database, SqlTransaction sqlTransaction, string landedCostID, int chargeID, string invoice, object invoiceDate, int year, int period, bool calcTaxOnFreight, ref string errorMessage, int invoiceType, bool suppressAlerts)
	{
		errorMessage = string.Empty;
		string empty = string.Empty;
		bool flag = false;
		SqlTransaction sqlTransaction2 = null;
		try
		{
			sqlTransaction2 = ((sqlTransaction != null) ? sqlTransaction : database.BeginTransaction());
			DataRow landedCostChargesRow = getLandedCostChargesRow(database, sqlTransaction2, landedCostID, chargeID);
			if (invoiceType == 0)
			{
				invoiceType = 1;
			}
			if (!string.IsNullOrEmpty(landedCostChargesRow.Field<string>("rmhReverseLandedCostID")))
			{
				invoiceType = 2;
			}
			DataRow dataRow = getApInvoiceRow(database, sqlTransaction2, invoice);
			errorMessage = validateData(landedCostChargesRow, dataRow, landedCostID, invoice, invoiceType, chargeID);
			if (!string.IsNullOrEmpty(errorMessage))
			{
				return string.Empty;
			}
			SqlDataAdapter adapter = null;
			DataTable dataTable = database.GetDataTable("Select * From APInvoices Where 0 = 1", fillSchema: false, out adapter, sqlTransaction2);
			if (string.IsNullOrEmpty(invoice))
			{
				dataRow = createAPInvoiceNewRow(database, sqlTransaction2, dataTable, landedCostChargesRow, invoiceType, ref errorMessage);
				if (dataRow == null)
				{
					return string.Empty;
				}
				dataTable.Rows.Add(dataRow);
				flag = true;
			}
			dataRow["appPlantID"] = landedCostChargesRow.Field<string>("rmcPlantID");
			errorMessage = validateData(landedCostChargesRow, dataRow, landedCostID, invoice, dataRow.Field<byte>("appInvoiceType"), chargeID);
			if (!string.IsNullOrEmpty(errorMessage))
			{
				return string.Empty;
			}
			int aPInvoiceMaxLine = getAPInvoiceMaxLine(database, sqlTransaction2, dataRow.Field<string>("appAPInvoiceID"));
			if (aPInvoiceMaxLine == 0)
			{
				flag = true;
			}
			string supplierID = landedCostChargesRow.Field<string>("rmhSupplierOrganizationID");
			DataRow organizationsRow = getOrganizationsRow(database, sqlTransaction2, supplierID);
			M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
			if (flag)
			{
				setAPInvoicesDefaultValues(database, sqlTransaction2, m1DataDictionary, dataRow, landedCostChargesRow, organizationsRow, supplierID, invoiceDate, year, period);
			}
			else if (!landedCostChargesRow.Field<string>("rmhSupplierLocationID").Equals(dataRow.Field<string>("appAPInvoiceLocationID")))
			{
				errorMessage = $"Landed Cost {landedCostID} contains a different invoice location id than what is on invoice";
				return string.Empty;
			}
			SqlDataAdapter adapter2 = null;
			DataTable dataTable2 = database.GetDataTable("select * from APInvoiceLines where 0=1", fillSchema: false, out adapter2, sqlTransaction2);
			DataRow dataRow2 = createAPInvoiceLinesNewRow(database, sqlTransaction2, dataTable2, dataRow, landedCostChargesRow, aPInvoiceMaxLine);
			SqlDataAdapter adapter3 = null;
			DataTable dataTable3 = database.GetDataTable("select * from APInvoiceExpenseAccounts where 0=1", fillSchema: false, out adapter3, sqlTransaction2);
			createAPInvoiceExpenseAccounts(database, sqlTransaction2, dataTable3, landedCostChargesRow, organizationsRow, dataRow, dataRow2);
			m1DataDictionary.FindMatchingFields("LandedCostCharges", "APInvoiceLines").CopyData(landedCostChargesRow, dataRow2);
			bool fixForeign = fixForeignNeeded(database, sqlTransaction2, dataRow);
			if (!string.IsNullOrEmpty(invoice))
			{
				database.UpdateData(new DataRow[1] { dataRow }, adapter, sqlTransaction2);
			}
			database.UpdateData(dataTable, adapter, sqlTransaction2);
			database.UpdateData(dataTable2, adapter2, sqlTransaction2);
			database.UpdateData(dataTable3, adapter3, sqlTransaction2);
			database.CommitTransaction(sqlTransaction2);
			AppAxLegacy appAxLegacy = new AppAxLegacy(database);
			string text = dataRow.Field<string>("appAPInvoiceID");
			appAxLegacy.RefreshCurrencyForDetails("APInvoices", text, fixForeign);
			UpdateLandedCostCharges(database, dataTable2);
			return text;
		}
		catch (Exception ex)
		{
			database.RollbackTransaction(sqlTransaction2);
			errorMessage = ex.Message;
			return string.Empty;
		}
	}

	public string UpdateVarianceExpenseAccounts(M1Database database, SqlTransaction sqlTransaction, string landedCostID, int landedCostChargeID, string apInvoiceID, int apInvoiceLineID, decimal newExpenseAmount)
	{
		string empty = string.Empty;
		if (database.Props("FN").Field<bool>("xafGLCreateStockJournals"))
		{
			SqlTransaction sqlTransaction2 = null;
			try
			{
				sqlTransaction2 = ((sqlTransaction != null) ? sqlTransaction : database.BeginTransaction());
				string iD = database.User.ID;
				DateTime now = DateTime.Now;
				DataRow apInvoiceLinesRow = getApInvoiceLinesRow(database, sqlTransaction2, apInvoiceID, apInvoiceLineID);
				COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, sqlTransaction2, apInvoiceLinesRow.Field<string>("aplPartID"), apInvoiceLinesRow.Field<string>("appPlantID"), string.Empty, string.Empty);
				int num = database.Props("PN").Field<byte>("xapIMCostingMethod");
				SqlDataAdapter expenseAccountsAdapter = null;
				DataTable apInvoiceExpenseAccountsTable = getApInvoiceExpenseAccountsTable(database, sqlTransaction2, out expenseAccountsAdapter, apInvoiceID, apInvoiceLineID);
				decimal num2 = default(decimal);
				decimal num3 = default(decimal);
				DataRow dataRow = null;
				if (apInvoiceExpenseAccountsTable != null)
				{
					if (num == 3)
					{
						if (!string.IsNullOrEmpty(cOGSAccounts.PurchaseVarianceGLAccountID))
						{
							foreach (DataRow row in apInvoiceExpenseAccountsTable.Rows)
							{
								if (!row.Field<string>("apxExpenseGLAccountID").Equals(cOGSAccounts.PurchaseVarianceGLAccountID))
								{
									num2 += row.Field<decimal>("apxAmount");
								}
								else
								{
									dataRow = row;
								}
							}
							num3 = newExpenseAmount - num2;
							if (dataRow == null && num3 != 0m)
							{
								createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccountsTable, apInvoiceID, apInvoiceLineID, apInvoiceExpenseAccountsTable.Rows.Count + 1, iD, now, cOGSAccounts.PurchaseVarianceGLAccountID, Math.Round(num3, 2), 0m);
							}
							else if (dataRow.Field<decimal>("apxAmount") != num3 && num3 != 0m)
							{
								dataRow["apxAmount"] = num3;
							}
						}
					}
					else
					{
						foreach (DataRow row2 in apInvoiceExpenseAccountsTable.Rows)
						{
							if (row2.Field<string>("apxExpenseGLAccountID").Equals(cOGSAccounts.AccruedCreditorsGLAccountID))
							{
								num2 += row2.Field<decimal>("apxAmount");
							}
							else
							{
								row2["apxAmount"] = 0;
							}
						}
						num3 = newExpenseAmount - num2;
						if (num3 != 0m)
						{
							DataRow landedCostChargesRow = getLandedCostChargesRow(database, sqlTransaction2, landedCostID, landedCostChargeID);
							createExpenseAccountsForVariance(database, sqlTransaction2, apInvoiceExpenseAccountsTable, num3, landedCostID, landedCostChargeID, apInvoiceID, apInvoiceLineID, iD, now, landedCostChargesRow.Field<byte>("rmhLandedCostMethod"));
						}
					}
				}
				database.UpdateData(apInvoiceExpenseAccountsTable, expenseAccountsAdapter, sqlTransaction2);
				database.CommitTransaction(sqlTransaction2);
				if (num3 == 0m)
				{
					DataRow[] array = null;
					array = ((num != 3) ? apInvoiceExpenseAccountsTable.Select("apxAmount = 0") : new DataRow[1] { dataRow });
					removeAPInvoiceExpenseAccounts(database, sqlTransaction2, array);
				}
			}
			catch (Exception ex)
			{
				database.RollbackTransaction(sqlTransaction2);
				return ex.Message;
			}
		}
		return empty;
	}

	private void UpdateLandedCostCharges(M1Database database, DataTable apInvoicesLines)
	{
		foreach (DataRow row in apInvoicesLines.Rows)
		{
			string text = row.Field<string>("aplLandedCostID");
			if (!string.IsNullOrEmpty(text))
			{
				int num = row.Field<short>("aplLandedCostChargeID");
				string o = row.Field<string>("aplAPInvoiceID");
				int num2 = row.Field<short>("aplAPInvoiceLineID");
				string queryString = $"UPDATE LandedCostCharges SET rmhInvoicedComplete = 1, rmhAPInvoiceID = {M1Util.ConvertToSql(o)}, rmhAPInvoiceLineID = {M1Util.ConvertToSql(num2)} WHERE rmhLandedCostID = {M1Util.ConvertToSql(text)} And rmhLandedCostChargeID = {num}";
				database.ExecuteCommand(queryString);
			}
		}
	}

	private bool fixForeignNeeded(M1Database database, SqlTransaction transaction, DataRow apInvoiceRow)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select cmlCurrencyRateID From OrganizationLocations Where cmlOrganizationID = @OrganizationID And cmlLocationID = @LocationID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrganizationID", SqlDbType.NVarChar)).Value = apInvoiceRow.Field<string>("appSupplierOrganizationID");
		sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = apInvoiceRow.Field<string>("appAPInvoiceLocationID");
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			string text = apInvoiceRow.Field<string>("appCurrencyRateID");
			if (dataTable.Rows[0].Field<string>("cmlCurrencyRateID").Equals(text) && !string.IsNullOrEmpty(text) && !text.Equals(database.HomeCurrencyID))
			{
				return true;
			}
		}
		return false;
	}

	private string validateData(DataRow landedCostCharges, DataRow apInvoice, string landedCostID, string invoice, int invoiceType, int chargeID)
	{
		string result = string.Empty;
		if (landedCostCharges == null)
		{
			return $"Unable to find landed cost {landedCostID} in the Landed Cost charges table.";
		}
		if (!string.IsNullOrEmpty(invoice))
		{
			if (apInvoice == null)
			{
				return $"Unable to find invoice {invoice} in the AP Invoices table.";
			}
			if (invoiceType != apInvoice.Field<byte>("appInvoiceType"))
			{
				return $"M1 is unable to transfer landed cost {landedCostID} to invoice {invoice} because the desired invoice types do not match.";
			}
		}
		if (landedCostCharges.Field<bool>("rmhInvoicedComplete"))
		{
			return $"Charge {chargeID} on Landed Cost {landedCostID} has already been invoiced.";
		}
		if (landedCostCharges != null && apInvoice != null)
		{
			if (!apInvoice.Field<string>("appSupplierOrganizationID").Equals(landedCostCharges.Field<string>("rmhSupplierOrganizationID")))
			{
				result = string.Format("Landed Cost {0} is for supplier {1} , which is different than the supplier for invoice {2}", landedCostID, landedCostCharges.Field<string>("rmhSupplierOrganizationID"), invoice);
			}
			if (!apInvoice.Field<string>("appCurrencyRateID").Equals(landedCostCharges.Field<string>("rmhCurrencyRateID")))
			{
				result = string.Format("Landed Cost {0} has a currency rate of {1} , which is different than the currency rate for invoice {2}", landedCostID, landedCostCharges.Field<string>("rmhCurrencyRateID"), invoice);
			}
			if (apInvoice.Field<decimal>("appExchangeRate") != landedCostCharges.Field<decimal>("rmhExchangeRate"))
			{
				result = string.Format("Landed Cost {0} has an exchange rate of {1} , which is different than the exchange rate for invoice {2}", landedCostID, landedCostCharges.Field<decimal>("rmhExchangeRate"), invoice);
			}
		}
		return result;
	}

	private bool createAPInvoiceExpenseAccountsFromEATable(DataTable expenseAccountsSource, DataTable apInvoiceExpenseAccounts, DataRow apInvoiceLines, string expenseAccount, string userID)
	{
		decimal num = default(decimal);
		decimal num2 = apInvoiceLines.Field<decimal>("aplExtendedCostBase");
		string apInvoiceID = apInvoiceLines.Field<string>("aplAPInvoiceID");
		short apInvoiceLineID = apInvoiceLines.Field<short>("aplAPInvoiceLineID");
		if (expenseAccountsSource != null && expenseAccountsSource.Rows.Count != 0)
		{
			DataRow dataRow = null;
			foreach (DataRow row in expenseAccountsSource.Rows)
			{
				string text = row.Field<string>("xazExpenseGLAccountID");
				string empty = string.Empty;
				empty = ((!string.IsNullOrEmpty(text)) ? text : expenseAccount);
				decimal num3 = row.Field<decimal>("xazPercent");
				decimal num4 = Math.Round(num3 / 100m * num2, 2);
				dataRow = createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccounts, apInvoiceID, apInvoiceLineID, row.Field<short>("xazSequence"), userID, DateTime.Now, empty, num4, num3);
				num += num4;
			}
			if (num != apInvoiceLines.Field<decimal>("aplExtendedCostBase"))
			{
				dataRow["apxAmount"] = Math.Round(dataRow.Field<decimal>("apxAmount") + (num2 - num));
			}
			return true;
		}
		return false;
	}

	private DataRow createAPInvoiceNewRow(M1Database database, SqlTransaction transaction, DataTable apInvoices, DataRow landedCostCharges, int invoiceType, ref string errorMessage)
	{
		string value = (string)database.NextIDs.GetNextIDForTable("APInvoices");
		if (string.IsNullOrEmpty(value))
		{
			errorMessage = "Unable to get the next available invoice number.";
			return null;
		}
		DataRow dataRow = apInvoices.NewRow().BlankRow();
		dataRow["appAPInvoiceID"] = value;
		dataRow["appInvoiceType"] = invoiceType;
		dataRow["appSupplierOrganizationID"] = landedCostCharges.Field<string>("rmhSupplierOrganizationID");
		dataRow["appCreatedBy"] = database.User.ID;
		dataRow["appCreatedDate"] = DateTime.Now;
		dataRow["appCurrencyRateID"] = landedCostCharges.Field<string>("rmhCurrencyRateID");
		bool flag = landedCostCharges.Field<bool>("rmhCustomRate");
		dataRow["appCustomRate"] = flag;
		if (flag)
		{
			dataRow["appExchangeRate"] = landedCostCharges.Field<decimal>("rmhExchangeRate");
		}
		else
		{
			dataRow["appExchangeRate"] = database.GetExchangeRate(dataRow.Field<string>("appCurrencyRateID"), null);
		}
		dataRow["appOriginalExchangeRate"] = dataRow.Field<decimal>("appExchangeRate");
		return dataRow;
	}

	private int getAPInvoiceMaxLine(M1Database database, SqlTransaction transaction, string apInvoiceID)
	{
		int result = 0;
		SqlCommand sqlCommand = database.NewSqlCommand("Select Max(aplAPInvoiceLineID) as aplAPInvoiceLineID from APInvoiceLines where aplAPInvoiceID = @apAPInvoiceID");
		sqlCommand.Parameters.Add(new SqlParameter("@apAPInvoiceID", SqlDbType.NVarChar)).Value = apInvoiceID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0 && !dataTable.Rows[0].IsNull("aplAPInvoiceLineID"))
		{
			result = dataTable.Rows[0].Field<short>("aplAPInvoiceLineID");
		}
		return result;
	}

	private void setAPInvoicesDefaultValues(M1Database database, SqlTransaction transaction, M1DataDictionary dataDictionary, DataRow apInvoice, DataRow landedCostCharges, DataRow organizations, string supplierID, object invoiceDate, int year, int period)
	{
		new AP().SetAPInvoiceAccounts(database, transaction, apInvoice);
		apInvoice["appAPInvoiceLocationID"] = landedCostCharges.Field<string>("rmhSupplierLocationID");
		if (organizations != null)
		{
			apInvoice["appPaymentTermID"] = organizations.Field<string>("cmoSupplierPaymentTermID");
		}
		string locationID = landedCostCharges.Field<string>("rmhSupplierLocationID");
		DataRow organizationLocationRow = getOrganizationLocationRow(database, supplierID, locationID, transaction);
		if (organizationLocationRow != null)
		{
			apInvoice["appFreightTaxCodeID"] = organizationLocationRow.Field<string>("cmoSupplierTaxCodeID");
			apInvoice["appSecondFreightTaxCodeID"] = organizationLocationRow.Field<string>("cmoSupplierSecondTaxCodeID");
		}
		apInvoice["appAPInvoiceContactID"] = landedCostCharges.Field<string>("rmhSupplierContactID");
		if (invoiceDate == null)
		{
			apInvoice["appInvoiceDate"] = DateTime.Now;
		}
		else
		{
			apInvoice["appInvoiceDate"] = (DateTime)invoiceDate;
		}
		DateTime dateTime = apInvoice.Field<DateTime>("appInvoiceDate");
		if (year == 0 || period == 0)
		{
			YearAndPeriod yearAndPeriod = new Financial().GetYearAndPeriod(database, DateTime.Now, "AP", IgnoreClosed: true, transaction);
			apInvoice["appGLFiscalYearID"] = yearAndPeriod.Year;
			apInvoice["appGLFiscalYearPeriodID"] = yearAndPeriod.Period;
		}
		else
		{
			apInvoice["appGLFiscalYearID"] = year;
			apInvoice["appGLFiscalYearPeriodID"] = period;
		}
		dataDictionary.FindMatchingFields("LandedCostCharges", "APInvoices").CopyData(landedCostCharges, apInvoice);
		AppAxFinancial appAxFinancial = new AppAxFinancial(database);
		object dDueDate = DateTime.Now;
		object dDiscountDate = DateTime.Now;
		if (appAxFinancial.CalculateDiscountAndDueDate(dateTime, apInvoice.Field<string>("appPaymentTermID"), ref dDueDate, ref dDiscountDate))
		{
			apInvoice["appDueDate"] = dDueDate;
			apInvoice["appDiscountDueDate"] = dDiscountDate;
		}
	}

	private DataRow createAPInvoiceLinesNewRow(M1Database database, SqlTransaction transaction, DataTable apInvoiceLinesTable, DataRow apInvoice, DataRow landedCostCharges, int maxLine)
	{
		DataRow dataRow = apInvoiceLinesTable.NewRow().BlankRow();
		dataRow["aplAPInvoiceID"] = apInvoice.Field<string>("appAPInvoiceID");
		dataRow["aplCreatedBy"] = database.User.ID;
		dataRow["aplCreatedDate"] = DateTime.Now;
		maxLine++;
		dataRow["aplAPInvoiceLineID"] = maxLine;
		dataRow["aplLandedCostID"] = landedCostCharges.Field<string>("rmhLandedCostID");
		dataRow["aplLandedCostChargeID"] = landedCostCharges.Field<short>("rmhLandedCostChargeID");
		dataRow["aplPartID"] = landedCostCharges.Field<string>("rmhLandedCostCategoryID");
		dataRow["aplPartDescription"] = landedCostCharges.Field<string>("rmhDescription");
		int num = 1;
		dataRow["aplPurchaseQuantity"] = num;
		dataRow["aplReceivedQuantity"] = 1;
		dataRow["aplInvoicedComplete"] = true;
		decimal num2 = landedCostCharges.Field<decimal>("rmhTotalCost");
		dataRow["aplPurchaseUnitCostBase"] = num2;
		dataRow["aplExtendedCostBase"] = Math.Round((decimal)num * num2, 2);
		decimal num3 = landedCostCharges.Field<decimal>("rmhTotalCostForeign");
		dataRow["aplPurchaseUnitCostForeign"] = num3;
		dataRow["aplExtendedCostForeign"] = Math.Round((decimal)num * num3, 2);
		apInvoiceLinesTable.Rows.Add(dataRow);
		return dataRow;
	}

	private DataRow createAPInvoiceExpenseAccountsNewRow(DataTable apInvoiceExpenseAccountsTable, string apInvoiceID, int apInvoiceLineID, int apInvoiceExpenseAccountID, string createdBy, DateTime createdDate, string expenseGLAccountID, decimal amount, decimal percent)
	{
		DataRow dataRow = apInvoiceExpenseAccountsTable.NewRow().BlankRow();
		dataRow["apxAPInvoiceID"] = apInvoiceID;
		dataRow["apxAPInvoiceLineID"] = apInvoiceLineID;
		dataRow["apxAPInvoiceExpenseAccountID"] = apInvoiceExpenseAccountID;
		dataRow["apxCreatedBy"] = createdBy;
		dataRow["apxCreatedDate"] = createdDate;
		dataRow["apxExpenseGLAccountID"] = expenseGLAccountID;
		dataRow["apxAmount"] = amount;
		dataRow["apxPercent"] = percent;
		apInvoiceExpenseAccountsTable.Rows.Add(dataRow);
		return dataRow;
	}

	private decimal CalculatePercentValue(M1Database database, SqlTransaction transaction, DataRow poRow, int landedCostMethod, decimal totalDetails, decimal totalQuantity, decimal totalWeight, decimal totalVolume)
	{
		decimal result = default(decimal);
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = poRow.Field<decimal>("pmlPurchaseQuantity");
		decimal num4 = num3 * (poRow.Field<decimal>("pmlPurchaseUnitCostBase") + Math.Round(poRow.Field<decimal>("pmlSetupChargeBase") / num3, 5));
		string value = poRow.Field<string>("pmlPartID");
		string value2 = poRow.Field<string>("pmlPartRevisionID");
		SqlCommand sqlCommand = null;
		DataTable dataTable = null;
		switch (landedCostMethod)
		{
		case 1:
			if (totalDetails != 0m)
			{
				return num4 / totalDetails;
			}
			break;
		case 2:
			if (totalQuantity != 0m)
			{
				return num3 / totalQuantity;
			}
			break;
		case 3:
			sqlCommand = database.NewSqlCommand("Select IsNull(imrWeight,0) As imrWeight From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @PartRevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = value;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = value2;
			dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				num2 = dataTable.Rows[0].Field<decimal>("imrWeight") * num3;
			}
			if (totalWeight != 0m)
			{
				return num2 / totalWeight;
			}
			break;
		case 4:
			sqlCommand = database.NewSqlCommand("Select IsNull(imrVolume,0) As imrVolume From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @PartRevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = value;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = value2;
			dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				num = dataTable.Rows[0].Field<decimal>("imrVolume") * num3;
			}
			if (totalWeight != 0m)
			{
				return num / totalVolume;
			}
			break;
		}
		return result;
	}

	private void getCostMethodTotals(M1Database database, SqlTransaction transaction, string landedCostID, ref decimal totalDetails, ref decimal totalQuantity, ref decimal totalWeight, ref decimal totalVolume)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select pmpPlantID, pmlPartID, pmlJobType, pmlPartRevisionID, pmlPurchaseUnitCostBase, pmlPurchaseQuantity, pmlSetupChargeBase From PurchaseOrders Inner Join PurchaseOrderLines On pmpPurchaseOrderID = pmlPurchaseOrderID Where pmlLandedCostID = @LandedCostID  Order By pmlPurchaseOrderID, pmlPurchaseOrderLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = landedCostID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable == null)
		{
			return;
		}
		decimal num = default(decimal);
		foreach (DataRow row in dataTable.Rows)
		{
			num = row.Field<decimal>("pmlPurchaseQuantity");
			totalDetails += (row.Field<decimal>("pmlPurchaseUnitCostBase") + Math.Round(row.Field<decimal>("pmlSetupChargeBase") / num, 5)) * num;
			totalQuantity += num;
			SqlCommand sqlCommand2 = database.NewSqlCommand("Select IsNull(imrWeight,0) As imrWeight,IsNull(imrVolume,0) As imrVolume From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @PartRevisionID");
			sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = row.Field<string>("pmlPartID");
			sqlCommand2.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = row.Field<string>("pmlPartRevisionID");
			DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
			if (dataTable2 != null && dataTable2.Rows.Count != 0)
			{
				totalWeight += dataTable2.Rows[0].Field<decimal>("imrWeight") * num;
				totalVolume += dataTable2.Rows[0].Field<decimal>("imrVolume") * num;
			}
		}
	}

	private DataRow getExistentApInvoiceExpenseAccountRow(DataTable apInvoiceExpenseAccountsTable, COGSAccounts accounts, int jobType, Guid lcUniqueID)
	{
		DataRow result = null;
		string empty = string.Empty;
		Guid guid = default(Guid);
		foreach (DataRow row in apInvoiceExpenseAccountsTable.Rows)
		{
			guid = row.Field<Guid>("apxSourceTableUniqueID");
			empty = row.Field<string>("apxExpenseGLAccountID");
			if (jobType == 1 && accounts.WIPMaterialGLAccountID.Equals(empty) && lcUniqueID == guid)
			{
				result = row;
				break;
			}
			if (jobType == 2 && accounts.WIPSubcontractGLAccountID.Equals(empty) && lcUniqueID == guid)
			{
				result = row;
				break;
			}
			if (accounts.InventoryGLAccountID.Equals(empty) && lcUniqueID == guid)
			{
				result = row;
				break;
			}
		}
		return result;
	}

	private DataTable getApInvoiceExpenseAccountsTable(M1Database database, SqlTransaction transaction, out SqlDataAdapter expenseAccountsAdapter, string apInvoiceID, int apInvoiceLineID)
	{
		string queryString = $"Select * From APInvoiceExpenseAccounts where apxAPInvoiceID = {M1Util.ConvertToSql(apInvoiceID)} And apxAPInvoiceLineID = {apInvoiceLineID}";
		DataTable dataTable = database.GetDataTable(queryString, fillSchema: false, out expenseAccountsAdapter, transaction);
		if (dataTable != null)
		{
			return dataTable;
		}
		return null;
	}

	private DataRow getApInvoiceLinesRow(M1Database database, SqlTransaction transaction, string apInvoiceID, int apInvoiceLineID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select aplPartID, appPlantID From APInvoices Inner Join APInvoiceLines On appAPInvoiceID = aplAPInvoiceID Where aplAPInvoiceID = @APInvoiceID And aplAPInvoiceLineID = @APInvoiceLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@APInvoiceID", SqlDbType.NVarChar)).Value = apInvoiceID;
		sqlCommand.Parameters.Add(new SqlParameter("@APInvoiceLineID", SqlDbType.Int)).Value = apInvoiceLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private void removeAPInvoiceExpenseAccounts(M1Database database, SqlTransaction transaction, DataRow[] rowsToRemove)
	{
		foreach (DataRow row in rowsToRemove)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Delete From APInvoiceExpenseAccounts Where apxAPInvoiceID = @APInvoiceID And apxAPInvoiceLineID = @APInvoiceLineID And apxAPInvoiceExpenseAccountID = @APInvoiceExpenseAccountID");
			sqlCommand.Parameters.Add(new SqlParameter("@APInvoiceID", SqlDbType.NVarChar)).Value = row.Field<string>("apxAPInvoiceID");
			sqlCommand.Parameters.Add(new SqlParameter("@APInvoiceLineID", SqlDbType.SmallInt)).Value = row.Field<short>("apxAPInvoiceLineID");
			sqlCommand.Parameters.Add(new SqlParameter("@APInvoiceExpenseAccountID", SqlDbType.SmallInt)).Value = row.Field<short>("apxAPInvoiceExpenseAccountID");
			database.ExecuteCommand(sqlCommand);
		}
	}

	private void createAPInvoiceExpenseAccounts(M1Database database, SqlTransaction transaction, DataTable apInvoiceExpenseAccountsTable, DataRow landedCostCharges, DataRow organizations, DataRow apInvoice, DataRow apInvoiceLines)
	{
		string text = landedCostCharges.Field<string>("xazExpenseGLAccountID");
		bool flag = database.Props("FN").Field<bool>("xafGLCreateStockJournals");
		if (string.IsNullOrEmpty(text) && !flag)
		{
			string text2 = organizations.Field<string>("xazExpenseGLAccountID");
			if (!string.IsNullOrEmpty(text2))
			{
				text = text2;
			}
		}
		string apInvoiceID = apInvoiceLines.Field<string>("aplAPInvoiceID");
		short num = apInvoiceLines.Field<short>("aplAPInvoiceLineID");
		string iD = database.User.ID;
		DateTime now = DateTime.Now;
		if (flag)
		{
			COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, apInvoiceLines.Field<string>("aplPartID"), apInvoice.Field<string>("appPlantID"), string.Empty, string.Empty);
			if (!string.IsNullOrEmpty(cOGSAccounts.AccruedCreditorsGLAccountID))
			{
				string landedCostID = landedCostCharges.Field<string>("rmhLandedCostID");
				short landedCostChargeID = landedCostCharges.Field<short>("rmhLandedCostChargeID");
				_ = 0;
				createAccruedCreditorsExpenseAccounts(database, transaction, apInvoiceExpenseAccountsTable, landedCostID, landedCostChargeID, apInvoiceID, num, iD, now, cOGSAccounts.AccruedCreditorsGLAccountID);
				decimal landedCostVariance = apInvoiceLines.Field<decimal>("aplExtendedCostBase") - landedCostCharges.Field<decimal>("rmhTotalCost");
				database.Props("PN").Field<byte>("xapIMCostingMethod");
				createExpenseAccountsForVariance(database, transaction, apInvoiceExpenseAccountsTable, landedCostVariance, landedCostID, landedCostChargeID, apInvoiceID, num, iD, now, landedCostCharges.Field<byte>("rmhLandedCostMethod"));
				return;
			}
			DataTable landedCostExpenseAccountsTable = getLandedCostExpenseAccountsTable(database, transaction, landedCostCharges.Field<string>("rmhLandedCostCategoryID"));
			if (!createAPInvoiceExpenseAccountsFromEATable(landedCostExpenseAccountsTable, apInvoiceExpenseAccountsTable, apInvoiceLines, text, iD))
			{
				DataTable orgExpenseAccountsTable = getOrgExpenseAccountsTable(database, transaction, landedCostCharges.Field<string>("rmhSupplierOrganizationID"));
				if (!createAPInvoiceExpenseAccountsFromEATable(orgExpenseAccountsTable, apInvoiceExpenseAccountsTable, apInvoiceLines, text, iD) && !string.IsNullOrEmpty(text))
				{
					createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccountsTable, apInvoiceID, num, 1, iD, now, text, apInvoiceLines.Field<decimal>("aplExtendedCostBase"), 100m);
				}
			}
			return;
		}
		DataTable landedCostExpenseAccountsTable2 = getLandedCostExpenseAccountsTable(database, transaction, landedCostCharges.Field<string>("rmhLandedCostCategoryID"));
		if (!createAPInvoiceExpenseAccountsFromEATable(landedCostExpenseAccountsTable2, apInvoiceExpenseAccountsTable, apInvoiceLines, text, iD))
		{
			DataTable orgExpenseAccountsTable2 = getOrgExpenseAccountsTable(database, transaction, landedCostCharges.Field<string>("rmhSupplierOrganizationID"));
			if (!createAPInvoiceExpenseAccountsFromEATable(orgExpenseAccountsTable2, apInvoiceExpenseAccountsTable, apInvoiceLines, text, iD) && !string.IsNullOrEmpty(text))
			{
				createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccountsTable, apInvoiceID, num, 1, iD, now, text, apInvoiceLines.Field<decimal>("aplExtendedCostBase"), 100m);
			}
		}
	}

	private void createExpenseAccountsForVariance(M1Database database, SqlTransaction transaction, DataTable apInvoiceExpenseAccountsTable, decimal landedCostVariance, string landedCostID, int landedCostChargeID, string apInvoiceID, int apInvoiceLinesID, string userID, DateTime createdDate, int landedCostMethod)
	{
		int num = apInvoiceExpenseAccountsTable.Rows.Count;
		decimal num2 = default(decimal);
		if (!(landedCostVariance != 0m))
		{
			return;
		}
		decimal totalDetails = default(decimal);
		decimal totalQuantity = default(decimal);
		decimal totalWeight = default(decimal);
		decimal totalVolume = default(decimal);
		getCostMethodTotals(database, transaction, landedCostID, ref totalDetails, ref totalQuantity, ref totalWeight, ref totalVolume);
		DataRow dataRow = null;
		Guid guid = default(Guid);
		DataTable pOLandedCostChargesDetailLinesTable = getPOLandedCostChargesDetailLinesTable(database, transaction, landedCostID, landedCostChargeID);
		DataRow dataRow2 = null;
		foreach (DataRow row in pOLandedCostChargesDetailLinesTable.Rows)
		{
			dataRow2 = getPurchaseOrderRow(database, transaction, row.Field<string>("rmiPurchaseOrderID"), row.Field<short>("rmiPurchaseOrderLineID"));
			COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, dataRow2.Field<string>("pmlPartID"), dataRow2.Field<string>("pmpPlantID"), string.Empty, string.Empty);
			guid = row.Field<Guid>("rmiUniqueID");
			int num3 = dataRow2.Field<byte>("pmlJobType");
			if ((num3 != 0 || string.IsNullOrEmpty(cOGSAccounts.InventoryGLAccountID)) && (num3 != 1 || string.IsNullOrEmpty(cOGSAccounts.WIPMaterialGLAccountID)) && (num3 != 2 || string.IsNullOrEmpty(cOGSAccounts.WIPSubcontractGLAccountID)))
			{
				continue;
			}
			dataRow = getExistentApInvoiceExpenseAccountRow(apInvoiceExpenseAccountsTable, cOGSAccounts, num3, guid);
			if (dataRow == null)
			{
				if (row.Field<byte>("pmlPurchaseType") == 4 || row.Field<byte>("pmlPurchaseType") == 5)
				{
					num++;
					dataRow = createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccountsTable, apInvoiceID, apInvoiceLinesID, num, userID, createdDate, row.Field<string>("pmxExpenseGLAccountID"), 0m, 0m);
					dataRow["apxSourceTableName"] = "LandedCostChargeDetails";
					dataRow["apxSourceTableUniqueID"] = row.Field<Guid>("rmiUniqueID");
				}
				else
				{
					string expenseGLAccount = getExpenseGLAccount(num3, cOGSAccounts);
					SqlCommand sqlCommand = database.NewSqlCommand("select apxAPInvoiceID, apxAPInvoiceLineID, apxAPInvoiceExpenseAccountID, apxExpenseGLAccountID from APInvoiceExpenseAccounts  where apxAPInvoiceID = @APInvoiceID and apxAPInvoiceLineID = @APInvoiceLineID and apxExpenseGLAccountID = @GLExpenseAccount");
					sqlCommand.Parameters.AddWithValue("@APInvoiceID", apInvoiceID);
					sqlCommand.Parameters.AddWithValue("@APInvoiceLineID", apInvoiceLinesID);
					sqlCommand.Parameters.AddWithValue("@GLExpenseAccount", expenseGLAccount);
					DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						num++;
						dataRow = createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccountsTable, apInvoiceID, apInvoiceLinesID, num, userID, createdDate, expenseGLAccount, 0m, 0m);
						dataRow["apxSourceTableName"] = "LandedCostChargeDetails";
						dataRow["apxSourceTableUniqueID"] = row.Field<Guid>("rmiUniqueID");
					}
				}
			}
			decimal num4 = CalculatePercentValue(database, transaction, dataRow2, landedCostMethod, totalDetails, totalQuantity, totalWeight, totalVolume);
			dataRow["apxAmount"] = Math.Round(landedCostVariance * num4, 2);
			num2 += dataRow.Field<decimal>("apxAmount");
		}
		if (landedCostVariance - num2 != 0m)
		{
			dataRow["apxAmount"] = Math.Round(dataRow.Field<decimal>("apxAmount") + (landedCostVariance - num2), 2);
		}
	}

	private int createAccruedCreditorsExpenseAccounts(M1Database database, SqlTransaction transaction, DataTable apInvoiceExpenseAccountsTable, string landedCostID, int landedCostChargeID, string apInvoiceID, int apInvoiceLinesID, string userID, DateTime createdDate, string accountID)
	{
		int num = 0;
		SqlCommand sqlCommand = database.NewSqlCommand("Select LandedCostChargeDetails.* From LandedCostChargeDetails Where rmiLandedCostID = @LandedCostID and rmiLandedCostChargeID = @LandedCostChargeID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = landedCostID;
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostChargeID", SqlDbType.SmallInt)).Value = landedCostChargeID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			decimal num2 = dataTable.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("rmiTotalCost"));
			decimal d = default(decimal);
			foreach (DataRow row in dataTable.Rows)
			{
				decimal num3 = row.Field<decimal>("rmiTotalCost");
				num++;
				if (num2 != 0m)
				{
					d = num3 * 100m / num2;
				}
				DataRow dataRow = createAPInvoiceExpenseAccountsNewRow(apInvoiceExpenseAccountsTable, apInvoiceID, apInvoiceLinesID, num, userID, createdDate, accountID, Math.Round(num3, 2), Math.Round(d, 2));
				dataRow["apxSourceTableName"] = "LandedCostChargeDetails";
				dataRow["apxSourceTableUniqueID"] = row.Field<Guid>("rmiUniqueID");
			}
		}
		return num;
	}

	private string getExpenseGLAccount(int jobType, COGSAccounts accounts)
	{
		string empty = string.Empty;
		return jobType switch
		{
			1 => accounts.WIPMaterialGLAccountID, 
			2 => accounts.WIPSubcontractGLAccountID, 
			_ => accounts.InventoryGLAccountID, 
		};
	}

	private DataTable getLandedCostChargesDetailsTable(M1Database database, SqlTransaction transaction, string landedCostID, int landedCostChargeID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select LandedCostChargeDetails.* From LandedCostChargeDetails Where rmiLandedCostID = @LandedCostID and rmiLandedCostChargeID = @LandedCostChargeID");
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostID", SqlDbType.NVarChar)).Value = landedCostID;
		sqlCommand.Parameters.Add(new SqlParameter("@LandedCostChargeID", SqlDbType.SmallInt)).Value = landedCostChargeID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable;
		}
		return null;
	}

	private DataRow getPurchaseOrderRow(M1Database database, SqlTransaction transaction, string poID, int poLineID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select pmpPlantID, pmlPartID, pmlJobType, pmlPartRevisionID, pmlPurchaseUnitCostBase, pmlPurchaseQuantity, pmlSetupChargeBase From PurchaseOrders Inner Join PurchaseOrderLines On pmpPurchaseOrderID = pmlPurchaseOrderID Where pmlPurchaseOrderID = @PurchaseOrderID And pmlPurchaseOrderLineID = @PurchaseOrderLineID Order By pmlPurchaseOrderID, pmlPurchaseOrderLineID ");
		sqlCommand.Parameters.Add(new SqlParameter("@PurchaseOrderID", SqlDbType.NVarChar)).Value = poID;
		sqlCommand.Parameters.Add(new SqlParameter("@PurchaseOrderLineID", SqlDbType.Int)).Value = poLineID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private DataTable getOrgExpenseAccountsTable(M1Database database, SqlTransaction transaction, string supplierID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select xazSupplierOrganizationID, xazExpenseGLAccountID, xazSequence, xazPercent From ExpenseAccountSplits Where xazSupplierOrganizationID = @SupplierOrganizationID");
		sqlCommand.Parameters.Add(new SqlParameter("@SupplierOrganizationID", SqlDbType.NVarChar)).Value = supplierID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable;
		}
		return null;
	}

	private DataTable getLandedCostExpenseAccountsTable(M1Database database, SqlTransaction transaction, string landedCostCategoryID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select xazLandedCostCategoryID, xazExpenseGLAccountID, xazSequence, xazPercent From ExpenseAccountSplits Where xazLandedCostCategoryID = @landedCostCategoryID");
		sqlCommand.Parameters.Add(new SqlParameter("@landedCostCategoryID", SqlDbType.NVarChar)).Value = landedCostCategoryID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable;
		}
		return null;
	}

	private DataRow getOrganizationsRow(M1Database database, SqlTransaction transaction, string supplierID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select cmoAPInvoiceContactID, cmoSupplierPaymentTermID, IsNull((Select Top 1 xazExpenseGLAccountID From ExpenseAccountSplits Where xazSupplierOrganizationID = cmoOrganizationID Order By xazExpenseGLAccountID),'') As xazExpenseGLAccountID From Organizations where cmoOrganizationID = @OrganizationID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrganizationID", SqlDbType.NVarChar)).Value = supplierID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private DataRow getOrganizationLocationRow(M1Database database, string supplierID, string locationID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select cmlSupplierShippingMethodID, cmoSupplierTaxCodeID, cmoSupplierSecondTaxCodeID From OrganizationLocations Inner Join Organizations on cmoOrganizationID = cmlOrganizationID Where cmlOrganizationID = @OrganizationID and cmlLocationID = @LocationID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrganizationID", SqlDbType.NVarChar)).Value = supplierID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private DataRow getLandedCostChargesRow(M1Database database, SqlTransaction transaction, string landedCostID, int chargeID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select LandedCostCharges.*, IsNull((Select Top 1 xazExpenseGLAccountID From ExpenseAccountSplits Where xazLandedCostCategoryID = rmhLandedCostCategoryID Order By xazExpenseGLAccountID),'') As xazExpenseGLAccountID, IsNull((Select rmcPlantID From LandedCosts Where rmcLandedCostID = @rmhLandedCostID),'') As rmcPlantID From LandedCostCharges Where rmhLandedCostID = @rmhLandedCostID and rmhLandedCostChargeID = @rmhLandedCostChargeID");
		sqlCommand.Parameters.Add(new SqlParameter("@rmhLandedCostID", SqlDbType.NVarChar)).Value = landedCostID;
		sqlCommand.Parameters.Add(new SqlParameter("@rmhLandedCostChargeID", SqlDbType.SmallInt)).Value = chargeID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private DataRow getApInvoiceRow(M1Database database, SqlTransaction transaction, string invoice)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select * from APInvoices where appAPInvoiceID = @appAPInvoiceID");
		sqlCommand.Parameters.Add(new SqlParameter("@appAPInvoiceID", SqlDbType.NVarChar)).Value = invoice;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private DataTable getPOLandedCostChargesDetailLinesTable(M1Database database, SqlTransaction transaction, string landedCostID, int landedCostChargeID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select rmiLandedCostID, rmiLandedCostChargeID, rmiLandedCostChargeDetailID, rmiEstTotalCost, rmiTotalCost, rmiPurchaseOrderID, rmiPurchaseOrderLineID, rmiUniqueID, pmlJobType, pmlPurchaseType, pmxExpenseGLAccountID, pmxPercent, pmxAmount   From LandedCostChargeDetails  inner join PurchaseOrderLines on rmiPurchaseOrderID = pmlPurchaseOrderID and rmiPurchaseOrderLineID = pmlPurchaseOrderLineID  inner join PurchaseOrderAccounts on rmiPurchaseOrderID = pmxPurchaseOrderID and rmiPurchaseOrderLineID = pmxPurchaseOrderLineID  Where rmiLandedCostID = @LandedCostID and rmiLandedCostChargeID = @LandedCostChargeID");
		sqlCommand.Parameters.AddWithValue("@LandedCostID", landedCostID);
		sqlCommand.Parameters.AddWithValue("@LandedCostChargeID", landedCostChargeID);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable;
		}
		return null;
	}
}
