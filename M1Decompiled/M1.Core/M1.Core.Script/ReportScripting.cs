using System;
using System.Data;
using System.Data.OleDb;
using ADODB;

namespace M1.Core.Script;

public class ReportScripting : ScriptingBase
{
	public ReportScripting(IServiceProvider provider)
		: base(provider)
	{
	}

	public object ExecuteReportCodeRs(string code)
	{
		try
		{
			bool flag = isExecuting;
			LoadEnvironment();
			AddCode("Dim ReportData");
			isExecuting = true;
			base.ExecuteStatement(code);
			isExecuting = flag;
			return base.Eval("ReportData");
		}
		finally
		{
			ResetEnvironment();
		}
	}

	public DataTable ExecuteReportCodeDT(string code)
	{
		try
		{
			bool flag = isExecuting;
			LoadEnvironment();
			AddCode("Dim ReportData");
			isExecuting = true;
			base.ExecuteStatement(code);
			isExecuting = flag;
			object obj = base.Eval("ReportData");
			if (obj.GetType() == typeof(DataTable))
			{
				return (DataTable)obj;
			}
			Recordset recordset = (Recordset)obj;
			DataTable dataTable = new DataTable();
			new OleDbDataAdapter().Fill(dataTable, recordset);
			recordset.Close();
			return dataTable;
		}
		finally
		{
			ResetEnvironment();
		}
	}
}
