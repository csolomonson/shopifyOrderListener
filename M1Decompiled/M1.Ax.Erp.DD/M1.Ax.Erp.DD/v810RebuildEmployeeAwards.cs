using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeAwards to support unicode", "2013-10-17")]
public class v810RebuildEmployeeAwards
{
	public v810RebuildEmployeeAwards(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeAwards", new DmoField[5]
		{
			new DmoField("lnaEmployeeAwardID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnaDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lnaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lnaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lnaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("LNAEMPLOYEEAWARDID", unique: true),
			new DmoIndex("LNAUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
