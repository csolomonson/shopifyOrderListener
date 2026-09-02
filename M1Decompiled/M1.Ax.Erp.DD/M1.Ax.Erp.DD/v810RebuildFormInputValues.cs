using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FormInputValues to support unicode", "2013-10-17")]
public class v810RebuildFormInputValues
{
	public v810RebuildFormInputValues(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormInputValues", new DmoField[9]
		{
			new DmoField("xaiFormInputValueID", "identity", 4, 0, nullable: false),
			new DmoField("xaiFormID", "nvarchar", 75, 0, nullable: false),
			new DmoField("xaiControlName", "nvarchar", 35, 0, nullable: false),
			new DmoField("xaiValue", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xaiSourceUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("xaiSourceTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("xaiParentFormID", "nvarchar", 75, 0, nullable: false),
			new DmoField("xaiTopLevelFormID", "nvarchar", 75, 0, nullable: false),
			new DmoField("xaiLastRunDate", "datetime", 14, 0, nullable: true)
		}, new DmoIndex[7]
		{
			new DmoIndex("XAIFORMINPUTVALUEID", unique: true),
			new DmoIndex("xaiFormID", unique: false),
			new DmoIndex("xaiControlName", unique: false),
			new DmoIndex("xaiSourceUniqueID", unique: false),
			new DmoIndex("xaiSourceTable", unique: false),
			new DmoIndex("xaiParentFormID", unique: false),
			new DmoIndex("xaiTopLevelFormID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
