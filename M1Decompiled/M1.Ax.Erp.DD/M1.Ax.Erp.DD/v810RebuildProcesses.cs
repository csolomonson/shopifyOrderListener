using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Processes to support unicode", "2013-10-17")]
public class v810RebuildProcesses
{
	public v810RebuildProcesses(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Processes", new DmoField[17]
		{
			new DmoField("xacProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xacShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xacLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xacLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xacPrintInspectionLine", "bit", 1, 0, nullable: false),
			new DmoField("xacProjectedSetupRate", "numeric", 8, 2, nullable: false),
			new DmoField("xacProjectedProductionRate", "numeric", 8, 2, nullable: false),
			new DmoField("xacSetupHours", "numeric", 8, 2, nullable: false),
			new DmoField("xacStandardFactor", "nvarchar", 2, 0, nullable: false),
			new DmoField("xacProductionStandard", "numeric", 10, 4, nullable: false),
			new DmoField("xacInactive", "bit", 1, 0, nullable: false),
			new DmoField("xacInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xacInspectionType", "tinyint", 1, 0, nullable: false),
			new DmoField("xacExcludeFromTMJobs", "bit", 1, 0, nullable: false),
			new DmoField("xacCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xacCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xacUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XACPROCESSID", unique: true),
			new DmoIndex("XACUNIQUEID", unique: true),
			new DmoIndex("xacInactive", unique: false),
			new DmoIndex("xacInspectionType", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
