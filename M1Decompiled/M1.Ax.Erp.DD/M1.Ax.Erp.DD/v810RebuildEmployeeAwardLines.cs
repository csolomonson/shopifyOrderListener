using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeAwardLines to support unicode", "2013-10-17")]
public class v810RebuildEmployeeAwardLines
{
	public v810RebuildEmployeeAwardLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeAwardLines", new DmoField[9]
		{
			new DmoField("lnnEmployeeAwardID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnnEmployeeAwardLineID", "smallint", 4, 0, nullable: false),
			new DmoField("lnnPayRate", "numeric", 8, 4, nullable: false),
			new DmoField("lnnStartDate", "date", 14, 0, nullable: true),
			new DmoField("lnnInactive", "bit", 1, 0, nullable: false),
			new DmoField("lnnInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("lnnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lnnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lnnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LNNEMPLOYEEAWARDID,LNNEMPLOYEEAWARDLINEID", unique: true),
			new DmoIndex("LNNUNIQUEID", unique: true),
			new DmoIndex("lnnEmployeeAwardID", unique: false),
			new DmoIndex("lnnEmployeeAwardLineID", unique: false),
			new DmoIndex("lnnInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
