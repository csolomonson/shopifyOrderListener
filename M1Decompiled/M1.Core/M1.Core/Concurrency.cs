using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class Concurrency : IConcurrency
{
	public string RowVersionFieldName(FieldCollection fields, TableDefinition primaryTable)
	{
		if (fields != null && primaryTable != null && fields.Contains(primaryTable.FieldPrefix + "RowVersion"))
		{
			return primaryTable.FieldPrefix + "RowVersion";
		}
		return null;
	}

	public StringBuilder AddConcurrencyCheckToWhereClauseIfNecessary(StringBuilder whereClause, SqlCommand updateCommand, FieldCollection fields, TableDefinition primaryTable)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(whereClause);
		string text = RowVersionFieldName(fields, primaryTable);
		if (text != null)
		{
			int count = updateCommand.Parameters.Count;
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.AppendFormat("{0}=@P{1}", text, count.ToString());
			SqlParameter value = new SqlParameter("@P" + count, SqlDbType.Timestamp, 0, text);
			updateCommand.Parameters.Add(value).SourceVersion = DataRowVersion.Original;
		}
		return stringBuilder;
	}
}
