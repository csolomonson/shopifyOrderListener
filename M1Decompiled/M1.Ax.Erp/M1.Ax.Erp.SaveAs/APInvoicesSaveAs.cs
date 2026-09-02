using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.SaveAs;

[SaveAsProcessing("APInvoices")]
public class APInvoicesSaveAs : ISaveAsProcessing
{
	public void BeforeUpdate(SaveAsProcessingParms parm)
	{
		if (!parm.Table.Equals("APInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		using SqlCommand sqlCommand = new SqlCommand("SELECT appAPInvoiceID, appOpenInvoiceLoad FROM APInvoices WHERE appAPInvoiceID = @InvID AND appOpenInvoiceLoad <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@InvID", SqlDbType.NVarChar)).Value = parm.OldKeyValues[0];
		if (parm.Database.GetDataTable(sqlCommand).Rows.Count > 0)
		{
			throw new M1Exception("You may not use the Save as function on an invoice created through Open Invoice Load.");
		}
	}

	public void AfterUpdate(SaveAsProcessingParms parm)
	{
	}
}
