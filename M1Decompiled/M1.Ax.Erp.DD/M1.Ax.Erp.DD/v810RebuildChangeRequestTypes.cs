using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ChangeRequestTypes to support unicode", "2013-10-17")]
public class v810RebuildChangeRequestTypes
{
	public v810RebuildChangeRequestTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ChangeRequestTypes", new DmoField[7]
		{
			new DmoField("chtChangeRequestTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("chtDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("chtInactive", "bit", 1, 0, nullable: false),
			new DmoField("chtInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("chtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("chtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CHTCHANGEREQUESTTYPEID", unique: true),
			new DmoIndex("CHTUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
