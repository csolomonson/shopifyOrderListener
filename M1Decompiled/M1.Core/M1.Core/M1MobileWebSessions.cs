using System;
using System.Data.SqlClient;

namespace M1.Core;

public class M1MobileWebSessions
{
	private string connectionString;

	public Guid SessionID { get; private set; }

	public string Model { get; private set; }

	public string DataSet { get; private set; }

	public string UserID { get; private set; }

	public DateTime DateCreate { get; private set; }

	public DateTime DateLastUsed { get; private set; }

	public bool Active { get; private set; }

	public void SetConnectionString(string connection)
	{
		connectionString = connection;
	}

	public void LoadSessions()
	{
		using (new SqlConnection(connectionString))
		{
		}
	}
}
