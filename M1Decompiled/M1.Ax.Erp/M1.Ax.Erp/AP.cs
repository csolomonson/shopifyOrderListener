using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class AP
{
	public int GetPaymentHeaderCount(M1Database database, int sessionID)
	{
		if (sessionID == 0)
		{
			return 0;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select count(*) from APPaymentHeaders where aptAPPaymentSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand));
	}

	public int GetRecurringPaymentHeaderCount(M1Database database, int sessionID)
	{
		if (sessionID == 0)
		{
			return 0;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select count(*) from APPaymentHeaders where aptAPPaymentSessionID = @SessionID And aptRecurringPaymentID <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand));
	}

	public bool CalculateAPTaxablePayment(M1Database database, int taxablePaymentID)
	{
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		M1User m1User = database.GetService(typeof(M1User)) as M1User;
		if (taxablePaymentID == 0)
		{
			return false;
		}
		try
		{
			using SqlCommand sqlCommand = database.NewSqlCommand("Select * From APTaxablePayments Where tprAPTaxablePaymentID = @recordID");
			sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Int)).Value = taxablePaymentID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count <= 0)
			{
				return false;
			}
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable2 = database.GetDataTable("SELECT * FROM APTaxablePaymentTotals WHERE 0=1", fillSchema: true, out adapter);
			DataRow dataRow = null;
			SqlDataAdapter adapter2 = new SqlDataAdapter();
			DataTable dataTable3 = database.GetDataTable("SELECT * FROM APTaxablePaymentTotalDetails WHERE 0=1", fillSchema: true, out adapter2);
			DataRow dataRow2 = null;
			DateTime dateTime = dataTable.Rows[0].Field<DateTime>("tprStartDate");
			DateTime dateTime2 = dataTable.Rows[0].Field<DateTime>("tprEndDate");
			string value = dataTable.Rows[0].Field<string>("tprPlantID");
			string value2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Delete From APTaxablePaymentTotals Where tptAPTaxablePaymentID = " + taxablePaymentID.ToSql());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Delete From APTaxablePaymentTotalDetails Where tpdAPTaxablePaymentID = " + taxablePaymentID.ToSql());
			database.ExecuteCommand(stringBuilder.ToString());
			if (m1DataDictionary.ProductCode.IsModulePurchased("MP", database))
			{
				value2 = " And apsPlantID = @plantID ";
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("Select aptAPPaymentSessionID, aptAPPaymentHeaderID, apnAPInvoiceID, appSupplierOrganizationID, cmoFederalID, cmoName, cmoLastName, cmoFirstGivenName, cmoSecondGivenName, cmoTradingName, cmoAddressLine1, cmoAddressLine2, cmoCity, cmoState, cmoPostCode, cmoCountry, cmoPhoneNumber, cmoBSBNumber, cmoBankAccountNumber, aptPaymentDate, aptPaymentAmount, apnPaymentAmount, apnPaymentAmount As PaymentAmount, ");
			stringBuilder2.Append("(Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),apnPaymentAmount) / Convert(decimal(13,6),appInvoiceTotalBase)) * (appInvoiceSubtotalBase+appFreightAmountBase)), 2) Else apnPaymentAmount End))) As tpdReportableAmount, ");
			stringBuilder2.Append("(Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),apnPaymentAmount) / Convert(decimal(13,6),appInvoiceTotalBase)) * appInvoiceTaxAmountBase), 2) Else apnPaymentAmount End))) As tpdGSTAmount, ");
			stringBuilder2.Append("Convert(decimal(13,2),((Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),apnPaymentAmount) / Convert(decimal(13,6),appInvoiceTotalBase)) * appInvoiceTaxAmountBase), 2) Else apnPaymentAmount End))) / (Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),apnPaymentAmount) / Convert(decimal(13,6),appInvoiceTotalBase)) * (appInvoiceSubtotalBase+appFreightAmountBase)), 2) Else apnPaymentAmount End)))) * 100) As TaxRate ");
			stringBuilder2.Append("From APPaymentLines Inner Join APPaymentHeaders On apnAPPaymentSessionID = aptAPPaymentSessionID And apnAPPaymentHeaderID = aptAPPaymentHeaderID Inner Join APPaymentSessions On aptAPPaymentSessionID = apsAPPaymentSessionID Inner Join APInvoices On apnAPInvoiceID = appAPInvoiceID Inner Join Organizations On aptSupplierOrganizationID = cmoOrganizationID ");
			stringBuilder2.Append("Where apnPostedToGL <> 0 And aptPaymentDate >= @startDate And aptPaymentDate <= @endDate And appTaxReportable <> 0 And (aptPaymentType = 1 or (aptPaymentType = 3 And ISNULL((Select Void.aptPaymentType From APPaymentHeaders Void Where Void.aptAPPaymentSessionID = APPaymentHeaders.aptVoidAPPaymentSessionID And Void.aptAPPaymentHeaderID = APPaymentHeaders.aptVoidAPPaymentHeaderID),0) = 1)) ");
			stringBuilder2.Append(value2);
			stringBuilder2.Append(" UNION ALL ");
			stringBuilder2.Append("Select aptAPPaymentSessionID, aptAPPaymentHeaderID, '' As apnAPInvoiceID, aptSupplierOrganizationID As appSupplierOrganizationID, cmoFederalID, cmoName, cmoLastName, cmoFirstGivenName, cmoSecondGivenName, cmoTradingName, cmoAddressLine1, cmoAddressLine2, cmoCity, cmoState, cmoPostCode, cmoCountry, cmoPhoneNumber, cmoBSBNumber, cmoBankAccountNumber, aptPaymentDate, aptPaymentAmount, apnPaymentAmount, apnPaymentAmount As PaymentAmount, apnPaymentAmount As tpdReportableAmount, apnTaxAmount As tpdGSTAmount, ");
			stringBuilder2.Append("ISNULL((Select Top 1 xabTaxRate From TaxCodeLines Where xabTaxCodeID = apnTaxCodeID And xabEffectiveDate <= aptPaymentDate Order By xabEffectiveDate DESC),0) As TaxRate ");
			stringBuilder2.Append("From APPaymentHeaders Inner Join APPaymentLines On aptAPPaymentSessionID = apnAPPaymentSessionID and aptAPPaymentHeaderID = apnAPPaymentHeaderID Inner Join APPaymentSessions On aptAPPaymentSessionID = apsAPPaymentSessionID Inner Join Organizations On aptSupplierOrganizationID = cmoOrganizationID ");
			stringBuilder2.Append("Where apnPostedToGL <> 0 And aptPaymentDate >= @startDate And aptPaymentDate <= @endDate And aptTaxReportable <> 0 And (aptPaymentType = 2 or (aptPaymentType = 3 And ISNULL((Select Void.aptPaymentType From APPaymentHeaders Void Where Void.aptAPPaymentSessionID = APPaymentHeaders.aptVoidAPPaymentSessionID And Void.aptAPPaymentHeaderID = APPaymentHeaders.aptVoidAPPaymentHeaderID),0) = 2)) ");
			stringBuilder2.Append(value2);
			stringBuilder2.Append(" UNION ALL ");
			stringBuilder2.Append("Select aptAPPaymentSessionID, aptAPPaymentHeaderID, apnAPInvoiceID, appSupplierOrganizationID, cmoFederalID, cmoName, cmoLastName, cmoFirstGivenName, cmoSecondGivenName, cmoTradingName, cmoAddressLine1, cmoAddressLine2, cmoCity, cmoState, cmoPostCode, cmoCountry, cmoPhoneNumber, cmoBSBNumber, cmoBankAccountNumber, aptPaymentDate, aptPaymentAmount, apnPaymentAmount, ");
			stringBuilder2.Append("(Convert(decimal(13,2), (Case When IsNull((Select Count(*) From APPaymentLines lnCount Where lnCount.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And lnCount.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And lnCount.apnAPInvoiceID <> ''),0) = 1 Then aptPaymentAmount ");
			stringBuilder2.Append("Else aptPaymentAmount * (Convert(decimal(13,2),apnPaymentAmount) / Convert(decimal(13,2),IsNull((Select SUM(apnPaymentAmount) From APPaymentLines totalLines Where totalLines.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And totalLines.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And totalLines.apnAPInvoiceID <> ''), apnPaymentAmount))) End))) As PaymentAmount, ");
			stringBuilder2.Append("(Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6), (Case When IsNull((Select Count(*) From APPaymentLines lnCount Where lnCount.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And lnCount.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And lnCount.apnAPInvoiceID <> ''),0) = 1 Then aptPaymentAmount ");
			stringBuilder2.Append("Else aptPaymentAmount * (Convert(decimal(13,2),apnPaymentAmount) / Convert(decimal(13,2),IsNull((Select SUM(apnPaymentAmount) From APPaymentLines totalLines Where totalLines.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And totalLines.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And totalLines.apnAPInvoiceID <> ''), apnPaymentAmount))) End)) / Convert(decimal(13,6),appInvoiceTotalBase)) * (appInvoiceSubtotalBase+appFreightAmountBase)), 2) Else apnPaymentAmount End))) As tpdReportableAmount, ");
			stringBuilder2.Append("(Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),(Case When IsNull((Select Count(*) From APPaymentLines lnCount Where lnCount.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And lnCount.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And lnCount.apnAPInvoiceID <> ''),0) = 1 Then aptPaymentAmount ");
			stringBuilder2.Append("Else aptPaymentAmount * (Convert(decimal(13,2),apnPaymentAmount) / Convert(decimal(13,2),IsNull((Select SUM(apnPaymentAmount) From APPaymentLines totalLines Where totalLines.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And totalLines.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And totalLines.apnAPInvoiceID <> ''), apnPaymentAmount))) End)) / Convert(decimal(13,6),appInvoiceTotalBase)) * appInvoiceTaxAmountBase), 2) Else apnPaymentAmount End))) As tpdGSTAmount, ");
			stringBuilder2.Append("Convert(decimal(13,2), ((Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),(Case When IsNull((Select Count(*) From APPaymentLines lnCount Where lnCount.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And lnCount.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And lnCount.apnAPInvoiceID <> ''),0) = 1 Then aptPaymentAmount ");
			stringBuilder2.Append("Else aptPaymentAmount * (Convert(decimal(13,2),apnPaymentAmount) / Convert(decimal(13,2),IsNull((Select SUM(apnPaymentAmount) From APPaymentLines totalLines Where totalLines.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And totalLines.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And totalLines.apnAPInvoiceID <> ''), apnPaymentAmount))) End)) / Convert(decimal(13,6),appInvoiceTotalBase)) * appInvoiceTaxAmountBase), 2) Else apnPaymentAmount End))) ");
			stringBuilder2.Append("/ (Convert(decimal(13,2), (Case When appInvoiceTotalBase <> 0 Then Round(((Convert(decimal(13,6),(Case When IsNull((Select Count(*) From APPaymentLines lnCount Where lnCount.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And lnCount.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And lnCount.apnAPInvoiceID <> ''),0) = 1 Then aptPaymentAmount ");
			stringBuilder2.Append("Else aptPaymentAmount * (Convert(decimal(13,2),apnPaymentAmount) / Convert(decimal(13,2),IsNull((Select SUM(apnPaymentAmount) From APPaymentLines totalLines Where totalLines.apnAPPaymentSessionID = APPaymentHeaders.aptAPPaymentSessionID And totalLines.apnAPPaymentHeaderID = APPaymentHeaders.aptAPPaymentHeaderID And totalLines.apnAPInvoiceID <> ''), apnPaymentAmount))) End)) / Convert(decimal(13,6),appInvoiceTotalBase)) * (appInvoiceSubtotalBase+appFreightAmountBase)), 2) Else apnPaymentAmount End)))) * 100) As TaxRate ");
			stringBuilder2.Append("From APPaymentLines Inner Join APPaymentHeaders On apnAPPaymentSessionID = aptAPPaymentSessionID And apnAPPaymentHeaderID = aptAPPaymentHeaderID Inner Join APPaymentSessions On aptAPPaymentSessionID = apsAPPaymentSessionID Inner Join APInvoices On apnAPInvoiceID = appAPInvoiceID Inner Join Organizations On aptSupplierOrganizationID = cmoOrganizationID ");
			stringBuilder2.Append("Where apnPostedToGL <> 0 And aptPaymentDate >= @startDate And aptPaymentDate <= @endDate And appTaxReportable <> 0 And (aptPaymentType = 7 or (aptPaymentType = 3 And ISNULL((Select Void.aptPaymentType From APPaymentHeaders Void Where Void.aptAPPaymentSessionID = APPaymentHeaders.aptVoidAPPaymentSessionID And Void.aptAPPaymentHeaderID = APPaymentHeaders.aptVoidAPPaymentHeaderID),0) = 7)) ");
			stringBuilder2.Append(value2);
			stringBuilder2.Append("Order By appSupplierOrganizationID, aptAPPaymentSessionID, aptAPPaymentHeaderID");
			using SqlCommand sqlCommand2 = database.NewSqlCommand(stringBuilder2.ToString());
			sqlCommand2.Parameters.Add(new SqlParameter("@startDate", SqlDbType.DateTime)).Value = dateTime;
			sqlCommand2.Parameters.Add(new SqlParameter("@endDate", SqlDbType.DateTime)).Value = dateTime2;
			sqlCommand2.Parameters.Add(new SqlParameter("@plantID", SqlDbType.NVarChar)).Value = value;
			DataTable dataTable4 = database.GetDataTable(sqlCommand2);
			if (dataTable4.Rows.Count > 0)
			{
				string text = string.Empty;
				int num = 0;
				int num2 = 0;
				decimal value3 = default(decimal);
				decimal value4 = default(decimal);
				decimal value5 = default(decimal);
				SqlCommand sqlCommand3 = database.NewSqlCommand("Select COUNT(*) As RecordCount From (Select appAPInvoiceID, aplExtendedCostBase, aplTaxAmountBase, ISNULL((Select Top 1 xabTaxRate From TaxCodeLines Where xabTaxCodeID = aplTaxCodeID And xabEffectiveDate <= appInvoiceDate Order By xabEffectiveDate DESC),0) As TaxRate From APInvoices Inner Join APInvoiceLines On appAPInvoiceID = aplAPInvoiceID Where appAPInvoiceID = @InvID) As Temp Where TaxRate <> 10 And TaxRate <> 0");
				sqlCommand3.Parameters.Add(new SqlParameter("@InvID", SqlDbType.NVarChar));
				foreach (DataRow row in dataTable4.Rows)
				{
					if (!text.Equals(row.Field<string>("appSupplierOrganizationID").Trim(), StringComparison.CurrentCultureIgnoreCase))
					{
						if (dataRow != null)
						{
							dataRow["tptGrossAmountPaid"] = Convert.ToInt32(value3);
							dataRow["tptTotalGST"] = Convert.ToInt32(value4);
							dataRow["tptTotalTaxWithhelD"] = Convert.ToInt32(value5);
							dataRow.EndEdit();
							dataTable2.Rows.Add(dataRow);
							value3 = default(decimal);
							value4 = default(decimal);
							value5 = default(decimal);
							num2 = 0;
						}
						dataRow = dataTable2.NewRow().BlankRow();
						dataRow.BeginEdit();
						dataRow["tptAPTaxablePaymentID"] = taxablePaymentID;
						dataRow["tptAPTaxablePaymentTotalID"] = ++num;
						dataRow["tptOrganizationID"] = row.Field<string>("appSupplierOrganizationID");
						dataRow["tptPayeeBusinessNumber"] = row.Field<string>("cmoFederalID");
						dataRow["tptPayeeBusinessName"] = row.Field<string>("cmoName");
						dataRow["tptPayeeLastName"] = row.Field<string>("cmoLastName");
						dataRow["tptPayeeFirstGivenName"] = row.Field<string>("cmoFirstGivenName");
						dataRow["tptPayeeSecondGivenName"] = row.Field<string>("cmoSecondGivenName");
						dataRow["tptPayeeTradingName"] = row.Field<string>("cmoTradingName");
						dataRow["tptPayeeAddressLine1"] = row.Field<string>("cmoAddressLine1");
						dataRow["tptPayeeAddressLine2"] = row.Field<string>("cmoAddressLine2");
						dataRow["tptPayeeCity"] = row.Field<string>("cmoCity");
						dataRow["tptPayeeState"] = row.Field<string>("cmoState");
						dataRow["tptPayeePostCode"] = row.Field<string>("cmoPostCode");
						dataRow["tptPayeeCountry"] = row.Field<string>("cmoCountry");
						dataRow["tptPayeePhoneNumber"] = row.Field<string>("cmoPhoneNumber");
						dataRow["tptPayeeBankBSBNumber"] = row.Field<string>("cmoBSBNumber");
						dataRow["tptPayeeBankAccountNumber"] = row.Field<string>("cmoBankAccountNumber");
						dataRow["tptAmendmentIndicator"] = "O";
						dataRow["tptClosed"] = 0;
						dataRow["tptCreatedBy"] = m1User.ID;
						dataRow["tptCreatedDate"] = DateTime.Now;
						text = row.Field<string>("appSupplierOrganizationID").Trim();
					}
					dataRow2 = dataTable3.NewRow().BlankRow();
					dataRow2.BeginEdit();
					dataRow2["tpdAPTaxablePaymentID"] = taxablePaymentID;
					dataRow2["tpdAPTaxablePaymentTotalID"] = num;
					dataRow2["tpdAPTaxablePaymentDetailID"] = ++num2;
					dataRow2["tpdAPInvoiceID"] = row.Field<string>("apnAPInvoiceID");
					dataRow2["tpdAPPaymentSessionID"] = row.Field<int>("aptAPPaymentSessionID");
					dataRow2["tpdAPPaymentHeaderID"] = row.Field<int>("aptAPPaymentHeaderID");
					dataRow2["tpdClosed"] = 0;
					dataRow2["tpdCreatedBy"] = m1User.ID;
					dataRow2["tpdCreatedDate"] = DateTime.Now;
					dataRow2["tpdReportableAmount"] = row.Field<decimal>("tpdReportableAmount") + row.Field<decimal>("tpdGSTAmount");
					if (row.Field<decimal>("TaxRate") == 10m || row.Field<decimal>("TaxRate") == 0m)
					{
						dataRow2["tpdGSTAmount"] = row.Field<decimal>("tpdGSTAmount");
						dataRow2["tpdTaxWithheld"] = 0;
					}
					else if (row.Field<string>("apnAPInvoiceID").Trim().Length == 0)
					{
						dataRow2["tpdGSTAmount"] = 0;
						dataRow2["tpdTaxWithheld"] = row.Field<decimal>("tpdGSTAmount");
					}
					else
					{
						sqlCommand3.Parameters["@InvID"].Value = row.Field<string>("apnAPInvoiceID").Trim();
						DataTable dataTable5 = database.GetDataTable(sqlCommand3);
						if (dataTable5.Rows.Count > 0)
						{
							if (dataTable5.Rows[0].Field<int>("RecordCount") > 0)
							{
								dataRow2["tpdGSTAmount"] = 0;
								dataRow2["tpdTaxWithheld"] = row.Field<decimal>("tpdGSTAmount");
							}
							else
							{
								dataRow2["tpdGSTAmount"] = row.Field<decimal>("tpdGSTAmount");
								dataRow2["tpdTaxWithheld"] = 0;
							}
						}
						else
						{
							dataRow2["tpdGSTAmount"] = row.Field<decimal>("tpdGSTAmount");
							dataRow2["tpdTaxWithheld"] = 0;
						}
					}
					value3 += Convert.ToDecimal(dataRow2["tpdReportableAmount"]);
					value4 += Convert.ToDecimal(dataRow2["tpdGSTAmount"]);
					value5 += Convert.ToDecimal(dataRow2["tpdTaxWithheld"]);
					dataRow2.EndEdit();
					dataTable3.Rows.Add(dataRow2);
				}
				if (dataRow != null)
				{
					dataRow["tptGrossAmountPaid"] = Convert.ToInt32(value3);
					dataRow["tptTotalGST"] = Convert.ToInt32(value4);
					dataRow["tptTotalTaxWithhelD"] = Convert.ToInt32(value5);
					dataRow.EndEdit();
					dataTable2.Rows.Add(dataRow);
				}
				if (dataTable2.Rows.Count <= 0)
				{
					return false;
				}
				database.UpdateData(dataTable2, adapter);
				if (dataTable3.Rows.Count > 0)
				{
					database.UpdateData(dataTable3, adapter2);
				}
				database.ExecuteCommand("Update APTaxablePayments Set tprTotalsCalculatedDate = GetDate(), tprTotalRecords = " + num.ToSql() + " Where tprAPTaxablePaymentID = " + taxablePaymentID.ToSql());
			}
		}
		catch (Exception ex)
		{
			throw new M1Exception(ex.Message);
		}
		return true;
	}

	public bool ReopenTaxablePayment(M1Database database, int taxablePaymentID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (taxablePaymentID == 0)
		{
			return false;
		}
		stringBuilder.Append("UPDATE APTaxablePayments SET tprClosed = 0, tprClosedDate = Null WHERE tprAPTaxablePaymentID = " + taxablePaymentID.ToSql());
		stringBuilder.Append("\r\n");
		stringBuilder.Append("UPDATE APTaxablePaymentTotals SET tptClosed = 0 WHERE tptAPTaxablePaymentID = " + taxablePaymentID.ToSql());
		stringBuilder.Append("\r\n");
		stringBuilder.Append("UPDATE APTaxablePaymentTotalDetails SET tpdClosed = 0 WHERE tpdAPTaxablePaymentID = " + taxablePaymentID.ToSql());
		stringBuilder.Append("\r\n");
		database.ExecuteCommand(stringBuilder.ToString());
		return true;
	}

	public bool CloseTaxablePayment(M1Database database, int taxablePaymentID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (taxablePaymentID == 0)
		{
			return false;
		}
		stringBuilder.Append("UPDATE APTaxablePayments SET tprClosed = 1, tprClosedDate = " + DateTime.Today.ToSql() + " WHERE tprAPTaxablePaymentID = " + taxablePaymentID.ToSql());
		stringBuilder.Append("\r\n");
		stringBuilder.Append("UPDATE APTaxablePaymentTotals SET tptClosed = 1 WHERE tptAPTaxablePaymentID = " + taxablePaymentID.ToSql());
		stringBuilder.Append("\r\n");
		stringBuilder.Append("UPDATE APTaxablePaymentTotalDetails SET tpdClosed = 1 WHERE tpdAPTaxablePaymentID = " + taxablePaymentID.ToSql());
		stringBuilder.Append("\r\n");
		database.ExecuteCommand(stringBuilder.ToString());
		return true;
	}

	public bool ExportAusTaxablePaymentToFile(M1Database database, int taxablePaymentID)
	{
		StreamWriter streamWriter = null;
		if (taxablePaymentID == 0)
		{
			return false;
		}
		try
		{
			string text = string.Empty;
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = "All Files (*.*)|*.*",
				FilterIndex = 2,
				RestoreDirectory = true,
				Title = "Save As",
				FileName = "PAIVS",
				CheckPathExists = true,
				ValidateNames = true
			};
			M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
			saveFileDialog.AutoUpgradeEnabled = appContext == null || !appContext.DisableOpenFileHelp;
			if (appContext.IsHosted)
			{
				saveFileDialog.InitialDirectory = appContext.Metadata.FileShareLocation;
			}
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				text = saveFileDialog.FileName;
				streamWriter = File.CreateText(text);
				saveFileDialog.Dispose();
			}
			_ = string.Empty;
			if (!string.IsNullOrWhiteSpace(text))
			{
				using (SqlCommand sqlCommand = database.NewSqlCommand("Select * From APTaxablePayments Where tprAPTaxablePaymentID = @recordID"))
				{
					sqlCommand.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Int)).Value = taxablePaymentID;
					DataTable dataTable = database.GetDataTable(sqlCommand);
					int num = 0;
					if (dataTable.Rows.Count <= 0)
					{
						streamWriter.Close();
						streamWriter.Dispose();
						throw new M1MissingOrInvalidDataException("Record does not exist.");
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("704IDENTREGISTER1");
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyBusinessNumber").Replace(" ", "").PadRight(11)
						.Substring(0, 11));
					stringBuilder.Append("P");
					stringBuilder.Append(formatDateForExport(dataTable.Rows[0].Field<DateTime?>("tprEndDate")));
					stringBuilder.Append("PCM");
					stringBuilder.Append("FPAIVV01.0");
					stringBuilder.Append(' ', 654);
					streamWriter.Write(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("704IDENTREGISTER2");
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyName").ToUpper().PadRight(200)
						.Substring(0, 200));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprContactPerson").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprContactPhoneNumber").Replace("-", " ").PadRight(15)
						.Substring(0, 15));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprContactFaxNumber").Replace("-", " ").PadRight(15)
						.Substring(0, 15));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyFileReference").PadRight(16).Substring(0, 16));
					stringBuilder.Append(' ', 403);
					_ = dataTable.Rows[0].Field<string>("tprContactPerson").ToUpper().PadRight(38)
						.Length;
					streamWriter.Write(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("704IDENTREGISTER3");
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyAddressLine1").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyAddressLine2").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyCity").ToUpper().PadRight(27)
						.Substring(0, 27));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyState").ToUpper().PadRight(3)
						.Substring(0, 3));
					stringBuilder.Append(removePunctuation(dataTable.Rows[0].Field<string>("tprCompanyPostCode")).PadRight(4).Substring(0, 4));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyCountry").ToUpper().PadRight(20)
						.Substring(0, 20));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyAddressLine1").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyAddressLine2").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyCity").ToUpper().PadRight(27)
						.Substring(0, 27));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyState").ToUpper().PadRight(3)
						.Substring(0, 3));
					stringBuilder.Append(removePunctuation(dataTable.Rows[0].Field<string>("tprCompanyPostCode")).PadRight(4).Substring(0, 4));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyCountry").ToUpper().PadRight(20)
						.Substring(0, 20));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyEmailAddress").PadRight(76).Substring(0, 76));
					stringBuilder.Append(' ', 351);
					streamWriter.Write(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("704IDENTITY");
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyBusinessNumber").Replace(" ", "").PadRight(11)
						.Substring(0, 11));
					if (dataTable.Rows[0].Field<short>("tprBranchNumber") == 0)
					{
						stringBuilder.Append("001");
					}
					else
					{
						stringBuilder.Append(Convert.ToString(dataTable.Rows[0].Field<short>("tprBranchNumber")).PadLeft(3, '0'));
					}
					stringBuilder.Append(Convert.ToString(dataTable.Rows[0].Field<short>("tprTaxYear")).PadRight(4).Substring(0, 4));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyName").ToUpper().PadRight(200)
						.Substring(0, 200));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyTradingName").ToUpper().PadRight(200)
						.Substring(0, 200));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyAddressLine1").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyAddressLine2").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyCity").ToUpper().PadRight(27)
						.Substring(0, 27));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyState").ToUpper().PadRight(3)
						.Substring(0, 3));
					stringBuilder.Append(removePunctuation(dataTable.Rows[0].Field<string>("tprCompanyPostCode")).PadRight(4).Substring(0, 4));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyCountry").ToUpper().PadRight(20)
						.Substring(0, 20));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprContactPerson").ToUpper().PadRight(38)
						.Substring(0, 38));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprContactPhoneNumber").Replace("-", " ").PadRight(15)
						.Substring(0, 15));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprContactFaxNumber").Replace("-", " ").PadRight(15)
						.Substring(0, 15));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("tprCompanyEmailAddress").PadRight(76).Substring(0, 76));
					stringBuilder.Append(' ');
					streamWriter.Write(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("704SOFTWARE");
					stringBuilder.Append("COMMERCIAL ECI M1 VERSION 8.0");
					stringBuilder.Append(' ', 51);
					stringBuilder.Append(' ', 613);
					streamWriter.Write(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					using (SqlCommand sqlCommand2 = database.NewSqlCommand("Select * From APTaxablePaymentTotals WHere tptAPTaxablePaymentID = @recordID"))
					{
						sqlCommand2.Parameters.Add(new SqlParameter("@recordID", SqlDbType.Int)).Value = taxablePaymentID;
						DataTable dataTable2 = database.GetDataTable(sqlCommand2);
						if (dataTable2.Rows.Count <= 0)
						{
							streamWriter.Close();
							streamWriter.Dispose();
							throw new M1MissingOrInvalidDataException("Record does not exist.");
						}
						foreach (DataRow row in dataTable2.Rows)
						{
							stringBuilder.Append("704DPAIVS");
							stringBuilder.Append(row.Field<string>("tptPayeeBusinessNumber").Replace(" ", "").PadRight(11)
								.Substring(0, 11));
							stringBuilder.Append(row.Field<string>("tptPayeeLastName").ToUpper().PadRight(30)
								.Substring(0, 30));
							stringBuilder.Append(row.Field<string>("tptPayeeFirstGivenName").ToUpper().PadRight(15)
								.Substring(0, 15));
							stringBuilder.Append(row.Field<string>("tptPayeeSecondGivenName").ToUpper().PadRight(15)
								.Substring(0, 15));
							stringBuilder.Append(row.Field<string>("tptPayeeBusinessName").ToUpper().PadRight(200)
								.Substring(0, 200));
							stringBuilder.Append(row.Field<string>("tptPayeeTradingName").ToUpper().PadRight(200)
								.Substring(0, 200));
							stringBuilder.Append(row.Field<string>("tptPayeeAddressLine1").ToUpper().PadRight(38)
								.Substring(0, 38));
							stringBuilder.Append(row.Field<string>("tptPayeeAddressLine2").ToUpper().PadRight(38)
								.Substring(0, 38));
							stringBuilder.Append(row.Field<string>("tptPayeeCity").ToUpper().PadRight(27)
								.Substring(0, 27));
							stringBuilder.Append(row.Field<string>("tptPayeeState").ToUpper().PadRight(3)
								.Substring(0, 3));
							stringBuilder.Append(removePunctuation(row.Field<string>("tptPayeePostCode")).PadRight(4).Substring(0, 4));
							stringBuilder.Append(row.Field<string>("tptPayeeCountry").ToUpper().PadRight(20)
								.Substring(0, 20));
							stringBuilder.Append(row.Field<string>("tptPayeePhoneNumber").Replace("-", " ").PadRight(15)
								.Substring(0, 15));
							stringBuilder.Append(row.Field<string>("tptPayeeBankBSBNumber").Replace(" ", "").PadRight(6, '0')
								.Substring(0, 6));
							stringBuilder.Append(row.Field<string>("tptPayeeBankAccountNumber").Replace(" ", "").PadRight(9, '0')
								.Substring(0, 9));
							stringBuilder.Append(formatCurrencyForExportAUS(row.Field<decimal>("tptGrossAmountPaid")).PadLeft(11, '0').Substring(0, 11));
							stringBuilder.Append(formatCurrencyForExportAUS(row.Field<decimal>("tptTotalTaxWithheld")).PadLeft(11, '0').Substring(0, 11));
							stringBuilder.Append(formatCurrencyForExportAUS(row.Field<decimal>("tptTotalGST")).PadLeft(11, '0').Substring(0, 11));
							stringBuilder.Append(row.Field<string>("tptAmendmentIndicator").PadRight(1).Substring(0, 1));
							stringBuilder.Append(' ', 30);
							streamWriter.Write(stringBuilder.ToString());
							stringBuilder.Length = 0;
							stringBuilder.Capacity = 0;
							num++;
						}
					}
					num++;
					stringBuilder.Append("704FILE-TOTAL");
					stringBuilder.Append(num.ToString().PadLeft(8, '0').Substring(0, 8));
					stringBuilder.Append(' ', 683);
					streamWriter.Write(stringBuilder.ToString());
				}
				streamWriter.Close();
				streamWriter.Dispose();
				return true;
			}
			return false;
		}
		catch (M1MissingOrInvalidDataException ex)
		{
			throw new M1MissingOrInvalidDataException(ex.Message);
		}
	}

	public bool ExportSepaNLCreditTransfer(M1Database database, int sessionID, string fileName)
	{
		if (sessionID == 0)
		{
			return false;
		}
		using (SqlCommand sqlCommand = database.NewSqlCommand("select count(aptAPPaymentHeaderID) as 'NbrOfTxs', max(apsPaymentAmount) as'CtrlSum', max(apsEFTSettlementDate) as 'ReqdExctnDt', max(glnBankAccountName) as 'glnBankAccountName', max(glnIBAN) as 'glnIBAN', max(glnBIC) as 'glnBIC' from M1_M1.dbo.APPaymentSessions s  inner join M1_M1.dbo.APPaymentHeaders h on s.apsAPPaymentSessionID = h.aptAPPaymentSessionID left outer join M1_M1.dbo.BankAccounts b on s.apsBankAccountID = b.glnBankAccountID where apsAPPaymentSessionID = @SessionID group by apsAPPaymentSessionID"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
			foreach (DataRow row3 in database.GetDataTable(sqlCommand).Rows)
			{
				if (string.IsNullOrWhiteSpace(fileName))
				{
					M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
					using SaveFileDialog saveFileDialog = new SaveFileDialog
					{
						Filter = "All Files (*.xml)|*.xml",
						FilterIndex = 2,
						RestoreDirectory = true,
						Title = "Save As",
						CheckPathExists = true,
						ValidateNames = true,
						AutoUpgradeEnabled = (appContext == null || !appContext.DisableOpenFileHelp)
					};
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						fileName = saveFileDialog.FileName;
					}
				}
				if (string.IsNullOrWhiteSpace(fileName))
				{
					continue;
				}
				XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, Encoding.UTF8);
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteStartElement("Document");
				xmlTextWriter.WriteStartAttribute("xmlns:xsi");
				xmlTextWriter.WriteString("http://www.w3.rog/2001/XMLSchema-instance");
				xmlTextWriter.WriteStartAttribute("xmlns");
				xmlTextWriter.WriteString("urn:iso:std:iso:20022:tech:xsd:pain.001.001.03");
				xmlTextWriter.WriteStartElement("CstmrCdtTrfInitn");
				xmlTextWriter.WriteStartElement("GrpHdr");
				xmlTextWriter.WriteStartElement("MsgId");
				xmlTextWriter.WriteValue(sessionID);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("CreDtTm");
				xmlTextWriter.WriteString(DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("NbOfTxs");
				xmlTextWriter.WriteValue(row3.Field<int>("NbrOfTxs"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("CtrlSum");
				decimal num = row3.Field<decimal>("CtrlSum");
				xmlTextWriter.WriteValue(M1Math.Round(num, 2));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("InitgPty");
				xmlTextWriter.WriteStartElement("Nm");
				xmlTextWriter.WriteString(database.Props("DatasetProperties").Field<string>("xadName"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PmtInf");
				xmlTextWriter.WriteStartElement("PmtInfId");
				xmlTextWriter.WriteValue(sessionID);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PmtMtd");
				xmlTextWriter.WriteString("TRF");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("BtchBookg");
				int num2 = row3.Field<int>("NbrOfTxs");
				if (num2 > 1)
				{
					xmlTextWriter.WriteString("true");
				}
				else
				{
					xmlTextWriter.WriteString("false");
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("NbOfTxs");
				xmlTextWriter.WriteValue(num2);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("CtrlSum");
				xmlTextWriter.WriteValue(M1Math.Round(num, 2));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PmtTpInf");
				xmlTextWriter.WriteStartElement("SvcLvl");
				xmlTextWriter.WriteStartElement("Cd");
				xmlTextWriter.WriteString("SEPA");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("ReqdExctnDt");
				xmlTextWriter.WriteString(row3.Field<DateTime>("ReqdExctnDt").ToString("yyyy-MM-dd"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Dbtr");
				xmlTextWriter.WriteStartElement("Nm");
				xmlTextWriter.WriteString(row3.Field<string>("glnBankAccountName"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("DbtrAcct");
				xmlTextWriter.WriteStartElement("Id");
				xmlTextWriter.WriteStartElement("IBAN");
				xmlTextWriter.WriteString(row3.Field<string>("glnIBAN"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("DbtrAgt");
				xmlTextWriter.WriteStartElement("FinInstnId");
				xmlTextWriter.WriteStartElement("BIC");
				xmlTextWriter.WriteString(row3.Field<string>("glnBIC"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				using (SqlCommand sqlCommand2 = database.NewSqlCommand("select * from APPaymentHeaders where aptAPPaymentSessionID = @SessionID"))
				{
					sqlCommand2.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionID;
					foreach (DataRow row4 in database.GetDataTable(sqlCommand2).Rows)
					{
						xmlTextWriter.WriteStartElement("CdtTrfTxInf");
						xmlTextWriter.WriteStartElement("PmtId");
						xmlTextWriter.WriteStartElement("EndToEndId");
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(sessionID);
						stringBuilder.Append("-");
						stringBuilder.Append(row4.Field<int>("aptAPPaymentHeaderID"));
						xmlTextWriter.WriteString(stringBuilder.ToString());
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteStartElement("Amt");
						xmlTextWriter.WriteStartElement("InstdAmt");
						xmlTextWriter.WriteStartAttribute("Ccy");
						xmlTextWriter.WriteString("EUR");
						xmlTextWriter.WriteEndAttribute();
						decimal num3 = row4.Field<decimal>("aptPaymentAmount");
						xmlTextWriter.WriteValue(M1Math.Round(num3, 2));
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteStartElement("CdtrAgt");
						xmlTextWriter.WriteStartElement("FinInstnId");
						xmlTextWriter.WriteStartElement("BIC");
						xmlTextWriter.WriteValue(row4.Field<string>("aptBIC"));
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteStartElement("Cdtr");
						xmlTextWriter.WriteStartElement("Nm");
						xmlTextWriter.WriteValue(row4.Field<string>("aptSupplierOrganizationID"));
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteStartElement("CdtrAcct");
						xmlTextWriter.WriteStartElement("Id");
						xmlTextWriter.WriteStartElement("IBAN");
						xmlTextWriter.WriteValue(row4.Field<string>("aptIBAN"));
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteStartElement("RmtInf");
						xmlTextWriter.WriteStartElement("Ustrd");
						xmlTextWriter.WriteValue(row4.Field<string>("aptEFTDescription"));
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
					}
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndDocument();
				xmlTextWriter.Close();
				return true;
			}
		}
		return false;
	}

	public void SetAPInvoiceAccounts(M1Database database, SqlTransaction transaction, DataRow apInvoiceRow)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(apInvoiceRow.Field<string>("appPlantID")))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select Case When IsNull(xavAPAPGLAccountID,'') = '' Then xauAPAPGLAccountID Else xavAPAPGLAccountID End As xauAPAPGLAccountID, Case When IsNull(xavAPFreightGLAccountID,'') = '' Then xauAPFreightGLAccountID Else xavAPFreightGLAccountID End As xauAPFreightGLAccountID,Case When IsNull(xavUseProperties,0) = 0 Then xauUseProperties Else xavUseProperties End As xauUseProperties From Plants Left Outer Join PlantDepartments On xauPlantID = xavPlantID And xavPlantDepartmentID = @xavPlantDepartmentID And xavUseProperties = 1 Where xauPlantID = @xauPlantID");
			sqlCommand.Parameters.Add(new SqlParameter("@xavPlantDepartmentID", SqlDbType.NVarChar)).Value = apInvoiceRow.Field<string>("appPlantDepartmentID");
			sqlCommand.Parameters.Add(new SqlParameter("@xauPlantID", SqlDbType.NVarChar)).Value = apInvoiceRow.Field<string>("appPlantID");
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable != null && dataTable.Rows.Count != 0 && dataTable.Rows[0].Field<bool>("xauUseProperties"))
			{
				flag = true;
				apInvoiceRow["appAPGLAccountID"] = dataTable.Rows[0].Field<string>("xauAPAPGLAccountID");
				apInvoiceRow["appFreightGLAccountID"] = dataTable.Rows[0].Field<string>("xauAPFreightGLAccountID");
			}
		}
		if (!flag)
		{
			apInvoiceRow["appAPGLAccountID"] = database.Props("AP").Field<string>("xafAPAPGLAccountID");
			apInvoiceRow["appFreightGLAccountID"] = database.Props("AP").Field<string>("xafAPFreightGLAccountID");
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("Select mcpARGLAccountID, mcpAPGLAccountID From CurrencyRates Where mcpCurrencyRateID = @currencyRateID");
		sqlCommand2.Parameters.Add(new SqlParameter("@currencyRateID", SqlDbType.NVarChar)).Value = apInvoiceRow.Field<string>("appCurrencyRateID");
		DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
		if (dataTable2 != null && dataTable2.Rows.Count != 0 && !string.IsNullOrEmpty(dataTable2.Rows[0].Field<string>("mcpAPGLAccountID")))
		{
			apInvoiceRow["appAPGLAccountID"] = dataTable2.Rows[0].Field<string>("mcpAPGLAccountID");
		}
	}

	private string formatDateForExport(DateTime? date)
	{
		if (!date.HasValue)
		{
			return "00000000";
		}
		return date.Value.ToString("ddMMyyyy");
	}

	private string removePunctuation(string data)
	{
		return data.Replace("-", "").Replace(" ", "").Replace("(", "")
			.Replace(")", "")
			.Replace(".", "")
			.Replace(",", "");
	}

	private string formatCurrencyForExportAUS(decimal amount)
	{
		return Math.Floor(Convert.ToDouble(amount)).ToString();
	}
}
