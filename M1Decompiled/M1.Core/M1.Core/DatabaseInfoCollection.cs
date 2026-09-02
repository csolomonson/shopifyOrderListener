using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace M1.Core;

public class DatabaseInfoCollection : KeyedCollection<string, DatabaseInfo>
{
	protected AppContext currentContext;

	public event EventHandler ListRefreshed;

	public DatabaseInfoCollection(AppContext context)
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
		currentContext = context;
	}

	protected override string GetKeyForItem(DatabaseInfo item)
	{
		return item.Name;
	}

	public void OnListRefreshed(EventArgs e)
	{
		this.ListRefreshed?.Invoke(this, e);
	}

	public virtual void Refresh()
	{
		Refresh(null, null);
	}

	public virtual void Refresh(string database)
	{
		bool needToClose = true;
		SqlConnection connection = currentContext.DBServerManager.GetConnection(null, "master", openImmediately: true, null, null, ref needToClose);
		try
		{
			SqlCommand sqlCommand = currentContext.DBServerManager.NewSqlCommand(connection, null, "master", "Select user_access,isnull(collation_name, '') as collation_name From sys.databases where name = @Name");
			sqlCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar)).Value = database;
			byte b = 0;
			string collationName = string.Empty;
			SqlDataAdapter adapter;
			DataTable dataTable = currentContext.DBServerManager.GetDataTable(connection, null, "master", 0, sqlCommand, fillSchema: false, out adapter);
			if (dataTable.Rows.Count != 0)
			{
				b = dataTable.Rows[0].Field<byte>("user_access");
				collationName = dataTable.Rows[0].Field<string>("collation_name");
			}
			if (b == 1)
			{
				currentContext.DBServerManager.ClearAllPools();
			}
			DatabaseInfo datasetProperties = currentContext.DBServerManager.GetDatasetProperties(connection, null, database);
			if (datasetProperties != null)
			{
				datasetProperties.SingleUser = b == 1;
				datasetProperties.CollationName = collationName;
				if (Contains(database))
				{
					SetItem(IndexOf(base[database]), datasetProperties);
				}
				else
				{
					Add(datasetProperties);
				}
			}
		}
		finally
		{
			if (needToClose)
			{
				connection.Close();
			}
		}
		OnListRefreshed(EventArgs.Empty);
	}

	public virtual void Refresh(SqlConnection sqlConnection, M1User m1User)
	{
		Clear();
		bool needToClose = true;
		sqlConnection = currentContext.DBServerManager.GetConnection(m1User, "master", openImmediately: true, sqlConnection, null, ref needToClose);
		try
		{
			DataTable dataTable = currentContext.DBServerManager.GetDataTable(sqlConnection, m1User, "master", 0, "select name,user_access,isnull(collation_name, '') as collation_name,compatibility_level from sys.databases where lower(left(name,3))='m1_' And HAS_PERMS_BY_NAME(name, 'DATABASE', 'UPDATE')=1 order by name asc");
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					DatabaseInfo datasetProperties = currentContext.DBServerManager.GetDatasetProperties(sqlConnection, m1User, row.Field<string>("name"));
					if (datasetProperties != null)
					{
						datasetProperties.SingleUser = row.Field<byte>("user_access") == 1;
						datasetProperties.CollationName = row.Field<string>("collation_name");
						datasetProperties.CompatibilityLevel = row.Field<byte>("compatibility_level");
						Add(datasetProperties);
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
}
