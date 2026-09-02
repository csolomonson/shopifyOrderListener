using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.SaveAs;

[SaveAsProcessing("APInvoiceLines")]
public class APInvoiceLinesSaveAs : ISaveAsProcessing
{
	public void BeforeUpdate(SaveAsProcessingParms parm)
	{
		if (!parm.Table.Equals("APInvoiceLines", StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		using SqlCommand sqlCommand = new SqlCommand("SELECT appAPInvoiceID, appPostedToGL FROM APInvoices WHERE appAPInvoiceID = @InvID AND appPostedToGL <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@InvID", SqlDbType.NVarChar)).Value = parm.NewKeyValues[0];
		if (parm.Database.GetDataTable(sqlCommand).Rows.Count > 0)
		{
			throw new M1Exception("You may not add an invoice line to an invoice that has been posted to the General Ledger.");
		}
	}

	public void AfterUpdate(SaveAsProcessingParms parm)
	{
	}
}
