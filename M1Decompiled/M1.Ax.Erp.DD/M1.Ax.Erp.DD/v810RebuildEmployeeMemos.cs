using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeMemos to support unicode", "2013-10-17")]
public class v810RebuildEmployeeMemos
{
	public v810RebuildEmployeeMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeMemos", new DmoField[9]
		{
			new DmoField("lmkEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmkEmployeeMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("lmkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("lmkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMKEMPLOYEEID,LMKEMPLOYEEMEMOID", unique: true),
			new DmoIndex("LMKUNIQUEID", unique: true),
			new DmoIndex("lmkEmployeeID", unique: false),
			new DmoIndex("lmkEmployeeMemoID", unique: false),
			new DmoIndex("lmkMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
