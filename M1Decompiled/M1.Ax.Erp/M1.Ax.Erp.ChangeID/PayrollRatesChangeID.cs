using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("PayrollRates")]
public class PayrollRatesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		string empty = string.Empty;
		SqlDataAdapter adapter;
		DataTable dataTable = parm.Database.GetDataTable("SELECT New.paqPayrollRateID AS paqPayrollRateIDNew, Prev.paqPayrollRateID AS paqPayrollRateIDPrev, Prev.paqExpenseID AS paqExpenseIDPrev, Prev.paqGLAccountID AS paqGLAccountIDPrev, New.paqExpenseID AS paqExpenseIDNew, New.paqGLAccountID AS paqGLAccountIDNew FROM PayrollRateExpenseLinks AS Prev, PayrollRateExpenseLinks AS New WHERE Prev.paqPayrollRateID = " + parm.OldKeyValues[parm.OldKeyValues.GetLowerBound(0)].ToSql() + " AND New.paqPayrollRateID = " + parm.NewKeyValues[parm.NewKeyValues.GetLowerBound(0)].ToSql() + " AND Prev.paqExpenseID = New.paqExpenseID", fillSchema: false, out adapter, parm.SqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (row["paqGLAccountIDPrev"] != row["paqGLAccountIDNew"])
			{
				empty = string.Empty;
				if (parm.ChangeIDType == 2)
				{
					empty = row["paqGLAccountIDPrev"].ToString();
				}
				else if (parm.ChangeIDType == 3)
				{
					empty = row["paqGLAccountIDNew"].ToString();
				}
				parm.Database.ExecuteCommand("UPDATE PAYROLLRATEEXPENSELINKS SET paqGLAccountID = " + empty.ToSql() + " WHERE paqPayrollRateID = " + row["paqPayrollRateIDPrev"].ToSql() + " AND paqExpenseID = " + row["paqExpenseIDPrev"].ToSql(), parm.SqlTransaction);
			}
		}
		parm.Database.ExecuteCommand("Delete From PAYROLLRATEEXPENSELINKS Where paqPayrollRateID = " + parm.NewKeyValues[parm.NewKeyValues.GetLowerBound(0)].ToSql(), parm.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
