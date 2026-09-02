using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class Lead
{
	public string CreateLead(M1Database database, string customerID, string locationID, string currencyID, string partID, string revisionID, decimal leadQty = 1m)
	{
		using M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "Leads", null, true);
		DataRow dataRow = m1BindingSource.AddNew() as DataRow;
		m1BindingSource.SetKeyToNextAvailable(dataRow);
		dataRow.SetField("lopCustomerOrganizationID", customerID);
		dataRow.SetField("lopLocationID", locationID);
		if (!string.IsNullOrWhiteSpace(currencyID))
		{
			dataRow.SetField("lopCurrencyRateID", currencyID);
		}
		if (!string.IsNullOrWhiteSpace(partID))
		{
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("LeadLines");
			DataRow dataRow2 = childBindingSource.AddNew() as DataRow;
			childBindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2.SetField("lolPartID", partID);
			dataRow2.SetField("lolPartRevisionID", revisionID);
			if (string.IsNullOrWhiteSpace(dataRow2.Field<string>("lolDescription")))
			{
				dataRow2.SetField("lolDescription", partID);
			}
			dataRow2.SetField("lolQuantity", leadQty);
		}
		m1BindingSource.SaveData();
		return dataRow.Field<string>("lopLeadID");
	}

	public void SetSalesPeople(M1Database database, M1BindingSource bsQuote, string orgID, string locationID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select cmkSalesEmployeeID,cmkPercent from OrganizationLocSalespeople where cmkOrganizationID = @OrgID And cmkLocationID = @LocID Order By cmkSequenceID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = locationID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = bsQuote.PrimaryTable.GetChildBindingSource("LeadSalesPeople");
		if (childBindingSource.Count != 0)
		{
			childBindingSource.RemoveWhere(string.Empty);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow obj = (DataRow)childBindingSource.AddNew();
			obj["lojSalesEmployeeID"] = row["cmkSalesEmployeeID"];
			obj["lojPercent"] = row["cmkPercent"];
		}
	}
}
