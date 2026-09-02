using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Expenses to support unicode", "2013-10-17")]
public class v810RebuildExpenses
{
	public v810RebuildExpenses(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Expenses", new DmoField[5]
		{
			new DmoField("lmxExpenseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmxDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("LMXEXPENSEID", unique: true),
			new DmoIndex("LMXUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
