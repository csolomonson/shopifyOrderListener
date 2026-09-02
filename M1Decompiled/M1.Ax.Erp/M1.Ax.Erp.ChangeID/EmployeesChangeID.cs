using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("Employees")]
public class EmployeesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		parm.DeleteStatements.AppendLine("DELETE FROM EmployeePersonalData WHERE lmdEmployeeID = " + parm.OldKeyValues[0].ToSql());
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.ChangeIDType == 1)
		{
			string s = parm.OldKeyValues[0].ToString();
			string text = parm.NewKeyValues[0].ToString();
			M1Database database = parm.Database;
			SqlTransaction sqlTransaction = parm.SqlTransaction;
			bool num = IsFilledPreviousEmployeeID(database, sqlTransaction, text);
			bool flag = StpEmployeeHasSubmittedSession(database, text);
			if (!num && flag)
			{
				UpdatePreviousEmployeeID(database, sqlTransaction, s.ToSql(), text.ToSql());
			}
		}
	}

	private void UpdatePreviousEmployeeID(M1Database database, SqlTransaction sqlTransaction, string oldEmployeeId, string newEmployeeId)
	{
		string queryString = "UPDATE Employees SET lmePreviousEmployeeID = " + oldEmployeeId + " WHERE lmeEmployeeID = " + newEmployeeId;
		database.ExecuteCommand(queryString, sqlTransaction);
	}

	private bool StpEmployeeHasSubmittedSession(M1Database database, string employeeId)
	{
		return new SingleTouchPayroll().StpEmployeeHasSubmittedSession(database, employeeId);
	}

	private bool IsFilledPreviousEmployeeID(M1Database database, SqlTransaction sqlTransaction, string employeeId)
	{
		using SqlCommand sqlCommand = new SqlCommand("SELECT lmePreviousEmployeeID FROM Employees WHERE lmeEmployeeID = @EmployeeId");
		sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
		DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
		if (dataTable.Rows.Count != 0 && !string.IsNullOrEmpty(dataTable.Rows[0].Field<string>("lmePreviousEmployeeID")))
		{
			return true;
		}
		return false;
	}
}
