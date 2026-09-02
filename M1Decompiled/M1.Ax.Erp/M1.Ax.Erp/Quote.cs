using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Quote
{
	public string CreateQuote(M1Database database, string customerID, string locationID, string currencyID, string partID, string revisionID)
	{
		using M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "Quotes", null, true);
		DataRow dataRow = m1BindingSource.AddNew() as DataRow;
		m1BindingSource.SetKeyToNextAvailable(dataRow);
		dataRow.SetField("qmpCustomerOrganizationID", customerID);
		dataRow.SetField("qmpARInvoiceLocationID", locationID);
		if (!string.IsNullOrWhiteSpace(currencyID))
		{
			dataRow.SetField("qmpCurrencyRateID", currencyID);
		}
		if (!string.IsNullOrWhiteSpace(partID))
		{
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("QuoteLines");
			M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("QuoteQuantities");
			childBindingSource2.NumberOfChildRowsToForce = 9;
			DataRow dataRow2 = childBindingSource.AddNew() as DataRow;
			childBindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2.SetField("qmlPartID", partID);
			dataRow2.SetField("qmlPartRevisionID", revisionID);
			if (string.IsNullOrWhiteSpace(dataRow2.Field<string>("qmlPartShortDescription")))
			{
				dataRow2.SetField("qmlPartShortDescription", partID);
			}
			childBindingSource2.MoveFirst();
			DataRow currentAsDataRow = childBindingSource2.CurrentAsDataRow;
			if (currentAsDataRow.Field<decimal>("qmqQuoteQuantity") == 0m)
			{
				currentAsDataRow.SetField("qmqQuoteQuantity", 1m);
			}
		}
		m1BindingSource.SaveData();
		return dataRow.Field<string>("qmpQuoteID");
	}

	public void DeleteQuoteAssembly(M1Database database, SqlTransaction transaction, string quoteID, int lineID, int asmID)
	{
		if (string.IsNullOrWhiteSpace(quoteID))
		{
			throw new M1Exception("Quote ID is required.");
		}
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select qmaQuoteAssemblyID,qmaParentAssemblyID from QuoteAssemblies where qmaQuoteID = @QuoteID and qmaQuoteLineID = @LineID");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineID;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
			if (dataTable.Rows.Count != 0)
			{
				deleteNextAsmLevel(database, transaction, dataTable, quoteID, lineID, asmID);
				deleteAsm(database, transaction, quoteID, lineID, asmID, deleteAsm: false);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void deleteAsm(M1Database database, SqlTransaction transaction, string quoteID, int lineID, int asmID, bool deleteAsm)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("DELETE FormInputValues FROM FormInputValues INNER JOIN QuoteAssemblies On xaiSourceUniqueID = qmaUniqueID WHERE qmaQuoteID = @QuoteID And qmaQuoteLineID = @LineID And qmaQuoteAssemblyID = @AsmID And xaiSourceTable = 'QUOTEASSEMBLIES'");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("DELETE FROM QuoteMaterials WHERE qmmQuoteID = @QuoteID And qmmQuoteLineID = @LineID AND qmmQuoteAssemblyID = @AsmID\rDELETE FROM QuoteOperations WHERE qmoQuoteID = @QuoteID And qmoQuoteLineID = @LineID AND qmoQuoteAssemblyID = @AsmID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		database.ExecuteCommand(sqlCommand, transaction);
		if (deleteAsm)
		{
			sqlCommand = database.NewSqlCommand("DELETE FROM QuoteAssemblies WHERE qmaQuoteID = @QuoteID And qmaQuoteLineID = @LineID AND qmaQuoteAssemblyID = @AsmID");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
			database.ExecuteCommand(sqlCommand, transaction);
		}
	}

	private void deleteNextAsmLevel(M1Database database, SqlTransaction transaction, DataTable assembliesTable, string quoteID, int lineID, int parentAsm)
	{
		DataRow[] array = assembliesTable.Select("qmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAsm) + " and qmaQuoteAssemblyID <> 0");
		foreach (DataRow dataRow in array)
		{
			deleteNextAsmLevel(database, transaction, assembliesTable, quoteID, lineID, Convert.ToInt32(dataRow["qmaQuoteAssemblyID"]));
			deleteAsm(database, transaction, quoteID, lineID, Convert.ToInt32(dataRow["qmaQuoteAssemblyID"]), deleteAsm: true);
		}
	}

	public int TransferPrices(M1Database database, string quoteID, int line, bool expireExisting)
	{
		int result = 0;
		SqlCommand sqlCommand = database.NewSqlCommand("select qmpCustomerOrganizationID,qmpARInvoiceLocationID,qmpQuoteLocationID,qmlOrgPartID,qmlPartID,qmlPartRevisionID,qmpCurrencyRateID,qmpExpirationDate from Quotes Inner Join QuoteLines On qmpQuoteID = qmlQuoteID Where qmlQuoteID = @QuoteID and qmlQuoteLineID = @QuoteLineID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = line;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			bool flag = !string.IsNullOrWhiteSpace(dataRow.Field<string>("qmpCurrencyRateID"));
			sqlCommand = database.NewSqlCommand("select qmqQuoteQuantity,qmqRevisedUnitPriceBase,qmqRevisedUnitPriceForeign,qmqLeadTime from QuoteQuantities Where qmqQuoteID = @QuoteID and qmqQuoteLineID = @QuoteLineID and qmqQuoteQuantity > 0 order by qmqQuoteQuantity");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.Int)).Value = line;
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			if (dataTable2.Rows.Count != 0)
			{
				M1BindingSource m1BindingSource = new M1BindingSource(database);
				m1BindingSource.LoadDefinition(string.Empty, "PartPrices", null, true);
				M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("PartPriceBreaks");
				DataRow dataRow2 = m1BindingSource.AddNew() as DataRow;
				m1BindingSource.SetKeyToNextAvailable(dataRow2);
				dataRow2["imiPriceType"] = 2;
				dataRow2["imiPartID"] = dataRow["qmlPartID"];
				dataRow2["imiPartRevisionID"] = dataRow["qmlPartRevisionID"];
				dataRow2["imiOrganizationID"] = dataRow["qmpCustomerOrganizationID"];
				dataRow2["imiLocationID"] = dataRow["qmpARInvoiceLocationID"];
				if (flag)
				{
					dataRow2["imiCurrencyRateID"] = dataRow["qmpCurrencyRateID"];
				}
				dataRow2["imiStartDate"] = DateTime.Today;
				dataRow2["imiEndDate"] = dataRow["qmpExpirationDate"];
				dataRow2["imiQuoteID"] = quoteID;
				int num = 0;
				foreach (DataRow row in dataTable2.Rows)
				{
					DataRow dataRow4 = childBindingSource.AddNew() as DataRow;
					num++;
					dataRow4["imjPartPriceBreakID"] = num;
					dataRow4["imjQuantity"] = row["qmqQuoteQuantity"];
					if (flag)
					{
						dataRow4["imjUnitPrice"] = row["qmqRevisedUnitPriceForeign"];
					}
					else
					{
						dataRow4["imjUnitPrice"] = row["qmqRevisedUnitPriceBase"];
					}
				}
				if (expireExisting)
				{
					sqlCommand = database.NewSqlCommand("UPDATE PartPrices SET imiEndDate = @ExpireDate WHERE imiOrganizationID = @OrgID and imiPartID = @PartID And imiPartRevisionID = @RevisionID AND {fn IFNULL(imiStartDate,'19000101')} <= {fn CURDATE()} AND {fn IFNULL(imiEndDate,'20790606')} >= {fn CURDATE()} AND imiPriceType = 2");
					sqlCommand.Parameters.Add(new SqlParameter("@ExpireDate", SqlDbType.DateTime)).Value = DateTime.Today.Subtract(TimeSpan.FromDays(1.0));
					sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("imiOrganizationID").Trim();
					sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("imiPartID").Trim();
					sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("imiPartRevisionID").Trim();
					database.ExecuteCommand(sqlCommand);
				}
				m1BindingSource.SaveData();
				new Part().CreatePartCrossRef(database, dataRow.Field<string>("qmlPartID"), dataRow.Field<string>("qmlPartRevisionID"), dataRow.Field<string>("qmlOrgPartID"), dataRow.Field<string>("qmpCustomerOrganizationID"), dataRow.Field<string>("qmpQuoteLocationID"), string.Empty, string.Empty, 1m, null);
				result = Convert.ToInt32(dataRow2["imiPartPriceID"]);
			}
		}
		return result;
	}

	public void RefreshMatrix(M1Database database, string whereClause)
	{
		DataTable dataTable = database.GetDataTable("Select qmlQuoteID,qmlQuoteLineID From QuoteLines Inner Join Quotes On qmlQuoteID = qmpQuoteID Where " + whereClause + " And qmlMatrixCalculated = 0 Order By qmlQuoteID,qmlQuoteLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		QuoteQuantity quoteQuantity = new QuoteQuantity();
		M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.DataSourceTable = "QuoteLines";
		foreach (DataRow row in dataTable.Rows)
		{
			m1BindingSource.NavigateTo(database, "qmlQuoteID = " + row.Field<string>("qmlQuoteID").ToSql() + " And qmlQuoteLineID = " + row.Field<short>("qmlQuoteLineID").ToSql());
			foreach (DataRow row2 in m1BindingSource.PrimaryTable.GetChildBindingSource("QuoteQuantities").GetDataTable().Rows)
			{
				quoteQuantity.CalculateUsingCurrentQty(database, row2, (SqlTransaction)null, false);
			}
			m1BindingSource.CurrentAsDataRow["qmlMatrixCalculated"] = true;
			m1BindingSource.SaveData();
			m1BindingSource.ClearCache();
		}
	}

	public void SetQuantities(M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("qmlPartID")) && string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("qmlPartGroupID")))
		{
			return;
		}
		M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("QuoteQuantities");
		if (childBindingSource != null && childBindingSource.Count == 0)
		{
			return;
		}
		childBindingSource.SetRowCount(0);
		childBindingSource.SetRowCount(childBindingSource.NumberOfChildRowsToForce);
		DataView dataView = childBindingSource.GetDataView(currentAsDataRow);
		DataRow row = bindingSource.Fields["qmlQuoteID"].RelatedTableGetDataRow("qmpCustomerOrganizationID,qmpARInvoiceLocationID,qmpCurrencyRateID,qmpQuoteDate");
		PriceCalculation sellingPrice = new Part().GetSellingPrice(bindingSource.CurrentDatabase, currentAsDataRow.Field<string>("qmlPartID"), currentAsDataRow.Field<string>("qmlPartRevisionID"), currentAsDataRow.Field<string>("qmlPartGroupID"), row.Field<string>("qmpCustomerOrganizationID"), row.Field<string>("qmpARInvoiceLocationID"), 0m, row.Field<string>("qmpCurrencyRateID"), row.Field<DateTime?>("qmpQuoteDate"));
		if (sellingPrice.PartPrice == null)
		{
			if (sellingPrice.CalculationType != PriceCalculationType.NoPrice && sellingPrice.FullPrice != 0m)
			{
				DataRowView dataRowView = dataView[0];
				dataRowView["qmqQuoteQuantity"] = 1;
				dataRowView["qmqLeadTime"] = string.Empty;
				if (sellingPrice.IsForeignCurrency)
				{
					dataRowView["qmqFullRevisedUnitPriceForeign"] = sellingPrice.DiscountedPrice;
					dataRowView["qmqRevisedUnitPriceForeign"] = sellingPrice.DiscountedPrice;
				}
				else
				{
					dataRowView["qmqFullRevisedUnitPriceBase"] = sellingPrice.DiscountedPrice;
					dataRowView["qmqRevisedUnitPriceBase"] = sellingPrice.DiscountedPrice;
				}
			}
			return;
		}
		int num = 0;
		foreach (PriceLineData line in sellingPrice.PartPrice.Lines)
		{
			if (!bindingSource.Database.Props("QM").Field<bool>("xapQMMultipleQuantities") && num >= 1)
			{
				break;
			}
			if (dataView.Count > num)
			{
				dataView[num]["qmqQuoteQuantity"] = line.Quantity;
			}
			num++;
		}
	}

	public void GetPriceForQuantity(M1Database database, DataRow row)
	{
		decimal num = default(decimal);
		string value = string.Empty;
		short num2 = -1;
		if (row.Table.Columns.Contains("OrderQty"))
		{
			num = row.Field<decimal>("OrderQty");
		}
		if (row.Table.Columns.Contains("qmlQuoteID"))
		{
			value = row.Field<string>("qmlQuoteID");
		}
		if (row.Table.Columns.Contains("qmlQuoteLineID"))
		{
			num2 = row.Field<short>("qmlQuoteLineID");
		}
		if (!(num != 0m))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select qmqQuoteQuantity,qmqRevisedUnitPriceBase,qmqAdditionalChargeBase,qmqRevisedUnitPriceForeign,qmqAdditionalChargeForeign,qmqAdditionalChargeDescription,qmqUnitDiscountBase,qmqUnitDiscountForeign from QuoteQuantities where qmqQuoteID = @QuoteID and qmqQuoteLineID = @LineID order by qmqQuoteID,qmqQuoteLineID,qmqQuoteQuantityID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.SmallInt)).Value = num2;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		decimal value2 = default(decimal);
		decimal value3 = default(decimal);
		decimal value4 = default(decimal);
		decimal value5 = default(decimal);
		decimal value6 = default(decimal);
		decimal value7 = default(decimal);
		string value8 = string.Empty;
		decimal num3 = default(decimal);
		foreach (DataRow row2 in dataTable.Rows)
		{
			if (num >= row2.Field<decimal>("qmqQuoteQuantity") && num3 < row2.Field<decimal>("qmqQuoteQuantity"))
			{
				num3 = row2.Field<decimal>("qmqQuoteQuantity");
				value2 = row2.Field<decimal>("qmqRevisedUnitPriceBase");
				value3 = row2.Field<decimal>("qmqRevisedUnitPriceForeign");
				value4 = row2.Field<decimal>("qmqAdditionalChargeBase");
				value5 = row2.Field<decimal>("qmqAdditionalChargeForeign");
				value8 = row2.Field<string>("qmqAdditionalChargeDescription");
				if (database.Props("OM").Field<bool>("xapOMEnableDiscountFields"))
				{
					value6 = row2.Field<decimal>("qmqUnitDiscountBase");
					value7 = row2.Field<decimal>("qmqUnitDiscountForeign");
				}
				else
				{
					value6 = default(decimal);
					value7 = default(decimal);
				}
			}
		}
		if (num3 != 0m)
		{
			if (row.Table.Columns.Contains("UnitPriceBase"))
			{
				row.SetField("UnitPriceBase", value2);
			}
			if (row.Table.Columns.Contains("AdditionalChargeBase"))
			{
				row.SetField("AdditionalChargeBase", value4);
			}
			if (row.Table.Columns.Contains("UnitPriceForeign"))
			{
				row.SetField("UnitPriceForeign", value3);
			}
			if (row.Table.Columns.Contains("AdditionalChargeForeign"))
			{
				row.SetField("AdditionalChargeForeign", value5);
			}
			if (row.Table.Columns.Contains("AdditionalChargeDescription"))
			{
				row.SetField("AdditionalChargeDescription", value8);
			}
			if (row.Table.Columns.Contains("UnitDiscountBase"))
			{
				row.SetField("UnitDiscountBase", value6);
			}
			if (row.Table.Columns.Contains("UnitDiscountForeign"))
			{
				row.SetField("UnitDiscountForeign", value7);
			}
		}
	}

	public void UpdateFieldsInGrid(M1Database database, DataRow row, string changedField)
	{
		if (!row.Table.Columns.Contains("FieldSelected") || !row.Table.Columns.Contains("CreateJob") || !row.Table.Columns.Contains("TransferQuoteMethod") || !row.Table.Columns.Contains("qmlPartID"))
		{
			return;
		}
		int num = 0;
		DataTable dataTable = database.GetDataTable("Select IsNull(impDeliveryType, 0) As impDeliveryType From Parts Where impPartID = " + row.Field<string>("qmlPartID").ToSql());
		if (dataTable.Rows.Count != 0)
		{
			num = dataTable.Rows[0].Field<byte>("impDeliveryType");
		}
		bool flag = num == 1;
		if (changedField.Equals("FieldSelected"))
		{
			if (row.Field<bool>("FieldSelected"))
			{
				if (database.Props("OM").Field<bool>("xapOMMarkCreateJobForMTO") && flag)
				{
					row.SetField("CreateJob", value: true);
				}
				if (row.Field<bool>("CreateJob") && database.Props("OM").Field<bool>("xapOMMarkPullQuoteMethodForMTO") && flag)
				{
					row.SetField("TransferQuoteMethod", value: true);
				}
			}
			else
			{
				row.SetField("CreateJob", value: false);
				row.SetField("TransferQuoteMethod", value: false);
			}
		}
		else
		{
			if (!changedField.Equals("CreateJob"))
			{
				return;
			}
			if (row.Field<bool>("CreateJob"))
			{
				if (database.Props("OM").Field<bool>("xapOMMarkPullQuoteMethodForMTO") && flag)
				{
					row.SetField("TransferQuoteMethod", value: true);
				}
			}
			else
			{
				row.SetField("CreateJob", value: false);
				row.SetField("TransferQuoteMethod", value: false);
			}
		}
	}

	public void CloseRelatedFollowups(M1Database database, SqlTransaction transaction, string quoteID, string OrderID)
	{
		if (string.IsNullOrWhiteSpace(quoteID))
		{
			return;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
		m1BindingSource.LoadDefinition(string.Empty, "Followups", null, true, loadDataNow: false);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo(database, "cmfQuoteID = " + quoteID.ToSql() + " and cmfStatus <> 3");
		if (m1BindingSource.Count == 0)
		{
			return;
		}
		string value = "Transferred Quote " + quoteID.Trim().ToUpper() + " to Order " + OrderID.Trim().ToUpper() + " on " + DateTime.Today.ToLongDateString();
		foreach (DataRow row in m1BindingSource.GetDataTable().Rows)
		{
			row["cmfStatus"] = 3;
			row["cmfCompletedDate"] = DateTime.Today;
			if (string.IsNullOrWhiteSpace(row.Field<string>("cmfLongDescriptionText")))
			{
				row["cmfLongDescriptionText"] = value;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(row.Field<string>("cmfLongDescriptionText"));
				stringBuilder.AppendLine(value);
				row["cmfLongDescriptionText"] = stringBuilder.ToString();
			}
			row["cmfLongDescriptionRTF"] = row.Field<string>("cmfLongDescriptionText");
		}
		m1BindingSource.SaveData();
	}
}
