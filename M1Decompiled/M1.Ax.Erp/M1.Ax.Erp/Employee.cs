using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Employee
{
	public class EmployeeMessageResult
	{
		public int SentCount;

		public int NotSentCount;

		public string NotSentEmployees = string.Empty;

		public string NotSentMessage = string.Empty;

		public EmployeeMessageResult(int sentCount, int notSentCount, string notSentEmployees)
		{
			SentCount = sentCount;
			NotSentCount = notSentCount;
			NotSentEmployees = notSentEmployees;
			if (NotSentEmployees.Length != 0)
			{
				NotSentMessage = "The following employees could not be found in the Employees table:\r" + notSentEmployees;
			}
		}
	}

	public EmployeeMessageResult SendEmployeeMessage(M1Database m1database, string cToList, string cSubject, string cMessageText, string cMessageRTF = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		cToList = cToList.Trim();
		if (cToList.Length != 0)
		{
			if (string.IsNullOrWhiteSpace(cMessageRTF))
			{
				cMessageRTF = cMessageText;
			}
			string employeeIDforUserId = GetEmployeeIDforUserId(m1database, m1database.User.ID);
			SqlCommand sqlCommand = m1database.NewSqlCommand("Select IsNull(Count(*),0) From Employees Where lmeEmployeeID = @EmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar));
			SqlCommand sqlCommand2 = m1database.NewSqlCommand("select IsNull(max(lmmEmployeeMessageID),0) as lmmEmployeeMessageID from EmployeeMessages where lmmEmployeeID = @EmployeeID");
			sqlCommand2.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar));
			string[] array = cToList.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				int num3 = text.IndexOf('[');
				if (num3 != -1)
				{
					text = text.Substring(num3 + 1);
					num3 = text.IndexOf(']');
					if (num3 != -1)
					{
						text = text.Substring(0, num3);
					}
				}
				if (text.Length == 0)
				{
					continue;
				}
				int num4 = 1;
				sqlCommand.Parameters["@EmployeeID"].Value = text;
				bool flag;
				if ((int)m1database.ExecuteScalar(sqlCommand) > 0)
				{
					flag = true;
				}
				else
				{
					flag = false;
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append("; ");
					}
					stringBuilder.Append(text);
					num++;
				}
				if (flag)
				{
					sqlCommand2.Parameters["@EmployeeID"].Value = text;
					DataTable dataTable = m1database.GetDataTable(sqlCommand2);
					if (dataTable.Rows.Count != 0)
					{
						num4 = Convert.ToInt32(dataTable.Rows[0]["lmmEmployeeMessageID"]) + 1;
					}
					SqlDataAdapter adapter;
					DataRow dataRow = m1database.GetDataTable("Select * From EmployeeMessages Where 0=1", fillSchema: true, out adapter).AddBlankRow();
					dataRow["lmmEmployeeID"] = text.Trim();
					dataRow["lmmEmployeeMessageID"] = num4;
					dataRow["lmmSubject"] = cSubject;
					dataRow["lmmBodyRTF"] = cMessageRTF;
					dataRow["lmmBodyText"] = cMessageText;
					dataRow["lmmStatus"] = 1;
					dataRow["lmmSenderEmployeeID"] = employeeIDforUserId;
					dataRow["lmmSentDate"] = DateTime.Now;
					m1database.UpdateData(new DataRow[1] { dataRow }, adapter);
					num2++;
				}
			}
			if (stringBuilder.Length != 0)
			{
				throw new M1Exception("The following employees could not be found in the Employees table:\r" + stringBuilder.ToString());
			}
		}
		return new EmployeeMessageResult(num2, num, stringBuilder.ToString());
	}

	public string GetEmployeeIDforUserId(M1Database database, string userId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 lmeEmployeeID from Employees where lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userId;
		string text = Convert.ToString(database.ExecuteScalar(sqlCommand));
		if (text != null)
		{
			return text.Trim();
		}
		return string.Empty;
	}

	public void CopyEmployeeTaxes(M1Database database, string fromEmployeeID, string toEmployeeID)
	{
		if (string.IsNullOrWhiteSpace(fromEmployeeID))
		{
			throw new M1Exception("From Employee ID is required");
		}
		if (string.IsNullOrWhiteSpace(toEmployeeID))
		{
			throw new M1Exception("To Employee ID is required");
		}
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Delete From EmployeeIncomeTaxes Where pamEmployeeID = @EmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = toEmployeeID;
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = database.NewSqlCommand("Insert Into EmployeeIncomeTaxes (pamEmployeeID,pamEmployeeIncomeTaxID,pamIncomeTaxID,pamIncomeTaxTypeID,pamIncomeTaxTableID,pamPersonalExemptions,pamDependentExemptions,pamAdditionalTaxAmount,pamExpenseGLAccountID,pamInactive,pamInactiveDate) Select @ToEmployeeID As pamEmployeeID,pamEmployeeIncomeTaxID,pamIncomeTaxID,pamIncomeTaxTypeID,pamIncomeTaxTableID,0 As pamPersonalExemptions,0 As pamDependentExemptions,pamAdditionalTaxAmount,pamExpenseGLAccountID,pamInactive,pamInactiveDate From EmployeeIncomeTaxes Where pamEmployeeID = @FromEmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@ToEmployeeID", SqlDbType.NVarChar)).Value = toEmployeeID;
			sqlCommand.Parameters.Add(new SqlParameter("@FromEmployeeID", SqlDbType.NVarChar)).Value = fromEmployeeID;
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
		}
	}
}
