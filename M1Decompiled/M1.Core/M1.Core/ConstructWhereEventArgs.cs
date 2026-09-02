using System;

namespace M1.Core;

public class ConstructWhereEventArgs : EventArgs
{
	public M1Database Database;

	private string _ExtraFilter;

	private string _TableName;

	private string _WhereClause;

	public string ExtraFilter => _ExtraFilter;

	public string TableName => _TableName;

	public string WhereClause => _WhereClause;

	public ConstructWhereEventArgs(M1Database database, string tableName, string extraFilter, string whereClause)
	{
		Database = database;
		_TableName = tableName;
		_ExtraFilter = extraFilter;
		_WhereClause = whereClause;
	}

	public void AddToWhereClause(string expr)
	{
		if (!string.IsNullOrWhiteSpace(expr))
		{
			if (_WhereClause.Length != 0)
			{
				_WhereClause = _WhereClause + " And " + expr;
			}
			else
			{
				_WhereClause = expr;
			}
		}
	}
}
