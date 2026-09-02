using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

public class v800240e
{
	public v800240e(DBConversionParms parms)
	{
		string text = "IMPLCHECKL";
		int newWorkFlowId = 10000;
		int num = 0;
		_ = string.Empty;
		SqlCommand sqlCommand = new SqlCommand();
		Dictionary<string, string> workFlowsToUpdate = GetWorkFlowsToUpdate(parms.ServerManager, newWorkFlowId, parms.User, parms.DataDictionary, parms.DatabaseName);
		Dictionary<string, string> workFlowTablesByWorkFlowID = GetWorkFlowTablesByWorkFlowID(parms.ServerManager, text, parms.User, parms.DataDictionary, parms.DatabaseName);
		if (workFlowsToUpdate.Count() > 0)
		{
			string empty = string.Empty;
			foreach (KeyValuePair<string, string> item in workFlowsToUpdate)
			{
				foreach (KeyValuePair<string, string> item2 in workFlowTablesByWorkFlowID)
				{
					empty = "UPDATE " + item2.Key + " SET " + item2.Value + " = '" + item.Value + "' WHERE " + item2.Value + " = '" + item.Key + "'";
					if (sqlCommand.CommandText.Length == 0)
					{
						sqlCommand.CommandText = empty;
					}
					else
					{
						sqlCommand.CommandText = sqlCommand.CommandText + ";" + empty;
					}
				}
			}
		}
		if (workFlowsToUpdate.Count > 0)
		{
			num = Convert.ToInt32(workFlowsToUpdate.Max((KeyValuePair<string, string> item) => item.Value)) + 1;
		}
		if (sqlCommand.CommandText.Length > 0)
		{
			sqlCommand.CommandText = sqlCommand.CommandText + ";update nextids set xanNextID = '" + num + "' where xanTable = 'WorkFlows';";
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, sqlCommand.CommandText);
		}
		SqlCommand sqlCommand2 = parms.DataDictionary.NewSqlCommand("Update ddExplorer Set dxExtd = '" + newWorkFlowId + "' where dxExtd like '%" + text + "%'");
		parms.DataDictionary.ExecuteCommand(sqlCommand2);
	}

	internal Dictionary<string, string> GetWorkFlowTablesByWorkFlowID(ServerManager serverManager, string oldWorkFlowID, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		SqlCommand sqlCommand = new SqlCommand();
		sqlCommand.CommandText = "SELECT 'WORKFLOWS','wfpWorkFlowID' FROM WorkFlows where wfpWorkFlowId = @OldWorkFlowID;SELECT 'WORKFLOWLINES','wflWorkFlowID' FROM WorkFlowLines where wflWorkFlowId = @OldWorkFlowID;SELECT 'WORKFLOWLINERESOURCES','wfrWorkFlowID' FROM WorkFlowLineResources where wfrWorkFlowId = @OldWorkFlowID;";
		sqlCommand.Parameters.Add(new SqlParameter("@OldWorkFlowID", SqlDbType.NVarChar)).Value = oldWorkFlowID;
		DataSet dataSet = serverManager.GetDataSet(null, m1User, databaseName, sqlCommand.CommandText);
		if (dataSet.Tables.Count > 0)
		{
			foreach (DataTable table in dataSet.Tables)
			{
				if (table.Rows.Count > 0)
				{
					DataRow dataRow = table.Rows[0];
					dictionary.Add(dataRow[0].ToString(), dataRow[1].ToString());
				}
			}
		}
		return dictionary;
	}

	internal Dictionary<string, string> GetWorkFlowsToUpdate(ServerManager serverManager, int newWorkFlowId, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName)
	{
		string empty = string.Empty;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		empty = "Select distinct wfpWorkFlowId from WorkFlows where wfpWorkFlowID != '" + newWorkFlowId + "' and wfpWorkFlowID like '%IMP%'";
		DataTable dataTable = serverManager.GetDataTable(null, m1User, databaseName, 0, empty);
		if (dataTable.Rows.Count > 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				dictionary.Add(row[0].ToString(), newWorkFlowId.ToString());
				newWorkFlowId++;
			}
		}
		return dictionary;
	}

	internal string AddResourceForAttachments(ServerManager serverManager, M1User m1User, string databaseName)
	{
		string result = string.Empty;
		SqlCommand sqlCommand = new SqlCommand();
		sqlCommand.CommandText = "SELECT * FROM attachments WHERE cmaWorkFlowID != ''";
		DataTable dataTable = serverManager.GetDataTable(null, m1User, databaseName, 0, sqlCommand.CommandText);
		if (dataTable.Rows.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow row in dataTable.Rows)
			{
				stringBuilder.Append("INSERT INTO WorkFlowLineResources");
				stringBuilder.Append("(wfrWorkFlowID,wfrWorkFlowLineID,wfrResourceID,wfrResourceType,wfrExternalResourceID,wfrUniqueID");
				stringBuilder.Append(")");
				stringBuilder.Append(" VALUES(");
				stringBuilder.Append("'").Append(row["cmaWorkFlowID"].ToString()).Append("',");
				stringBuilder.Append(Convert.ToInt32(row["cmaWorkFlowLineID"].ToString())).Append(",");
				stringBuilder.Append(GetNewResourceKey(serverManager, m1User, databaseName)).Append(",");
				stringBuilder.Append("'").Append("ATTACHMENTS").Append("',");
				stringBuilder.Append("'").Append(row["cmaAttachmentID"].ToString()).Append("',");
				stringBuilder.Append("'").Append(Guid.NewGuid()).Append("');");
			}
			result = stringBuilder.ToString();
		}
		return result;
	}

	private int GetNewResourceKey(ServerManager serverManager, M1User m1User, string databaseName)
	{
		string text = "";
		int num = 0;
		string text2 = "";
		text = "select max(wfrResourceId) + 1  as newId  from workFlowLineResources  group by wfrResourceId";
		DataTable dataTable = serverManager.GetDataTable(null, m1User, databaseName, 0, text);
		if (dataTable.Rows.Count > 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				foreach (DataColumn column in dataTable.Columns)
				{
					text2 = row[column].GetType().ToString().ToUpper();
					if (text2 == "SYSTEM.DECIMAL")
					{
						num = Convert.ToInt32(row[column].ToString());
					}
					else if (text2 == "SYSTEM.DBNULL")
					{
						num = 1;
					}
				}
			}
		}
		else
		{
			num++;
		}
		return num;
	}
}
