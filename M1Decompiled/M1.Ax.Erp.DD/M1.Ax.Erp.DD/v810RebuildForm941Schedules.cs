using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form941Schedules to support unicode", "2013-10-17")]
public class v810RebuildForm941Schedules
{
	public v810RebuildForm941Schedules(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form941Schedules", new DmoField[41]
		{
			new DmoField("ptsYearID", "smallint", 4, 0, nullable: false),
			new DmoField("ptsQuarterID", "tinyint", 1, 0, nullable: false),
			new DmoField("ptsPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ptsMonthID", "tinyint", 2, 0, nullable: false),
			new DmoField("ptsDay1", "money", 10, 2, nullable: false),
			new DmoField("ptsDay2", "money", 10, 2, nullable: false),
			new DmoField("ptsDay3", "money", 10, 2, nullable: false),
			new DmoField("ptsDay4", "money", 10, 2, nullable: false),
			new DmoField("ptsDay5", "money", 10, 2, nullable: false),
			new DmoField("ptsDay6", "money", 10, 2, nullable: false),
			new DmoField("ptsDay7", "money", 10, 2, nullable: false),
			new DmoField("ptsDay8", "money", 10, 2, nullable: false),
			new DmoField("ptsDay9", "money", 10, 2, nullable: false),
			new DmoField("ptsDay10", "money", 10, 2, nullable: false),
			new DmoField("ptsDay11", "money", 10, 2, nullable: false),
			new DmoField("ptsDay12", "money", 10, 2, nullable: false),
			new DmoField("ptsDay13", "money", 10, 2, nullable: false),
			new DmoField("ptsDay14", "money", 10, 2, nullable: false),
			new DmoField("ptsDay15", "money", 10, 2, nullable: false),
			new DmoField("ptsDay16", "money", 10, 2, nullable: false),
			new DmoField("ptsDay17", "money", 10, 2, nullable: false),
			new DmoField("ptsDay18", "money", 10, 2, nullable: false),
			new DmoField("ptsDay19", "money", 10, 2, nullable: false),
			new DmoField("ptsDay20", "money", 10, 2, nullable: false),
			new DmoField("ptsDay21", "money", 10, 2, nullable: false),
			new DmoField("ptsDay22", "money", 10, 2, nullable: false),
			new DmoField("ptsDay23", "money", 10, 2, nullable: false),
			new DmoField("ptsDay24", "money", 10, 2, nullable: false),
			new DmoField("ptsDay25", "money", 10, 2, nullable: false),
			new DmoField("ptsDay26", "money", 10, 2, nullable: false),
			new DmoField("ptsDay27", "money", 10, 2, nullable: false),
			new DmoField("ptsDay28", "money", 10, 2, nullable: false),
			new DmoField("ptsDay29", "money", 10, 2, nullable: false),
			new DmoField("ptsDay30", "money", 10, 2, nullable: false),
			new DmoField("ptsDay31", "money", 10, 2, nullable: false),
			new DmoField("ptsTotal", "money", 10, 2, nullable: false),
			new DmoField("ptsClosed", "bit", 1, 0, nullable: false),
			new DmoField("ptsDescription", "nvarchar", 4, 0, nullable: false),
			new DmoField("ptsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ptsCreatedDate", "date", 14, 0, nullable: true),
			new DmoField("ptsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("PTSYEARID,PTSPLANTID,PTSQUARTERID,PTSMONTHID", unique: true),
			new DmoIndex("PTSUNIQUEID", unique: true),
			new DmoIndex("ptsPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
