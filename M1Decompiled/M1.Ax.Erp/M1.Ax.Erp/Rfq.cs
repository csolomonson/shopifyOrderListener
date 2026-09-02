using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1Classes92;

namespace M1.Ax.Erp;

public class Rfq
{
	public void GetPriceForQuantity(M1Database database, DataRow row)
	{
		decimal num = default(decimal);
		string value = string.Empty;
		short num2 = -1;
		short num3 = -1;
		decimal num4 = 1m;
		if (row.Table.Columns.Contains("PurchaseQty"))
		{
			num = row.Field<decimal>("PurchaseQty");
		}
		if (row.Table.Columns.Contains("rqsRFQID"))
		{
			value = row.Field<string>("rqsRFQID");
		}
		if (row.Table.Columns.Contains("rqsRFQLineID"))
		{
			num2 = row.Field<short>("rqsRFQLineID");
		}
		if (row.Table.Columns.Contains("rqsRFQSupplierID"))
		{
			num3 = row.Field<short>("rqsRFQSupplierID");
		}
		if (row.Table.Columns.Contains("ConversionFactor") && !row.IsNull("ConversionFactor") && row.Field<decimal>("ConversionFactor") != 0m)
		{
			num4 = row.Field<decimal>("ConversionFactor");
		}
		if (!(num != 0m))
		{
			return;
		}
		if (row.Table.Columns.Contains("InventoryQty"))
		{
			row["InventoryQty"] = M1Math.Round(num / num4, database.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals"));
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select rqqRFQID,rqqRFQLineID,rqqRFQSupplierID,rqqRFQQuantityID,rqqQuantity,rqqPriceBase,rqqLeadTime from RFQQuantities where rqqRFQID = @RFQID and rqqRFQLineID = @LineID and rqqRFQSupplierID = @SupplierID order by rqqRFQID,rqqRFQLineID,rqqRFQSupplierID,rqqRFQQuantityID");
		sqlCommand.Parameters.Add(new SqlParameter("@RFQID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.SmallInt)).Value = num2;
		sqlCommand.Parameters.Add(new SqlParameter("@SupplierID", SqlDbType.SmallInt)).Value = num3;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		decimal num5 = default(decimal);
		decimal num6 = default(decimal);
		short num7 = 0;
		foreach (DataRow row2 in dataTable.Rows)
		{
			if (num >= row2.Field<decimal>("rqqQuantity") && num6 < row2.Field<decimal>("rqqQuantity"))
			{
				num6 = row2.Field<decimal>("rqqQuantity");
				num5 = row2.Field<decimal>("rqqPriceBase");
				num7 = row2.Field<short>("rqqLeadTime");
			}
		}
		if (num6 != 0m)
		{
			if (row.Table.Columns.Contains("PurchaseUnitCost"))
			{
				row["PurchaseUnitCost"] = num5;
			}
			if (row.Table.Columns.Contains("PurchaseLeadTime"))
			{
				row["PurchaseLeadTime"] = num7;
			}
		}
	}

	public int AddSuppliersToRfqLine(M1BindingSource bindingSource)
	{
		int num = 0;
		if (bindingSource != null)
		{
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("select imxOrganizationID,imxLocationID,imxOrgPartID,cmlPurchaseContactID,cmoCurrencyRateID from PartCrossReferences Inner Join OrganizationLocations On imxOrganizationID = cmlOrganizationID and imxLocationID = cmlLocationID Inner Join Organizations On imxOrganizationID = cmoOrganizationID Where imxPurchased = 1 and imxInactive = 0 and imxOrganizationID <> '' and imxPartID = @PartID and imxPartRevisionID = @RevisionID and (cmoSupplierStatus = 1 or cmoSupplierStatus = 2)");
				sqlCommand.Parameters.Add("@PartID", SqlDbType.NVarChar).Value = currentAsDataRow.Field<string>("rqlPartID");
				sqlCommand.Parameters.Add("@RevisionID", SqlDbType.NVarChar).Value = currentAsDataRow.Field<string>("rqlPartRevisionID");
				DataTable dataTable = bindingSource.Database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					int obj = Convert.ToInt32(currentAsDataRow["rqlRfqLineID"]);
					M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("RfqSuppliers");
					foreach (DataRow row in dataTable.Rows)
					{
						bool flag = true;
						foreach (DataRowView item in childBindingSource.GetDataView())
						{
							if (Convert.ToInt32(item.Row["rqsRFQLineID"]).Equals(obj) && item.Row.Field<string>("rqsSupplierOrganizationID").Trim().Equals(row.Field<string>("imxOrganizationID").Trim(), StringComparison.CurrentCultureIgnoreCase) && item.Row.Field<string>("rqsPurchaseLocationID").Trim().Equals(row.Field<string>("imxLocationID").Trim(), StringComparison.CurrentCultureIgnoreCase))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							DataRow obj2 = childBindingSource.AddNew() as DataRow;
							obj2["rqsSupplierOrganizationID"] = row["imxOrganizationID"];
							obj2["rqsPurchaseLocationID"] = row["imxLocationID"];
							obj2["rqsPurchaseContactID"] = row["cmlPurchaseContactID"];
							num++;
						}
					}
					if (num != 0)
					{
						bindingSource.OnDataChanged(new DataChangedEventArgs(DataChangedFlag.CurrentAndDetailRows));
					}
				}
			}
		}
		return num;
	}

	public void UpdateSourceFromRfq(M1Database database, string rfqID)
	{
		if (string.IsNullOrEmpty(rfqID))
		{
			return;
		}
		M1BindingSource m1BindingSource = null;
		M1BindingSource m1BindingSource2 = null;
		M1BindingSource m1BindingSource3 = null;
		M1BindingSource m1BindingSource4 = null;
		rfqID = rfqID.Trim();
		SqlCommand sqlCommand = database.NewSqlCommand("Select rqsRFQID,rqsRFQLineID,rqsRFQSupplierID,rqlPartID,rqlPartRevisionID,rqlInventoryUnitOfMeasure,rqlPartShortDescription,rqlPartLongDescriptionRTF,rqlPartLongDescriptionText,rqlAlternatePart,rqlRFQType,rqlQuoteID,rqlQuoteLineID,rqlQuoteAssemblyID,rqlQuoteMaterialID,rqlQuoteOperationID,rqlJobID,rqlJobAssemblyID,rqlJobMaterialID,rqlJobOperationID,rqsSupplierOrganizationID,rqsPurchaseLocationID From RFQLines Inner Join RFQSuppliers On rqlRFQID = rqsRFQID And rqlRFQLineID = rqsRFQLineID Where rqlRFQID = @RfqID And (rqlQuoteID <> '' Or rqlJobID <> '') And rqsSelectedSupplier <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@RfqID", SqlDbType.NVarChar)).Value = rfqID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		byte decimals = database.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals");
		sqlCommand = database.NewSqlCommand("Select * From RFQQuantities Inner Join RFQLines On rqqRFQID = rqlRFQID And rqqRFQLineID = rqlRFQLineID Inner Join RFQSuppliers On rqqRFQID = rqsRFQID And rqqRFQLineID = rqsRFQLineID And rqqRFQSupplierID = rqsRFQSupplierID Where rqqRFQID = @RfqID And (rqlQuoteID <> '' Or rqlJobID <> '') And rqsSelectedSupplier <> 0 And rqqQuantity <> 0 Order By rqqRFQID,rqqRFQLineID,rqqRFQSupplierID,rqqRFQQuantityID");
		sqlCommand.Parameters.Add(new SqlParameter("@RfqID", SqlDbType.NVarChar)).Value = rfqID;
		DataTable dataTable2 = database.GetDataTable(sqlCommand);
		foreach (DataRow row in dataTable.Rows)
		{
			double num = ((clsPartFunctions)((ScriptApp)database.GetService(typeof(ScriptApp))).Ax("PartFunctions")).GetConversionFactor(row.Field<string>("rqlPartID"), row.Field<string>("rqlPartRevisionID"), row.Field<string>("rqsSupplierOrganizationID"), row.Field<string>("rqsPurchaseLocationID"));
			if (num <= 0.0)
			{
				num = 1.0;
			}
			if (row.Field<string>("rqlQuoteID").Trim().Length != 0)
			{
				DataRow currentAsDataRow;
				if (Convert.ToInt32(row["rqlRfqType"]) == 1)
				{
					if (m1BindingSource == null)
					{
						m1BindingSource = new M1BindingSource(database);
						m1BindingSource.DataSourceTable = "QuoteMaterials";
					}
					m1BindingSource.ClearCache();
					m1BindingSource.NavigateTo(database, "qmmQuoteID = " + M1Util.ConvertToSql(row.Field<string>("rqlQuoteID")) + " And qmmQuoteLineID = " + M1Util.ConvertToSql(row["rqlQuoteLineID"]) + " And qmmQuoteAssemblyID = " + M1Util.ConvertToSql(row["rqlQuoteAssemblyID"]) + " And qmmQuoteMaterialID = " + M1Util.ConvertToSql(row["rqlQuoteMaterialID"]));
					currentAsDataRow = m1BindingSource.CurrentAsDataRow;
					if (currentAsDataRow != null)
					{
						if (!currentAsDataRow.Field<string>("qmmPartID").Trim().Equals(row.Field<string>("rqlPartID").Trim()) || !currentAsDataRow.Field<string>("qmmPartRevisionID").Trim().Equals(row.Field<string>("rqlPartRevisionID").Trim()))
						{
							currentAsDataRow["qmmPartID"] = row["rqlPartID"];
							currentAsDataRow["qmmPartRevisionID"] = row["rqlPartRevisionID"];
							currentAsDataRow["qmmUnitOfMeasure"] = row["rqlInventoryUnitOfMeasure"];
							currentAsDataRow["qmmPartShortDescription"] = row["rqlPartShortDescription"];
							currentAsDataRow["qmmPartLongDescriptionRTF"] = row["rqlPartLongDescriptionRTF"];
							currentAsDataRow["qmmPartLongDescriptionText"] = row["rqlPartLongDescriptionText"];
						}
						currentAsDataRow["qmmSupplierOrganizationID"] = row["rqsSupplierOrganizationID"];
						currentAsDataRow["qmmPurchaseLocationID"] = row["rqsPurchaseLocationID"];
						for (int i = 1; i <= 9; i++)
						{
							currentAsDataRow["qmmQuantityBreak" + i] = 0;
							currentAsDataRow["qmmUnitCost" + i] = 0;
						}
						int num2 = 0;
						DataRow[] array = dataTable2.Select("rqqRFQID = " + M1Util.ConvertToLinq(row.Field<string>("rqsRFQID")) + " And rqqRFQLineID = " + M1Util.ConvertToLinq(row["rqsRFQLineID"]) + " And rqqRFQSupplierID = " + M1Util.ConvertToLinq(row["rqsRFQSupplierID"]));
						foreach (DataRow dataRow2 in array)
						{
							num2++;
							currentAsDataRow["qmmQuantityBreak" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow2["rqqQuantity"]) / num), decimals);
							currentAsDataRow["qmmUnitCost" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow2["rqqPriceBase"]) * num), 5);
							currentAsDataRow["qmmLeadTime" + num2] = dataRow2["rqqLeadTime"];
						}
						currentAsDataRow["qmmSourceRFQID"] = rfqID;
						currentAsDataRow["qmmSourcePriceID"] = 0;
						m1BindingSource.SaveData();
					}
					continue;
				}
				if (m1BindingSource2 == null)
				{
					m1BindingSource2 = new M1BindingSource(database);
					m1BindingSource2.DataSourceTable = "QuoteOperations";
				}
				m1BindingSource2.ClearCache();
				m1BindingSource2.NavigateTo(database, "qmoQuoteID = " + M1Util.ConvertToSql(row.Field<string>("rqlQuoteID")) + " And qmoQuoteLineID = " + M1Util.ConvertToSql(row["rqlQuoteLineID"]) + " And qmoQuoteAssemblyID = " + M1Util.ConvertToSql(row["rqlQuoteAssemblyID"]) + " And qmoQuoteOperationID = " + M1Util.ConvertToSql(row["rqlQuoteOperationID"]));
				currentAsDataRow = m1BindingSource2.CurrentAsDataRow;
				if (currentAsDataRow != null)
				{
					if (!currentAsDataRow.Field<string>("qmoPartID").Trim().Equals(row.Field<string>("rqlPartID").Trim()) || !currentAsDataRow.Field<string>("qmoPartRevisionID").Trim().Equals(row.Field<string>("rqlPartRevisionID").Trim()))
					{
						currentAsDataRow["qmoPartID"] = row["rqlPartID"];
						currentAsDataRow["qmoPartRevisionID"] = row["rqlPartRevisionID"];
						currentAsDataRow["qmoUnitOfMeasure"] = row["rqlInventoryUnitOfMeasure"];
					}
					currentAsDataRow["qmoSupplierOrganizationID"] = row["rqsSupplierOrganizationID"];
					currentAsDataRow["qmoPurchaseLocationID"] = row["rqsPurchaseLocationID"];
					for (int k = 1; k <= 9; k++)
					{
						currentAsDataRow["qmoQuantityBreak" + k] = 0;
						currentAsDataRow["qmoUnitCost" + k] = 0;
					}
					int num2 = 0;
					DataRow[] array = dataTable2.Select("rqqRFQID = " + M1Util.ConvertToLinq(row.Field<string>("rqsRFQID")) + " And rqqRFQLineID = " + M1Util.ConvertToLinq(row["rqsRFQLineID"]) + " And rqqRFQSupplierID = " + M1Util.ConvertToLinq(row["rqsRFQSupplierID"]));
					foreach (DataRow dataRow3 in array)
					{
						num2++;
						currentAsDataRow["qmoQuantityBreak" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow3["rqqQuantity"]) / num), decimals);
						currentAsDataRow["qmoUnitCost" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow3["rqqPriceBase"]) * num), 5);
					}
					m1BindingSource2.SaveData();
				}
			}
			else
			{
				if (row.Field<string>("rqlJobID").Trim().Length == 0)
				{
					continue;
				}
				DataRow currentAsDataRow;
				if (Convert.ToInt32(row["rqlRfqType"]) == 1)
				{
					if (m1BindingSource3 == null)
					{
						m1BindingSource3 = new M1BindingSource(database);
						m1BindingSource3.DataSourceTable = "JobMaterials";
					}
					m1BindingSource3.ClearCache();
					m1BindingSource3.NavigateTo(database, "jmmJobID = " + M1Util.ConvertToSql(row.Field<string>("rqlJobID")) + " And jmmJobAssemblyID = " + M1Util.ConvertToSql(row["rqlJobAssemblyID"]) + " And jmmJobMaterialID = " + M1Util.ConvertToSql(row["rqlJobMaterialID"]));
					currentAsDataRow = m1BindingSource3.CurrentAsDataRow;
					if (currentAsDataRow != null && currentAsDataRow.Field<string>("jmmPurchaseOrderID").Trim().Length == 0)
					{
						if (!currentAsDataRow.Field<string>("jmmPartID").Trim().Equals(row.Field<string>("rqlPartID").Trim()) || !currentAsDataRow.Field<string>("jmmPartRevisionID").Trim().Equals(row.Field<string>("rqlPartRevisionID").Trim()))
						{
							currentAsDataRow["jmmPartID"] = row["rqlPartID"];
							currentAsDataRow["jmmPartRevisionID"] = row["rqlPartRevisionID"];
							currentAsDataRow["jmmUnitOfMeasure"] = row["rqlInventoryUnitOfMeasure"];
							currentAsDataRow["jmmPartShortDescription"] = row["rqlPartShortDescription"];
							currentAsDataRow["jmmPartLongDescriptionRTF"] = row["rqlPartLongDescriptionRTF"];
							currentAsDataRow["jmmPartLongDescriptionText"] = row["rqlPartLongDescriptionText"];
						}
						currentAsDataRow["jmmSupplierOrganizationID"] = row["rqsSupplierOrganizationID"];
						currentAsDataRow["jmmPurchaseLocationID"] = row["rqsPurchaseLocationID"];
						for (int l = 1; l <= 9; l++)
						{
							currentAsDataRow["jmmQuantityBreak" + l] = 0;
							currentAsDataRow["jmmUnitCost" + l] = 0;
						}
						int num2 = 0;
						DataRow[] array = dataTable2.Select("rqqRFQID = " + M1Util.ConvertToLinq(row.Field<string>("rqsRFQID")) + " And rqqRFQLineID = " + M1Util.ConvertToLinq(row["rqsRFQLineID"]) + " And rqqRFQSupplierID = " + M1Util.ConvertToLinq(row["rqsRFQSupplierID"]));
						foreach (DataRow dataRow4 in array)
						{
							num2++;
							currentAsDataRow["jmmQuantityBreak" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow4["rqqQuantity"]) / num), decimals);
							currentAsDataRow["jmmUnitCost" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow4["rqqPriceBase"]) * num), 5);
							currentAsDataRow["jmmLeadTime" + num2] = dataRow4["rqqLeadTime"];
						}
						m1BindingSource3.SaveData();
					}
					continue;
				}
				if (m1BindingSource4 == null)
				{
					m1BindingSource4 = new M1BindingSource(database);
					m1BindingSource4.DataSourceTable = "JobOperations";
				}
				m1BindingSource4.ClearCache();
				m1BindingSource4.NavigateTo(database, "jmoJobID = " + M1Util.ConvertToSql(row.Field<string>("rqlJobID")) + " And jmoJobAssemblyID = " + M1Util.ConvertToSql(row["rqlJobAssemblyID"]) + " And jmoJobOperationID = " + M1Util.ConvertToSql(row["rqlJobOperationID"]));
				currentAsDataRow = m1BindingSource4.CurrentAsDataRow;
				if (currentAsDataRow != null && currentAsDataRow.Field<string>("jmoPurchaseOrderID").Trim().Length == 0)
				{
					if (!currentAsDataRow.Field<string>("jmoPartID").Trim().Equals(row.Field<string>("rqlPartID").Trim()) || !currentAsDataRow.Field<string>("jmoPartRevisionID").Trim().Equals(row.Field<string>("rqlPartRevisionID").Trim()))
					{
						currentAsDataRow["jmoPartID"] = row["rqlPartID"];
						currentAsDataRow["jmoPartRevisionID"] = row["rqlPartRevisionID"];
						currentAsDataRow["jmoUnitOfMeasure"] = row["rqlInventoryUnitOfMeasure"];
					}
					currentAsDataRow["jmoSupplierOrganizationID"] = row["rqsSupplierOrganizationID"];
					currentAsDataRow["jmoPurchaseLocationID"] = row["rqsPurchaseLocationID"];
					for (int m = 1; m <= 9; m++)
					{
						currentAsDataRow["jmoQuantityBreak" + m] = 0;
						currentAsDataRow["jmoUnitCost" + m] = 0;
					}
					int num2 = 0;
					DataRow[] array = dataTable2.Select("rqqRFQID = " + M1Util.ConvertToLinq(row.Field<string>("rqsRFQID")) + " And rqqRFQLineID = " + M1Util.ConvertToLinq(row["rqsRFQLineID"]) + " And rqqRFQSupplierID = " + M1Util.ConvertToLinq(row["rqsRFQSupplierID"]));
					foreach (DataRow dataRow5 in array)
					{
						num2++;
						currentAsDataRow["jmoQuantityBreak" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow5["rqqQuantity"]) / num), decimals);
						currentAsDataRow["jmoUnitCost" + num2] = M1Math.Round(Convert.ToDecimal(Convert.ToDouble(dataRow5["rqqPriceBase"]) * num), 5);
					}
					m1BindingSource4.SaveData();
				}
			}
		}
	}

	public int TransferPrices(M1Database database, string rfqID, int line, int seq, bool expireExisting, bool useForeignAmounts)
	{
		int result = 0;
		SqlCommand sqlCommand = database.NewSqlCommand("select rqsSupplierOrganizationID,rqsPurchaseLocationID,rqsOrgPartID,rqlPartID,rqlPartRevisionID,rqsCurrencyRateID,rqlPurchaseUnitOfMeasure,rqlInventoryUnitOfMeasure from RFQSuppliers Inner Join RFQLines On rqsRFQID = rqlRFQID and rqsRFQLineID = rqlRFQLineID Where rqsRFQID = @RfqID and rqsRFQLineID = @RfqLine and rqsRFQSupplierID = @RfqSeq");
		sqlCommand.Parameters.Add(new SqlParameter("@RfqID", SqlDbType.NVarChar)).Value = rfqID;
		sqlCommand.Parameters.Add(new SqlParameter("@RfqLine", SqlDbType.Int)).Value = line;
		sqlCommand.Parameters.Add(new SqlParameter("@RfqSeq", SqlDbType.Int)).Value = seq;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			sqlCommand = database.NewSqlCommand("select rqqQuantity,rqqPriceBase,rqqPriceForeign,rqqLeadTime from RFQQuantities Where rqqRFQID = @RfqID and rqqRFQLineID = @RfqLine and rqqRFQSupplierID = @RfqSeq and rqqQuantity > 0 order by rqqQuantity");
			sqlCommand.Parameters.Add(new SqlParameter("@RfqID", SqlDbType.NVarChar)).Value = rfqID;
			sqlCommand.Parameters.Add(new SqlParameter("@RfqLine", SqlDbType.Int)).Value = line;
			sqlCommand.Parameters.Add(new SqlParameter("@RfqSeq", SqlDbType.Int)).Value = seq;
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			M1BindingSource m1BindingSource = new M1BindingSource(database);
			m1BindingSource.LoadDefinition(string.Empty, "PartPrices", null, true);
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("PartPriceBreaks");
			DataRow dataRow2 = m1BindingSource.AddNew() as DataRow;
			m1BindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2["imiPriceType"] = 1;
			dataRow2["imiPartID"] = dataRow["rqlPartID"];
			dataRow2["imiPartRevisionID"] = dataRow["rqlPartRevisionID"];
			dataRow2["imiOrganizationID"] = dataRow["rqsSupplierOrganizationID"];
			dataRow2["imiLocationID"] = dataRow["rqsPurchaseLocationID"];
			if (useForeignAmounts)
			{
				dataRow2["imiCurrencyRateID"] = dataRow["rqsCurrencyRateID"];
			}
			dataRow2["imiStartDate"] = DateTime.Today;
			dataRow2["imiRFQID"] = rfqID;
			int num = 0;
			foreach (DataRow row in dataTable2.Rows)
			{
				DataRow dataRow4 = childBindingSource.AddNew() as DataRow;
				num++;
				dataRow4["imjPartPriceBreakID"] = num;
				dataRow4["imjQuantity"] = row["rqqQuantity"];
				if (useForeignAmounts && dataRow.Field<string>("rqsCurrencyRateID").Trim().Length != 0)
				{
					dataRow4["imjUnitPrice"] = row["rqqPriceForeign"];
				}
				else
				{
					dataRow4["imjUnitPrice"] = row["rqqPriceBase"];
				}
				dataRow4["imjLeadTime"] = row["rqqLeadTime"];
			}
			if (expireExisting)
			{
				sqlCommand = database.NewSqlCommand("UPDATE PartPrices SET imiEndDate = @ExpireDate WHERE imiOrganizationID = @OrgID and imiPartID = @PartID And imiPartRevisionID = @RevisionID AND {fn IFNULL(imiStartDate,'19000101')} <= {fn CURDATE()} AND {fn IFNULL(imiEndDate,'20790606')} >= {fn CURDATE()} AND imiPriceType = 1");
				sqlCommand.Parameters.Add(new SqlParameter("@ExpireDate", SqlDbType.DateTime)).Value = DateTime.Today.Subtract(TimeSpan.FromDays(1.0));
				sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("imiOrganizationID").Trim();
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("imiPartID").Trim();
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = dataRow2.Field<string>("imiPartRevisionID").Trim();
				database.ExecuteCommand(sqlCommand);
			}
			m1BindingSource.SaveData();
			sqlCommand = database.NewSqlCommand("UPDATE RFQSuppliers SET rqsUpdatedPartPrices = 1 WHERE rqsRFQID = @RfqID and rqsRFQLineID = @RfqLine and rqsRFQSupplierID = @RfqSeq");
			sqlCommand.Parameters.Add(new SqlParameter("@RfqID", SqlDbType.NVarChar)).Value = rfqID;
			sqlCommand.Parameters.Add(new SqlParameter("@RfqLine", SqlDbType.Int)).Value = line;
			sqlCommand.Parameters.Add(new SqlParameter("@RfqSeq", SqlDbType.Int)).Value = seq;
			database.ExecuteCommand(sqlCommand);
			new Part().CreatePartCrossRef(database, dataRow.Field<string>("rqlPartID"), dataRow.Field<string>("rqlPartRevisionID"), dataRow.Field<string>("rqsOrgPartID"), dataRow.Field<string>("rqsSupplierOrganizationID"), dataRow.Field<string>("rqsPurchaseLocationID"), string.Empty, dataRow.Field<string>("rqlPurchaseUnitOfMeasure"), 1m, null);
			result = Convert.ToInt32(dataRow2["imiPartPriceID"]);
		}
		return result;
	}
}
