using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("OrganizationLocations")]
public class OrganizationLocationsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.NewKeyValues[1].ToString().Trim().Length == 0)
		{
			parm.ParentIDMustExist = false;
		}
		if (parm.NewKeyValues[0] == parm.OldKeyValues[0] && parm.OldKeyValues[1].ToString().Trim().Length == 0 && parm.NewKeyValues.ToString().Trim().Length > 0)
		{
			throw new M1Exception("The blank location cannot be changed to a different location for the same " + parm.DataDictionary.Language.GetLocalString("organization") + " ID.");
		}
		if (parm.OldKeyValues[1].ToString().Length == 0)
		{
			parm.DeleteStatements.AppendLine("DELETE FROM Organizations WHERE cmoOrganizationID = " + parm.OldKeyValues[0].ToSql());
		}
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		processOrganizationLocSalespeople(parm);
		if (parm.OldKeyValues[0].ToString().Trim() != parm.NewKeyValues[0].ToString().Trim())
		{
			parm.Database.ExecuteCommand("UPDATE Organizations SET CMODEFAULTSHIPLOCATIONID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMODEFAULTSHIPLOCATIONID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
			parm.Database.ExecuteCommand("UPDATE Organizations SET CMODEFAULTQUOTELOCATIONID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMODEFAULTQUOTELOCATIONID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
			parm.Database.ExecuteCommand("UPDATE Organizations SET CMODEFAULTARINVOICELOCATIONID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMODEFAULTARINVOICELOCATIONID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
			parm.Database.ExecuteCommand("UPDATE Organizations SET CMODEFAULTAPINVOICELOCATIONID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMODEFAULTAPINVOICELOCATIONID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
			parm.Database.ExecuteCommand("UPDATE Organizations SET CMODROPSHIPLOCATIONID = '' WHERE CMODROPSHIPORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMODROPSHIPLOCATIONID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
		}
		SqlDataAdapter adapter;
		if (parm.OldKeyValues[1].ToString().Trim().Length == 0)
		{
			new ChangeIDProcessing().ChangeIDMergeRecords(parm.Database, "ORGANIZATIONS", "cmoOrganizationID = " + parm.OldKeyValues[0].ToSql(), "cmoOrganizationID = " + parm.NewKeyValues[0].ToSql(), new string[1] { "CMOORGANIZATIONID" }, parm.ChangeIDType, parm.SqlTransaction);
			if (parm.Database.GetDataTable("SELECT cmoOrganizationID FROM Organizations WHERE cmoOrganizationID = " + parm.NewKeyValues[0].ToSql(), fillSchema: false, out adapter, parm.SqlTransaction).Rows.Count > 0)
			{
				parm.Database.ExecuteCommand("DELETE FROM Organizations WHERE cmoOrganizationID = " + parm.OldKeyValues[0].ToSql(), parm.SqlTransaction);
			}
		}
		if (parm.NewKeyValues[1].ToString().Length != 0 || parm.OldKeyValues[1].ToString().Length <= 0)
		{
			return;
		}
		DataTable dataTable = parm.Database.GetDataTable("SELECT * FROM Organizations WHERE cmoOrganizationID = " + parm.NewKeyValues[0].ToSql(), fillSchema: false, out adapter, parm.SqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			DataTable dataTable2 = parm.Database.GetDataTable("SELECT * FROM OrganizationLocations WHERE cmlOrganizationID = " + parm.OldKeyValues[0].ToSql() + " AND cmlLocationID = " + parm.OldKeyValues[1].ToSql(), fillSchema: false, out adapter, parm.SqlTransaction);
			if (dataTable2.Rows.Count > 0)
			{
				DataRow dataRow = dataTable.NewRow().BlankRow();
				M1Util.CopyMatchingFields(dataTable2.Rows[0], dataRow, "cml,ucml");
				dataRow["cmoOrganizationID"] = parm.NewKeyValues[0];
				dataTable.Rows.Add(dataRow);
				parm.Database.UpdateData(dataTable, adapter, parm.SqlTransaction);
			}
		}
	}

	private void processOrganizationLocSalespeople(ChangeIDProcessingParms parm)
	{
		string text = string.Empty;
		SqlDataAdapter adapter;
		DataTable dataTable = parm.Database.GetDataTable("SELECT * FROM OrganizationLocSalespeople WHERE cmkOrganizationID = " + parm.OldKeyValues[0].ToSql() + " And cmkLocationID = " + parm.OldKeyValues[1].ToSql() + " ORDER BY cmkOrganizationID, cmkLocationID, cmkSequenceID", fillSchema: false, out adapter, parm.SqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		int num = -1;
		bool flag = false;
		DataTable dataTable2 = parm.Database.GetDataTable("SELECT * FROM OrganizationLocSalespeople WHERE cmkOrganizationID = " + parm.NewKeyValues[0].ToSql() + " ORDER BY cmkOrganizationID, cmkLocationID, cmkSequenceID", fillSchema: false, out adapter, parm.SqlTransaction);
		foreach (DataRow row in dataTable.Rows)
		{
			if (row["cmkLocationID"].ToString().Trim() != text || num == -1)
			{
				num = 0;
				text = row["cmkLocationID"].ToString().Trim();
				DataRow[] array = dataTable2.Select("cmkLocationID = " + parm.NewKeyValues[1].ToLinq(), "cmkOrganizationID, cmkLocationID, cmkSequenceID");
				if (array.Length != 0)
				{
					flag = true;
					num = Convert.ToInt16(array[array.GetUpperBound(0)]["cmkSequenceID"]);
				}
				else
				{
					flag = false;
				}
			}
			if (dataTable2.Select("cmkLocationID = " + parm.NewKeyValues[1].ToLinq() + " AND cmkSalesEmployeeID = " + row["cmkSalesEmployeeID"].ToLinq()).Length != 0)
			{
				continue;
			}
			DataRow dataRow2 = dataTable2.NewRow().BlankRow();
			dataRow2.BeginEdit();
			foreach (DataColumn column in dataRow2.Table.Columns)
			{
				if (!SystemGeneratedFields.IsGenerated(column.ColumnName))
				{
					dataRow2[column.ColumnName] = row[column.ColumnName];
				}
			}
			num++;
			dataRow2["cmkSequenceID"] = num;
			dataRow2["cmkOrganizationID"] = parm.NewKeyValues[0];
			dataRow2["cmkLocationID"] = parm.NewKeyValues[1];
			if (flag)
			{
				dataRow2["cmkPercent"] = 0;
			}
			dataRow2.EndEdit();
			dataTable2.Rows.Add(dataRow2);
		}
		parm.Database.UpdateData(dataTable2, adapter, parm.SqlTransaction);
		parm.Database.ExecuteCommand("DELETE FROM OrganizationLocSalespeople WHERE cmkOrganizationID = " + parm.OldKeyValues[0].ToSql() + " And cmkLocationID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.NewKeyValues[1].ToString().Trim().Length == 0)
		{
			parm.Database.ExecuteCommand("Update Organizations Set cmoName = dest.cmlName, cmoAddressLine1 = dest.cmlAddressLine1, cmoAddressLine2 = dest.cmlAddressLine2, cmoAddressLine3 = dest.cmlAddressLine3, cmoCity = dest.cmlCity, cmoState = dest.cmlState, cmoCountry = dest.cmlCountry, cmoPostCode = dest.cmlPostCode, cmoPhoneNumber = dest.cmlPhoneNumber, cmoAlternatePhoneNumber = dest.cmlAlternatePhoneNumber, cmoFaxNumber = dest.cmlFaxNumber, cmoQuoteContactID = dest.cmlQuoteContactID, cmoShipContactID = dest.cmlShipContactID, cmoARInvoiceContactID = dest.cmlARInvoiceContactID,cmoPurchaseContactID = dest.cmlPurchaseContactID, cmoAPInvoiceContactID = dest.cmlAPInvoiceContactID,cmoCustomerTaxable = dest.cmlCustomerTaxable, cmoCustomerTaxCodeID = dest.cmlCustomerTaxCodeID, cmoCustomerShippingMethodID = dest.cmlCustomerShippingMethodID, cmoCustomerShipPaymentTypeID = dest.cmlCustomerShipPaymentTypeID,cmoCustomerPaymentTermsID = dest.cmlCustomerPaymentTermID, cmoARInvoicePerShipmentLine = dest.cmlARInvoicePerShipmentLine, cmoSupplierShippingMethodID = dest.cmlSupplierShippingMethodID, cmoSupplierPaymentTermID = dest.cmlSupplierPaymentTermID, cmoCurrencyRateID = dest.cmlCurrencyRateID, cmoCreditHold = dest.cmlCreditHold, cmoCustomerCreditLimit = dest.cmlCustomerCreditLimit, cmoTaxExemptNumber = dest.cmlTaxExemptNumber, cmoNonTaxReasonID = dest.cmlNonTaxReasonID, cmoEMailAddress = dest.cmlEMailAddress From Organizations Inner Join OrganizationLocations dest On cmlOrganizationID = cmoOrganizationID And cmlLocationID = '' Where cmoOrganizationID = " + parm.NewKeyValues[0].ToSql(), parm.SqlTransaction);
		}
		if (parm.OldKeyValues[1].ToString().Length == 0)
		{
			string text = new ChangeIDProcessing().ProcessChangeID(parm.Database, "ORGANIZATIONS", new object[1] { parm.OldKeyValues[0] }, new object[1] { parm.NewKeyValues[0] }, parm.ChangeIDType, parm.SqlTransaction, null);
			if (text.Length == 0)
			{
				throw new M1Exception("Error processing Change ID.");
			}
			text = text.Split(':')[1].TrimStart();
			if (text.Length != 0)
			{
				parm.ProcessChangeIdMessage.AppendLine(text);
			}
		}
	}
}
