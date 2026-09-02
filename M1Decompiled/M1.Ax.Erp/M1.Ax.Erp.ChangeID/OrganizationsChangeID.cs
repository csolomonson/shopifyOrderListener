using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("Organizations")]
public class OrganizationsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		processExpenseAccountSplits(parm);
		processOrganizationLocSalespeople(parm);
	}

	private void processOrganizationLocSalespeople(ChangeIDProcessingParms changeIdProcessingParms)
	{
		string text = string.Empty;
		SqlDataAdapter adapter;
		DataTable dataTable = changeIdProcessingParms.Database.GetDataTable("SELECT * FROM OrganizationLocSalespeople WHERE cmkOrganizationID = " + changeIdProcessingParms.OldKeyValues[0].ToSql() + " ORDER BY cmkOrganizationID, cmkLocationID, cmkSequenceID", fillSchema: false, out adapter, changeIdProcessingParms.SqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		int num = -1;
		bool flag = false;
		DataTable dataTable2 = changeIdProcessingParms.Database.GetDataTable("SELECT * FROM OrganizationLocSalespeople WHERE cmkOrganizationID = " + changeIdProcessingParms.NewKeyValues[0].ToSql() + " ORDER BY cmkOrganizationID, cmkLocationID, cmkSequenceID", fillSchema: false, out adapter, changeIdProcessingParms.SqlTransaction);
		foreach (DataRow row in dataTable.Rows)
		{
			if (row["cmkLocationID"].ToString().Trim() != text || num == -1)
			{
				num = 0;
				text = row["cmkLocationID"].ToString().Trim();
				DataRow[] array = dataTable2.Select("cmkLocationID = " + row["cmkLocationID"].ToLinq(), "cmkOrganizationID, cmkLocationID, cmkSequenceID");
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
			if (dataTable2.Select("cmkLocationID = " + row["cmkLocationID"].ToLinq() + " AND cmkSalesEmployeeID = " + row["cmkSalesEmployeeID"].ToLinq()).Length != 0)
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
			dataRow2["cmkOrganizationID"] = changeIdProcessingParms.NewKeyValues[0];
			if (flag)
			{
				dataRow2["cmkPercent"] = 0;
			}
			dataRow2.EndEdit();
			dataTable2.Rows.Add(dataRow2);
		}
		changeIdProcessingParms.Database.UpdateData(dataTable2, adapter, changeIdProcessingParms.SqlTransaction);
		changeIdProcessingParms.Database.ExecuteCommand("DELETE FROM OrganizationLocSalespeople WHERE cmkOrganizationID = " + changeIdProcessingParms.OldKeyValues[0].ToSql(), changeIdProcessingParms.SqlTransaction);
	}

	private void processExpenseAccountSplits(ChangeIDProcessingParms changeIdProcessingParms)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = changeIdProcessingParms.Database.GetDataTable("SELECT * FROM ExpenseAccountSplits WHERE xazSupplierOrganizationID = " + changeIdProcessingParms.OldKeyValues[0].ToSql() + " ORDER BY xazSupplierOrganizationID, xazSequence", fillSchema: false, out adapter, changeIdProcessingParms.SqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		int num = -1;
		bool flag = false;
		DataTable dataTable2 = changeIdProcessingParms.Database.GetDataTable("SELECT * FROM ExpenseAccountSplits WHERE xazSupplierOrganizationID = " + changeIdProcessingParms.NewKeyValues[0].ToSql() + " ORDER BY xazSupplierOrganizationID, xazSequence", fillSchema: false, out adapter, changeIdProcessingParms.SqlTransaction);
		foreach (DataRow row in dataTable.Rows)
		{
			if (num == -1)
			{
				num = 0;
				if (dataTable2.Rows.Count > 0)
				{
					flag = true;
					num = Convert.ToInt16(dataTable2.Rows[dataTable2.Rows.Count - 1]["xazSequence"]);
				}
				else
				{
					flag = false;
				}
			}
			if (dataTable2.Select("xazExpenseGLAccountID = " + row["xazExpenseGLAccountID"].ToLinq(), "xazSupplierOrganizationID, xazSequence").Length != 0)
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
			dataRow2["xazSequence"] = num;
			dataRow2["xazSupplierOrganizationID"] = changeIdProcessingParms.NewKeyValues[0];
			if (flag)
			{
				dataRow2["xazPercent"] = 0;
			}
			dataRow2.EndEdit();
			dataTable2.Rows.Add(dataRow2);
		}
		changeIdProcessingParms.Database.ExecuteCommand("DELETE FROM ExpenseAccountSplits WHERE xazSupplierOrganizationID = " + changeIdProcessingParms.OldKeyValues[0].ToSql(), changeIdProcessingParms.SqlTransaction);
		changeIdProcessingParms.Database.UpdateData(dataTable2, adapter, changeIdProcessingParms.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		parm.Database.ExecuteCommand("Update Organizations Set cmoName = dest.cmlName, cmoAddressLine1 = dest.cmlAddressLine1, cmoAddressLine2 = dest.cmlAddressLine2, cmoAddressLine3 = dest.cmlAddressLine3, cmoCity = dest.cmlCity, cmoState = dest.cmlState, cmoCountry = dest.cmlCountry, cmoPostCode = dest.cmlPostCode, cmoPhoneNumber = dest.cmlPhoneNumber, cmoAlternatePhoneNumber = dest.cmlAlternatePhoneNumber, cmoFaxNumber = dest.cmlFaxNumber, cmoQuoteContactID = dest.cmlQuoteContactID, cmoShipContactID = dest.cmlShipContactID, cmoARInvoiceContactID = dest.cmlARInvoiceContactID,cmoPurchaseContactID = dest.cmlPurchaseContactID, cmoAPInvoiceContactID = dest.cmlAPInvoiceContactID,cmoCustomerTaxable = dest.cmlCustomerTaxable, cmoCustomerTaxCodeID = dest.cmlCustomerTaxCodeID, cmoCustomerShippingMethodID = dest.cmlCustomerShippingMethodID, cmoCustomerShipPaymentTypeID = dest.cmlCustomerShipPaymentTypeID,cmoCustomerPaymentTermsID = dest.cmlCustomerPaymentTermID, cmoARInvoicePerShipmentLine = dest.cmlARInvoicePerShipmentLine, cmoSupplierShippingMethodID = dest.cmlSupplierShippingMethodID, cmoSupplierPaymentTermID = dest.cmlSupplierPaymentTermID, cmoCurrencyRateID = dest.cmlCurrencyRateID, cmoCreditHold = dest.cmlCreditHold, cmoCustomerCreditLimit = dest.cmlCustomerCreditLimit, cmoTaxExemptNumber = dest.cmlTaxExemptNumber, cmoNonTaxReasonID = dest.cmlNonTaxReasonID, cmoEMailAddress = dest.cmlEMailAddress From Organizations Inner Join OrganizationLocations dest On cmlOrganizationID = cmoOrganizationID And cmlLocationID = '' Where cmoOrganizationID = " + parm.NewKeyValues[0].ToSql(), parm.SqlTransaction);
	}
}
