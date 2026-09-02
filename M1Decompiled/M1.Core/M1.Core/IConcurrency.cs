using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public interface IConcurrency
{
	string RowVersionFieldName(FieldCollection fields, TableDefinition primaryTable);

	StringBuilder AddConcurrencyCheckToWhereClauseIfNecessary(StringBuilder whereClause, SqlCommand updateCommand, FieldCollection fields, TableDefinition primaryTable);
}
