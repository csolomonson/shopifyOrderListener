using System;
using System.Data;
using System.Data.SqlClient;

namespace M1.Core;

public class DataDictionaryInfoCollection : DatabaseInfoCollection
{
	public DataDictionaryInfoCollection(AppContext context)
		: base(context)
	{
	}

	public override void Refresh()
	{
		Refresh(null, null);
	}

	public override void Refresh(SqlConnection sqlConnection, M1User m1User)
	{
		Clear();
		bool needToClose = true;
		sqlConnection = currentContext.DDServerManager.GetConnection(null, "master", openImmediately: true, sqlConnection, null, ref needToClose);
		try
		{
			DataTable dataTable = currentContext.DDServerManager.GetDataTable(sqlConnection, m1User, "master", 0, "select name,user_access,isnull(collation_name,'') as collation_name,compatibility_level from sys.databases where lower(left(name,2))='m1' And lower(left(name,3)) <> 'm1_' And HAS_PERMS_BY_NAME(name, 'DATABASE', 'UPDATE')=1 order by name asc");
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					DatabaseInfo dataDictionaryProperties = getDataDictionaryProperties(sqlConnection, m1User, row.Field<string>("name"));
					if (dataDictionaryProperties != null)
					{
						dataDictionaryProperties.SingleUser = row.Field<byte>("user_access") == 1;
						dataDictionaryProperties.CollationName = row.Field<string>("collation_name");
						dataDictionaryProperties.CompatibilityLevel = row.Field<byte>("compatibility_level");
						Add(dataDictionaryProperties);
					}
				}
			}
		}
		finally
		{
			if (needToClose)
			{
				sqlConnection.Close();
			}
		}
		OnListRefreshed(EventArgs.Empty);
	}

	private DatabaseInfo getDataDictionaryProperties(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		bool needToClose = true;
		DatabaseInfo databaseInfo = new DatabaseInfo(dataDictionary: true);
		DataTable dataTable = new DataTable();
		sqlConnection = currentContext.DDServerManager.GetConnection(m1User, "master", openImmediately: false, sqlConnection, null, ref needToClose);
		try
		{
			new SqlDataAdapter("select * from " + databaseName + ".dbo.DDInfo", sqlConnection).Fill(dataTable);
		}
		finally
		{
			if (needToClose)
			{
				sqlConnection.Close();
			}
		}
		if (dataTable.Rows.Count > 0)
		{
			foreach (DataColumn column in dataTable.Columns)
			{
				string text = column.ColumnName.ToUpper();
				if (text == "DDVERS" || text == "DDVERSION")
				{
					databaseInfo.Version = dataTable.Rows[0][column].ToString().Trim();
				}
			}
		}
		if (databaseInfo.Version.Length != 0)
		{
			databaseInfo.Description = databaseName;
			databaseInfo.Name = databaseName;
			return databaseInfo;
		}
		return null;
	}
}
