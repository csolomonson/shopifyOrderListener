using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.SaveAs;

[SaveAsProcessing("ARInvoices")]
public class ARInvoicesSaveAs : ISaveAsProcessing
{
	public void BeforeUpdate(SaveAsProcessingParms parm)
	{
		if (!parm.Table.Equals("ARInvoices", StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		using SqlCommand sqlCommand = new SqlCommand("SELECT arpARInvoiceID, arpOpenInvoiceLoad FROM ARInvoices WHERE arpARInvoiceID = @InvID AND arpOpenInvoiceLoad <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@InvID", SqlDbType.NVarChar)).Value = parm.OldKeyValues[0];
		if (parm.Database.GetDataTable(sqlCommand).Rows.Count > 0)
		{
			throw new M1Exception("You may not use the Save as function on an invoice created through Open Invoice Load.");
		}
	}

	public void AfterUpdate(SaveAsProcessingParms parm)
	{
		if (parm != null && !string.IsNullOrWhiteSpace(parm.NewKeyValues[0].ToString()))
		{
			using (SqlCommand sqlCommand = new SqlCommand("UPDATE ARInvoices SET arpInvoicePaidBase=0,arpInvoicePaidForeign=0 WHERE arpARInvoiceID = @InvID"))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@InvID", SqlDbType.NVarChar)).Value = parm.NewKeyValues[0];
				parm.Database.ExecuteCommand(sqlCommand);
			}
		}
	}
}
