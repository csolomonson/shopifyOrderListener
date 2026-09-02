using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Extensions;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class M1AdoConnectionProxy
{
	public SqlTransaction SqlTransaction;

	public M1Database Database;

	public object Execute(string query)
	{
		if (query.TrimStart().ToLower().StartsWith("select"))
		{
			M1AdoRecordsetProxy m1AdoRecordsetProxy = new M1AdoRecordsetProxy();
			m1AdoRecordsetProxy.Open(query, this, 0, 1, 0, SqlTransaction);
			return m1AdoRecordsetProxy;
		}
		return Database.ExecuteCommand(query, SqlTransaction);
	}

	public object ExecuteScalar(string query)
	{
		return Database.ExecuteScalar(query, SqlTransaction);
	}

	public object CreateObject(string classId)
	{
		if (classId.Equals("ADODB.Recordset", StringComparison.CurrentCultureIgnoreCase))
		{
			return new M1AdoRecordsetProxy();
		}
		object obj = M1Util.COMCreateObject(classId);
		if (obj == null)
		{
			return DBNull.Value;
		}
		return obj;
	}
}
