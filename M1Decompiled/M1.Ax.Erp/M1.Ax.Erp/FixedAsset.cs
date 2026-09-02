using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class FixedAsset
{
	public class FixedAssetTypeAccounts
	{
		public string AssetTypeID = string.Empty;

		public string PlantID = string.Empty;

		public string AssetGLAccountID = string.Empty;

		public string DepreciationGLAccountID = string.Empty;

		public string AccumDeprGLAccountID = string.Empty;

		public string ExpenseGLAccountID = string.Empty;

		public string RepairsGLAccountID = string.Empty;

		public string RevaluationGLAccountID = string.Empty;

		public string ProfitGLAccountID = string.Empty;

		public string LossGLAccountID = string.Empty;
	}

	public string CreateAssetAdjustmentInvoice(M1Database database, int assetAdjustmentID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From AssetAdjustments Inner Join Assets On faaAssetID = fapAssetID Where faaAssetAdjustmentID = @AdjustID");
		sqlCommand.Parameters.Add(new SqlParameter("@AdjustID", SqlDbType.Int)).Value = assetAdjustmentID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			using M1BindingSource m1BindingSource = new M1BindingSource(database);
			m1BindingSource.DataSourceTable = "ARInvoices";
			DataRow dataRow2 = (DataRow)m1BindingSource.AddNew();
			m1BindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2["arpInvoiceType"] = 1;
			dataRow2["arpCustomerOrganizationID"] = dataRow["faaCustomerOrganizationID"];
			dataRow2["arpARInvoiceLocationID"] = dataRow["faaARInvoiceLocationID"];
			dataRow2["arpARInvoiceContactID"] = dataRow["faaARInvoiceContactID"];
			dataRow2["arpShipOrganizationID"] = dataRow["faaCustomerOrganizationID"];
			dataRow2["arpShipLocationID"] = dataRow["faaARInvoiceLocationID"];
			dataRow2["arpShipContactID"] = dataRow["faaARInvoiceContactID"];
			dataRow2["arpOrderDate"] = dataRow["faaAdjustmentDate"];
			dataRow2["arpInvoiceDate"] = dataRow["faaAdjustmentDate"];
			dataRow2["arpGLFiscalYearID"] = dataRow["faaGLFiscalYearID"];
			dataRow2["arpGLFiscalYearPeriodID"] = dataRow["faaGLFiscalYearPeriodID"];
			dataRow2["arpCurrencyRateID"] = dataRow["faaCurrencyRateID"];
			dataRow2["arpCustomRate"] = dataRow["faaCustomRate"];
			if (dataRow2.Field<bool>("arpCustomRate"))
			{
				dataRow2["arpExchangeRate"] = dataRow["faaExchangeRate"];
			}
			dataRow2["arpPlantID"] = dataRow["fapPlantID"];
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
			DataRow dataRow3 = (DataRow)childBindingSource.AddNew();
			childBindingSource.SetKeyToNextAvailable(dataRow3);
			string text = dataRow.Field<string>("fapDescription");
			dataRow3["arlPartID"] = text.Substring(0, Math.Min(text.Length, 30));
			dataRow3["arlPartShortDescription"] = text;
			dataRow3["arlInvoiceQuantity"] = dataRow["faaQuantity"];
			dataRow3["arlOrderQuantity"] = dataRow["faaQuantity"];
			dataRow3["arlUnitPriceForeign"] = dataRow["faaValueForeign"];
			dataRow3["arlFullUnitPriceForeign"] = dataRow["faaValueForeign"];
			dataRow3["arlAssetID"] = dataRow["faaAssetID"];
			dataRow3["arlAssetAdjustmentID"] = dataRow["faaAssetAdjustmentID"];
			dataRow3["arlPayCommission"] = false;
			m1BindingSource.SaveData();
			return dataRow2.Field<string>("arpARInvoiceID");
		}
		return string.Empty;
	}

	public bool CreateAssetFromReceiptLine(M1Database database, SqlTransaction trans, DataRow lineRow, DataRow receiptRow, decimal quantity)
	{
		try
		{
			if (lineRow != null && receiptRow != null && quantity > 0m && !string.IsNullOrWhiteSpace(lineRow.Field<string>("rmlPurchaseOrderID")) && lineRow.Field<short>("rmlPurchaseOrderLineID") != 0)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Select pmpOrderDate,pmlPartShortDescription,pmlAssetTypeID,pmlItemType,pmlPartLongDescriptionRTF,pmlPartLongDescriptionText From PurchaseOrderLines Inner Join PurchaseOrders On pmlPurchaseOrderID = pmpPurchaseOrderID Where pmlPurchaseOrderID = @PoID And pmlPurchaseOrderLineID = @PoLineID And pmlPurchaseType = 4");
				sqlCommand.Parameters.Add(new SqlParameter("@PoID", SqlDbType.NVarChar)).Value = lineRow.Field<string>("rmlPurchaseOrderID");
				sqlCommand.Parameters.Add(new SqlParameter("@PoLineID", SqlDbType.SmallInt)).Value = lineRow.Field<short>("rmlPurchaseOrderLineID");
				DataTable dataTable = database.GetDataTable(sqlCommand, trans);
				if (dataTable.Rows.Count != 0)
				{
					DataRow row = dataTable.Rows[0];
					int i = 0;
					M1BindingSource m1BindingSource = new M1BindingSource(database, trans);
					m1BindingSource.LoadDefinition(string.Empty, "Assets", null, true, loadDataNow: false);
					m1BindingSource.ClearCache();
					for (; (decimal)i < quantity; i++)
					{
						DataRow dataRow = m1BindingSource.AddNew() as DataRow;
						m1BindingSource.SetKeyToNextAvailable(dataRow);
						dataRow.SetField("fapPurchaseOrderID", lineRow.Field<string>("rmlPurchaseOrderID"));
						dataRow.SetField("fapPurchaseOrderLineID", lineRow.Field<short>("rmlPurchaseOrderLineID"));
						dataRow.SetField("fapReceiptID", receiptRow.Field<string>("rmpReceiptID"));
						dataRow.SetField("fapReceiptLineID", lineRow.Field<short>("rmlReceiptLineID"));
						dataRow.SetField("fapSupplierOrganizationID", receiptRow.Field<string>("rmpSupplierOrganizationID"));
						dataRow.SetField("fapPurchaseDate", row.Field<DateTime>("pmpOrderDate"));
						dataRow.SetField("fapDescription", row.Field<string>("pmlPartShortDescription"));
						dataRow.SetField("fapLongDescriptionRTF", row.Field<string>("pmlPartLongDescriptionRTF"));
						dataRow.SetField("fapLongDescriptionText", row.Field<string>("pmlPartLongDescriptionText"));
						dataRow.SetField("fapAssetTypeID", row.Field<string>("pmlAssetTypeID"));
						dataRow.SetField("fapItemType", row.Field<string>("pmlItemType"));
						dataRow.SetField("fapReceiptDate", receiptRow.Field<DateTime>("rmpReceiptDate"));
						dataRow.SetField("fapPurchaseValue", lineRow.Field<decimal>("rmlInventoryUnitCost"));
						dataRow.SetField("fapQuantity", 1);
						dataRow.SetField("fapPlantID", receiptRow.Field<string>("rmpPlantID"));
					}
					if (i > 0)
					{
						m1BindingSource.SaveData();
					}
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return false;
	}

	public void SetAssetValues(M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return;
		}
		string value = currentAsDataRow.Field<string>("faaAssetID");
		short num = currentAsDataRow.Field<short>("faaGLFiscalYearID");
		byte b = currentAsDataRow.Field<byte>("faaGLFiscalYearPeriodID");
		currentAsDataRow["faaOpeningAssetValue"] = 0;
		currentAsDataRow["faaAccumulatedDepreciation"] = 0;
		currentAsDataRow["faaDepreciationThisYear"] = 0;
		currentAsDataRow["faaNetAssetValue"] = 0;
		currentAsDataRow["faaProfitOrLoss"] = 0;
		currentAsDataRow["faaClosingPeriodDepreciation"] = 0;
		currentAsDataRow["faaClosingPercent"] = 0;
		if (!currentAsDataRow.Field<string>("faaAdjustmentType").Equals("P", StringComparison.CurrentCultureIgnoreCase))
		{
			SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("select fasOpeningAccumBalance from AssetSchedules where fasAssetID = @AssetID And fasType = 'BOOK' And fasGLFiscalYearID = @Year Order By fasGLFiscalYearPeriodID Asc");
			sqlCommand.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
			DataTable dataTable = bindingSource.Database.GetDataTable(sqlCommand, bindingSource.Transaction);
			if (dataTable.Rows.Count != 0)
			{
				currentAsDataRow["faaAccumulatedDepreciation"] = dataTable.Rows[0].Field<decimal>("fasOpeningAccumBalance");
			}
			else
			{
				sqlCommand = bindingSource.Database.NewSqlCommand("select fasClosingAccumBalance from AssetSchedules where fasAssetID = @AssetID And fasType = 'BOOK' And fasGLFiscalYearID < @Year Order By fasGLFiscalYearID Desc,fasGLFiscalYearPeriodID Desc");
				sqlCommand.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
				sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = num;
				dataTable = bindingSource.Database.GetDataTable(sqlCommand, bindingSource.Transaction);
				if (dataTable.Rows.Count != 0)
				{
					currentAsDataRow["faaAccumulatedDepreciation"] = dataTable.Rows[0].Field<decimal>("fasClosingAccumBalance");
				}
			}
			sqlCommand = bindingSource.Database.NewSqlCommand("select fasGLFiscalYearID,fasGLFiscalYearPeriodID,fasNetAssetValue,fasClosingAssetValue,fasClosingAccumBalance,fasDepreciationAmount from AssetSchedules where fasAssetID = @AssetID And fasType = 'BOOK' And (fasGLFiscalYearID < @Year Or (fasGLFiscalYearID = @Year And fasGLFiscalYearPeriodID <= @Period)) Order By fasGLFiscalYearID Desc,fasGLFiscalYearPeriodID Desc");
			sqlCommand.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
			sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = b;
			dataTable = bindingSource.Database.GetDataTable(sqlCommand, bindingSource.Transaction);
			if (dataTable.Rows.Count == 0)
			{
				return;
			}
			currentAsDataRow["faaOpeningAssetValue"] = dataTable.Rows[0].Field<decimal>("fasClosingAssetValue");
			currentAsDataRow["faaDepreciationThisYear"] = dataTable.Rows[0].Field<decimal>("fasClosingAccumBalance") - currentAsDataRow.Field<decimal>("faaAccumulatedDepreciation");
			currentAsDataRow["faaNetAssetValue"] = dataTable.Rows[0].Field<decimal>("fasNetAssetValue");
			sqlCommand = bindingSource.Database.NewSqlCommand("Select IsNull(fapInServiceDate, fapDepreciationStartDate) As fapInServiceDate From Assets Where fapAssetID = @AssetID");
			sqlCommand.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
			DataTable dataTable2 = bindingSource.Database.GetDataTable(sqlCommand, bindingSource.Transaction);
			DateTime? dateTime = null;
			if (dataTable2.Rows.Count != 0)
			{
				dateTime = dataTable2.Rows[0].Field<DateTime?>("fapInServiceDate");
				if (dateTime.HasValue && dateTime.HasValue)
				{
					sqlCommand = bindingSource.Database.NewSqlCommand("Select glfGLFiscalYearID From GLFiscalYearPeriods Where glfGLFiscalYearID = @Year And glfGLFiscalYearPeriodID = @Period And glfStartDate <= @ServiceDate And glfEndDate >= @ServiceDate");
					sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
					sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = b;
					sqlCommand.Parameters.Add(new SqlParameter("@ServiceDate", SqlDbType.DateTime)).Value = dateTime;
					if (bindingSource.Database.GetDataTable(sqlCommand, bindingSource.Transaction).Rows.Count == 0)
					{
						dateTime = null;
					}
				}
			}
			currentAsDataRow.Field<DateTime>("faaAdjustmentDate");
			DateTime dateTime2 = new DateTime(currentAsDataRow.Field<DateTime>("faaAdjustmentDate").Year, currentAsDataRow.Field<DateTime>("faaAdjustmentDate").Month, 1).AddMonths(1).AddDays(-1.0);
			decimal num2 = M1Math.Round(Convert.ToDecimal((decimal)currentAsDataRow.Field<DateTime>("faaAdjustmentDate").Day / Convert.ToDecimal(dateTime2.Day)) * 100m, 2);
			if (dateTime.HasValue && dateTime.HasValue)
			{
				DateTime value2 = currentAsDataRow.Field<DateTime>("faaAdjustmentDate");
				DateTime? dateTime3 = dateTime;
				if ((value2 - dateTime3).Value.Days >= 0)
				{
					num2 = default(decimal);
				}
			}
			currentAsDataRow["faaClosingPercent"] = num2;
			if (dataTable.Rows[0].Field<short>("fasGLFiscalYearID") == num && dataTable.Rows[0].Field<byte>("fasGLFiscalYearPeriodID") == b && dataTable.Rows[0].Field<decimal>("fasDepreciationAmount") != 0m)
			{
				currentAsDataRow["faaClosingPeriodDepreciation"] = M1Math.Round(dataTable.Rows[0].Field<decimal>("fasDepreciationAmount") * (num2 / 100m), 2);
				decimal num3 = dataTable.Rows[0].Field<decimal>("fasDepreciationAmount") - currentAsDataRow.Field<decimal>("faaClosingPeriodDepreciation");
				currentAsDataRow["faaDepreciationThisYear"] = currentAsDataRow.Field<decimal>("faaDepreciationThisYear") - num3;
				currentAsDataRow["faaNetAssetValue"] = dataTable.Rows[0].Field<decimal>("fasNetAssetValue") + num3;
			}
			return;
		}
		SqlCommand sqlCommand2 = bindingSource.Database.NewSqlCommand("select fasOpeningAccumBalance from AssetSchedules where fasAssetID = @AssetID And fasType = 'TAX' And fasGLFiscalYearID = @Year Order By fasGLFiscalYearPeriodID Asc");
		sqlCommand2.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
		sqlCommand2.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
		DataTable dataTable3 = bindingSource.Database.GetDataTable(sqlCommand2, bindingSource.Transaction);
		if (dataTable3.Rows.Count != 0)
		{
			currentAsDataRow["faaAccumulatedDepreciation"] = dataTable3.Rows[0].Field<decimal>("fasOpeningAccumBalance");
		}
		else
		{
			sqlCommand2 = bindingSource.Database.NewSqlCommand("select fasClosingAccumBalance from AssetSchedules where fasAssetID = @AssetID And fasType = 'TAX' And fasGLFiscalYearID < @Year Order By fasGLFiscalYearID Desc,fasGLFiscalYearPeriodID Desc");
			sqlCommand2.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
			sqlCommand2.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
			dataTable3 = bindingSource.Database.GetDataTable(sqlCommand2, bindingSource.Transaction);
			if (dataTable3.Rows.Count != 0)
			{
				currentAsDataRow["faaAccumulatedDepreciation"] = dataTable3.Rows[0].Field<decimal>("fasClosingAccumBalance");
			}
		}
		sqlCommand2 = bindingSource.Database.NewSqlCommand("select fasGLFiscalYearID,fasGLFiscalYearPeriodID,fasNetAssetValue,fasClosingAssetValue,fasClosingAccumBalance,fasDepreciationAmount from AssetSchedules where fasAssetID = @AssetID And fasType = 'TAX' And (fasGLFiscalYearID < @Year Or (fasGLFiscalYearID = @Year And fasGLFiscalYearPeriodID < @Period)) Order By fasGLFiscalYearID Desc,fasGLFiscalYearPeriodID Desc");
		sqlCommand2.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar)).Value = value;
		sqlCommand2.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
		sqlCommand2.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = b;
		dataTable3 = bindingSource.Database.GetDataTable(sqlCommand2, bindingSource.Transaction);
		if (dataTable3.Rows.Count != 0)
		{
			currentAsDataRow["faaOpeningAssetValue"] = dataTable3.Rows[0].Field<decimal>("fasClosingAssetValue");
			currentAsDataRow["faaDepreciationThisYear"] = 0;
			currentAsDataRow["faaNetAssetValue"] = dataTable3.Rows[0].Field<decimal>("fasNetAssetValue");
		}
	}

	public FixedAssetTypeAccounts GetAssetTypeGLAccount(M1Database database, SqlTransaction transaction, string assetTypeID, string plantID)
	{
		FixedAssetTypeAccounts fixedAssetTypeAccounts = new FixedAssetTypeAccounts();
		SqlCommand sqlCommand = database.NewSqlCommand("Select fatAssetTypeID,fatAssetGLAccountID,fatDepreciationGLAccountID,fatAccumDeprGLAccountID,fatExpenseGLAccountID,fatRepairsGLAccountID,fatRevaluationGLAccountID,fatProfitGLAccountID,fatLossGLAccountID,  fayAssetGLAccountID,fayDepreciationGLAccountID,fayAccumDeprGLAccountID,fayExpenseGLAccountID,fayRepairsGLAccountID,fayRevaluationGLAccountID,fayProfitGLAccountID,fayLossGLAccountID From AssetTypes Left Outer Join AssetTypePlants On fatAssetTypeID = fayAssetTypeID And fayAssetTypePlantID = @PlantID Where fatAssetTypeID = @AssetTypeID");
		sqlCommand.Parameters.Add(new SqlParameter("@AssetTypeID", SqlDbType.NVarChar)).Value = assetTypeID;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			fixedAssetTypeAccounts.AssetTypeID = assetTypeID;
			fixedAssetTypeAccounts.PlantID = plantID;
			if (dataRow["fatAssetGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.AssetGLAccountID = dataRow.Field<string>("fatAssetGLAccountID");
			}
			if (dataRow["fayAssetGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.AssetGLAccountID = dataRow.Field<string>("fayAssetGLAccountID");
			}
			if (dataRow["fatDepreciationGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.DepreciationGLAccountID = dataRow.Field<string>("fatDepreciationGLAccountID");
			}
			if (dataRow["fayDepreciationGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.DepreciationGLAccountID = dataRow.Field<string>("fayDepreciationGLAccountID");
			}
			if (dataRow["fatAccumDeprGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.AccumDeprGLAccountID = dataRow.Field<string>("fatAccumDeprGLAccountID");
			}
			if (dataRow["fayAccumDeprGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.AccumDeprGLAccountID = dataRow.Field<string>("fayAccumDeprGLAccountID");
			}
			if (dataRow["fatExpenseGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.ExpenseGLAccountID = dataRow.Field<string>("fatExpenseGLAccountID");
			}
			if (dataRow["fayExpenseGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.ExpenseGLAccountID = dataRow.Field<string>("fayExpenseGLAccountID");
			}
			if (dataRow["fatRepairsGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.RepairsGLAccountID = dataRow.Field<string>("fatRepairsGLAccountID");
			}
			if (dataRow["fayRepairsGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.RepairsGLAccountID = dataRow.Field<string>("fayRepairsGLAccountID");
			}
			if (dataRow["fatRevaluationGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.RevaluationGLAccountID = dataRow.Field<string>("fatRevaluationGLAccountID");
			}
			if (dataRow["fayRevaluationGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.RevaluationGLAccountID = dataRow.Field<string>("fayRevaluationGLAccountID");
			}
			if (dataRow["fatProfitGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.ProfitGLAccountID = dataRow.Field<string>("fatProfitGLAccountID");
			}
			if (dataRow["fayProfitGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.ProfitGLAccountID = dataRow.Field<string>("fayProfitGLAccountID");
			}
			if (dataRow["fatLossGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.LossGLAccountID = dataRow.Field<string>("fatLossGLAccountID");
			}
			if (dataRow["fayLossGLAccountID"] != DBNull.Value)
			{
				fixedAssetTypeAccounts.LossGLAccountID = dataRow.Field<string>("fayLossGLAccountID");
			}
		}
		return fixedAssetTypeAccounts;
	}
}
