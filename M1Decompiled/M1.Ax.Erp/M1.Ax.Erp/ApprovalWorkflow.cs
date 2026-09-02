using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public static class ApprovalWorkflow
{
	public static ApprovalDefinition GetApprovalDefinition(string table)
	{
		if (table.Equals("SalesOrders", StringComparison.CurrentCultureIgnoreCase))
		{
			return new SalesOrderApprovalDefinition();
		}
		if (table.Equals("PurchaseOrders", StringComparison.CurrentCultureIgnoreCase))
		{
			return new PurchaseOrderApprovalDefinition();
		}
		if (table.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase))
		{
			return new InspectionLineApprovalDefinition();
		}
		return null;
	}

	public static bool IsApprovalRequired(ApprovalDefinition definition, M1BindingSource bindingSource)
	{
		string text = GetRequesterEmployeeID(definition, bindingSource);
		M1Database currentDatabase = bindingSource.CurrentDatabase;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = getEmployeeForUserID(currentDatabase, currentDatabase.User.ID, bindingSource.Transaction);
		}
		decimal employeeApprovalAmount = GetEmployeeApprovalAmount(currentDatabase, text, definition);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (string.IsNullOrWhiteSpace(definition.ParentTotalField))
		{
			SqlCommand sqlCommand = currentDatabase.NewSqlCommand("Select Count(*) From " + definition.ApprovalSourceTable + " Where " + definition.ApprovalSourceKeyFields[0] + " = @EmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = text;
			if (Convert.ToInt32(currentDatabase.ExecuteScalar(sqlCommand, bindingSource.Transaction)) > 0)
			{
				return true;
			}
			return false;
		}
		if (employeeApprovalAmount != 0m && employeeApprovalAmount < currentAsDataRow.Field<decimal>(definition.ParentTotalField))
		{
			return true;
		}
		return false;
	}

	public static void CheckApprovals(ApprovalDefinition definition, M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		M1Database currentDatabase = bindingSource.CurrentDatabase;
		if (IsApprovalRequired(definition, bindingSource))
		{
			currentAsDataRow.SetField(definition.ParentStatusField, (byte)1);
			if (!string.IsNullOrWhiteSpace(definition.ParentReadyToPrintField))
			{
				currentAsDataRow.SetField(definition.ParentReadyToPrintField, value: false);
			}
			currentAsDataRow.SetField<DateTime?>(definition.ParentDecisionDateField, null);
		}
		else if (currentAsDataRow.Field<byte>(definition.ParentStatusField) == 2)
		{
			currentAsDataRow.SetField(definition.ParentStatusField, (byte)3);
		}
		currentAsDataRow.SetField(definition.ParentNextApprovalEmployeeIDField, string.Empty);
		currentAsDataRow.SetField<DateTime?>(definition.ParentApprovalRequestDateField, null);
		deleteApprovals(currentDatabase, definition, bindingSource);
	}

	private static object[] getKeys(DataRow row, string[] fields)
	{
		List<object> list = new List<object>();
		foreach (string columnName in fields)
		{
			list.Add(row[columnName]);
		}
		return list.ToArray();
	}

	private static string getEmployeeForUserID(M1Database database, string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select TOP 1 IsNull(lmeEmployeeID,'') from Employees where lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
		return Convert.ToString(database.ExecuteScalar(sqlCommand, transaction));
	}

	public static string GetEmployeeName(M1Database database, string employeeID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select IsNull(lmeEmployeeName,'') from Employees where lmeEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeID;
		return Convert.ToString(database.ExecuteScalar(sqlCommand, transaction));
	}

	public static string GetRequesterEmployeeID(ApprovalDefinition definition, M1BindingSource parentBindingSource)
	{
		if (!string.IsNullOrWhiteSpace(definition.ParentEmployeeIDField))
		{
			if (string.IsNullOrWhiteSpace(definition.ParentEmployeeChildTable))
			{
				return parentBindingSource.CurrentAsDataRow.Field<string>(definition.ParentEmployeeIDField);
			}
			DataView dataView = parentBindingSource.PrimaryTable.GetChildBindingSource(definition.ParentEmployeeChildTable).GetDataView();
			if (dataView.Count != 0)
			{
				return dataView[0].Row.Field<string>(definition.ParentEmployeeIDField);
			}
		}
		return string.Empty;
	}

	public static void TransferApprovals(ApprovalDefinition definition, M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (M1Util.IsNullOrEmpty(getKeys(currentAsDataRow, definition.ParentTableKeys)[0]))
		{
			return;
		}
		M1Database currentDatabase = bindingSource.CurrentDatabase;
		string text = GetRequesterEmployeeID(definition, bindingSource);
		if (string.IsNullOrWhiteSpace(text))
		{
			text = getEmployeeForUserID(currentDatabase, currentDatabase.User.ID, bindingSource.Transaction);
		}
		decimal num = default(decimal);
		if (!string.IsNullOrWhiteSpace(definition.ParentTotalField))
		{
			num = currentAsDataRow.Field<decimal>(definition.ParentTotalField);
		}
		M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource(definition.ApprovalInstanceTable);
		childBindingSource.RemoveWhere(string.Empty);
		if (string.IsNullOrWhiteSpace(text) || (!(GetEmployeeApprovalAmount(currentDatabase, text, definition) < num) && !string.IsNullOrWhiteSpace(definition.ParentTotalField)))
		{
			return;
		}
		Dictionary<string, ApprovalEmployee> dictionary = new Dictionary<string, ApprovalEmployee>(StringComparer.CurrentCultureIgnoreCase);
		getEmployeeForUserID(currentDatabase, currentDatabase.User.ID, bindingSource.Transaction);
		transferApprovalsNextLevel(currentDatabase, num, text, dictionary, definition, bindingSource.Transaction);
		int num2 = 0;
		foreach (KeyValuePair<string, ApprovalEmployee> item in dictionary)
		{
			DataRow obj = (DataRow)childBindingSource.AddNew();
			num2++;
			obj[definition.ApprovalInstanceKeyFields[definition.ApprovalInstanceKeyFields.Length - 1]] = num2;
			obj[definition.InstanceEmployeeIDField] = item.Value.EmployeeID;
			obj[definition.InstanceStatusField] = item.Value.Status;
		}
	}

	public static void TransferApprovalsDirect(M1Database database, ApprovalDefinition definition, DataRow row, SqlTransaction transaction)
	{
		object[] keys = getKeys(row, definition.ParentTableKeys);
		if (M1Util.IsNullOrEmpty(keys[0]))
		{
			return;
		}
		string text = ((!string.IsNullOrWhiteSpace(definition.ParentEmployeeIDField) && row.Table.Columns.Contains(definition.ParentEmployeeIDField)) ? row.Field<string>(definition.ParentEmployeeIDField) : getEmployeeForUserID(database, database.User.ID, transaction));
		decimal num = (string.IsNullOrWhiteSpace(definition.ParentTotalField) ? 100m : row.Field<decimal>(definition.ParentTotalField));
		deleteApprovalsDirect(database, definition, keys, transaction);
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		decimal employeeApprovalAmount = GetEmployeeApprovalAmount(database, text, definition);
		if (!(employeeApprovalAmount < num))
		{
			return;
		}
		Dictionary<string, ApprovalEmployee> dictionary = new Dictionary<string, ApprovalEmployee>(StringComparer.CurrentCultureIgnoreCase);
		string employeeForUserID = getEmployeeForUserID(database, database.User.ID, transaction);
		if (!text.Equals(employeeForUserID, StringComparison.CurrentCultureIgnoreCase) && employeeApprovalAmount != 0m)
		{
			dictionary.Add(text, new ApprovalEmployee(text, 1, employeeApprovalAmount));
		}
		transferApprovalsNextLevel(database, num, text, dictionary, definition, transaction);
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("select * from " + definition.ApprovalInstanceTable + " where 0=1", fillSchema: false, out adapter, transaction);
		int num2 = 0;
		foreach (KeyValuePair<string, ApprovalEmployee> item in dictionary)
		{
			DataRow dataRow = dataTable.NewRow().BlankRow();
			for (int i = 0; i < keys.Length; i++)
			{
				dataRow[definition.ApprovalInstanceKeyFields[i]] = keys[i];
			}
			num2++;
			dataRow[definition.ApprovalInstanceKeyFields[definition.ApprovalInstanceKeyFields.Length - 1]] = num2;
			dataRow[definition.InstanceEmployeeIDField] = item.Value.EmployeeID;
			dataRow[definition.InstanceStatusField] = item.Value.Status;
			dataTable.Rows.Add(dataRow);
		}
		database.UpdateData(dataTable, adapter, transaction);
	}

	public static decimal GetEmployeeApprovalAmount(M1Database database, string employeeID, ApprovalDefinition definition)
	{
		if (string.IsNullOrWhiteSpace(definition.SourceAmountField) || string.IsNullOrWhiteSpace(employeeID))
		{
			return 0m;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select IsNull(" + definition.SourceAmountField + ",0) From Employees Where lmeEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeID;
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand));
	}

	private static void transferApprovalsNextLevel(M1Database database, decimal orderTotal, string parentEmployeeID, Dictionary<string, ApprovalEmployee> approvalList, ApprovalDefinition definition, SqlTransaction transaction)
	{
		List<string> list = new List<string>();
		string text = definition.ApprovalSourceKeyFields[definition.ApprovalSourceKeyFields.Length - 1];
		if (string.IsNullOrWhiteSpace(definition.SourceAmountField))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select " + definition.ApprovalSourceEmployeeIDField + " From " + definition.ApprovalSourceTable + " Inner join Employees On " + definition.ApprovalSourceEmployeeIDField + " = lmeEmployeeID Where " + definition.ApprovalSourceKeyFields[0] + " = @p1 order by " + text);
			sqlCommand.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = parentEmployeeID;
			foreach (DataRow row2 in database.GetDataTable(sqlCommand, transaction).Rows)
			{
				string text2 = row2.Field<string>(definition.ApprovalSourceEmployeeIDField);
				if (!approvalList.ContainsKey(text2))
				{
					approvalList.Add(text2, new ApprovalEmployee(text2, 2, 0m));
					if (!list.Contains(text2, StringComparer.CurrentCultureIgnoreCase))
					{
						list.Add(text2);
					}
				}
			}
		}
		else
		{
			decimal num = -1m;
			SqlCommand sqlCommand2 = database.NewSqlCommand("select " + definition.ApprovalSourceEmployeeIDField + ",Case When " + definition.SourceAmountField + " = 0 Then 999999999.99 Else " + definition.SourceAmountField + " End As " + definition.SourceAmountField + " From " + definition.ApprovalSourceTable + " Inner join Employees On " + definition.ApprovalSourceEmployeeIDField + " = lmeEmployeeID Where " + definition.ApprovalSourceKeyFields[0] + " = @p1 order by " + definition.SourceAmountField + "," + text);
			sqlCommand2.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = parentEmployeeID;
			foreach (DataRow row3 in database.GetDataTable(sqlCommand2, transaction).Rows)
			{
				if (row3.Field<decimal>(definition.SourceAmountField) != num && num > orderTotal)
				{
					break;
				}
				string text3 = row3.Field<string>(definition.ApprovalSourceEmployeeIDField);
				if (!approvalList.ContainsKey(text3))
				{
					if (row3.Field<decimal>(definition.SourceAmountField) >= orderTotal)
					{
						approvalList.Add(text3, new ApprovalEmployee(text3, 2, row3.Field<decimal>(definition.SourceAmountField)));
					}
					if (!list.Contains(text3, StringComparer.CurrentCultureIgnoreCase))
					{
						list.Add(text3);
					}
				}
				num = row3.Field<decimal>(definition.SourceAmountField);
			}
		}
		foreach (string item in list)
		{
			transferApprovalsNextLevel(database, orderTotal, item, approvalList, definition, transaction);
		}
	}

	private static void deleteApprovals(M1Database database, ApprovalDefinition definition, M1BindingSource parentBs)
	{
		parentBs.PrimaryTable.GetChildBindingSource(definition.ApprovalInstanceTable).RemoveWhere(string.Empty);
	}

	private static void deleteApprovalsDirect(M1Database database, ApprovalDefinition definition, object[] approvalInstanceParentKeys, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand(string.Empty);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < approvalInstanceParentKeys.Length; i++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(definition.ApprovalInstanceKeyFields[i] + " = @p" + i);
			sqlCommand.Parameters.Add(new SqlParameter("@p" + i, approvalInstanceParentKeys[i]));
		}
		sqlCommand.CommandText = "Delete From " + definition.ApprovalInstanceTable + " Where " + stringBuilder.ToString();
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public static string FormatTextForEmail(ApprovalDefinition definition, M1BindingSource bindingSource, string formName, string requesterID)
	{
		M1Database currentDatabase = bindingSource.CurrentDatabase;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		SqlTransaction transaction = bindingSource.Transaction;
		string text = currentAsDataRow[definition.ParentTableKeys[0]].ToString();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<HTML><BODY><FONT FACE=\"Tahoma\" SIZE=2>");
		stringBuilder.Append(formName + " Info:<BR><BR>");
		stringBuilder.Append(formName + ": <A TARGET=\"_parent\" HREF=\"M1:Object:" + definition.ParentFormCollectionID + ":" + currentDatabase.ID + "\\" + text + "\">" + text + "</A><BR>");
		if (!string.IsNullOrWhiteSpace(requesterID))
		{
			string employeeName = GetEmployeeName(currentDatabase, requesterID, transaction);
			stringBuilder.Append("Requested By: <A HREF=\"M1:Object:Employee:" + currentDatabase.ID + "\\" + requesterID + "\">" + employeeName + "</A><BR>");
		}
		if (!string.IsNullOrWhiteSpace(definition.ParentTotalField))
		{
			stringBuilder.Append("Total: " + currentAsDataRow.Field<decimal>(definition.ParentTotalField).ToString("C") + "<BR>");
		}
		if (!currentAsDataRow.IsNull(definition.ParentApprovalRequestDateField))
		{
			stringBuilder.Append("Requested: " + currentAsDataRow.Field<DateTime>(definition.ParentApprovalRequestDateField).ToString("G") + "<BR>");
		}
		DataView dataView = bindingSource.PrimaryTable.GetChildBindingSource(definition.ApprovalInstanceTable).GetDataView();
		if (dataView.Count != 0)
		{
			stringBuilder.Append("<BR>Approval List<BR>");
			foreach (DataRowView item in dataView)
			{
				string employeeName2 = GetEmployeeName(currentDatabase, item.Row.Field<string>(definition.InstanceEmployeeIDField), transaction);
				stringBuilder.Append(employeeName2 + ":");
				switch (item.Row.Field<ApprovalStatus>(definition.InstanceStatusField))
				{
				case ApprovalStatus.RequiresApproval:
					stringBuilder.Append("Requires Approval");
					break;
				case ApprovalStatus.ApprovalRequested:
					stringBuilder.Append("Approval Requested");
					break;
				case ApprovalStatus.Approved:
					stringBuilder.Append("Approved");
					if (!item.Row.IsNull(definition.InstanceStatusDateField))
					{
						stringBuilder.Append(" - " + item.Row.Field<DateTime>(definition.InstanceStatusDateField).ToString("G"));
					}
					break;
				case ApprovalStatus.Rejected:
					stringBuilder.Append("Rejected");
					break;
				}
				if (!string.IsNullOrWhiteSpace(item.Row.Field<string>(definition.InstanceDescriptionField)))
				{
					stringBuilder.Append(" (" + item.Row.Field<string>(definition.InstanceDescriptionField) + ")");
				}
				stringBuilder.Append("<BR>");
			}
			stringBuilder.Append("<BR>");
		}
		stringBuilder.Append("</BODY></HTML>");
		return stringBuilder.ToString();
	}

	public static string FormatTextForEmailDirect(M1Database database, DataRow row, ApprovalDefinition definition, SqlTransaction transaction, string formName, string requesterID)
	{
		string text = row[definition.ParentTableKeys[0]].ToString();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<HTML><BODY><FONT FACE=\"Tahoma\" SIZE=2>");
		stringBuilder.Append(formName + " Info:<BR><BR>");
		stringBuilder.Append(formName + ": <A TARGET=\"_parent\" HREF=\"M1:Object:" + definition.ParentFormCollectionID + ":" + database.ID + "\\" + text + "\">" + text + "</A><BR>");
		if (!string.IsNullOrWhiteSpace(requesterID))
		{
			string employeeName = GetEmployeeName(database, requesterID, transaction);
			stringBuilder.Append("Requested By: <A HREF=\"M1:Object:Employee:" + database.ID + "\\" + requesterID + "\">" + employeeName + "</A><BR>");
		}
		if (!string.IsNullOrWhiteSpace(definition.ParentTotalField))
		{
			stringBuilder.Append("Total: " + row.Field<decimal>(definition.ParentTotalField).ToString("C") + "<BR>");
		}
		if (!row.IsNull(definition.ParentApprovalRequestDateField))
		{
			stringBuilder.Append("Requested: " + row.Field<DateTime>(definition.ParentApprovalRequestDateField).ToString("G") + "<BR>");
		}
		SqlCommand sqlCommand = database.NewSqlCommand(string.Empty);
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int i = 0; i < definition.ParentTableKeys.Length; i++)
		{
			if (stringBuilder2.Length != 0)
			{
				stringBuilder2.Append(" And ");
			}
			stringBuilder2.Append(definition.ApprovalInstanceKeyFields[i] + " = @p" + i);
			sqlCommand.Parameters.Add(new SqlParameter("@p" + i, row[definition.ParentTableKeys[i]]));
		}
		sqlCommand.CommandText = "SELECT " + definition.InstanceStatusField + "," + definition.InstanceStatusDateField + "," + definition.InstanceDescriptionField + ",lmeEmployeeName FROM " + definition.ApprovalInstanceTable + " Inner Join Employees On " + definition.InstanceEmployeeIDField + " = lmeEmployeeID WHERE " + stringBuilder2.ToString() + " ORDER BY " + definition.ApprovalInstanceKeyFields[definition.ApprovalInstanceKeyFields.Length - 1];
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			stringBuilder.Append("<BR>Approval List<BR>");
			foreach (DataRow row2 in dataTable.Rows)
			{
				stringBuilder.Append(row2.Field<string>("lmeEmployeeName") + ":");
				switch (row2.Field<ApprovalStatus>(definition.InstanceStatusField))
				{
				case ApprovalStatus.RequiresApproval:
					stringBuilder.Append("Requires Approval");
					break;
				case ApprovalStatus.ApprovalRequested:
					stringBuilder.Append("Approval Requested");
					break;
				case ApprovalStatus.Approved:
					stringBuilder.Append("Approved");
					if (!row2.IsNull(definition.InstanceStatusDateField))
					{
						stringBuilder.Append(" - " + row2.Field<DateTime>(definition.InstanceStatusDateField).ToString("G"));
					}
					break;
				case ApprovalStatus.Rejected:
					stringBuilder.Append("Rejected");
					break;
				}
				if (!string.IsNullOrWhiteSpace(row2.Field<string>(definition.InstanceDescriptionField)))
				{
					stringBuilder.Append("(" + row2.Field<string>(definition.InstanceDescriptionField) + ")");
				}
				stringBuilder.Append("<BR>");
			}
			stringBuilder.Append("<BR>");
		}
		stringBuilder.Append("</BODY></HTML>");
		return stringBuilder.ToString();
	}
}
