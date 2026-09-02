using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PunchCalculators to support unicode", "2013-10-17")]
public class v810RebuildPunchCalculators
{
	public v810RebuildPunchCalculators(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PunchCalculators", new DmoField[21]
		{
			new DmoField("ccuPunchCalculatorId", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("ccuPartsPerSheet", "int", 8, 0, nullable: false),
			new DmoField("ccuTools", "int", 8, 0, nullable: false),
			new DmoField("ccuHitsPerPart", "int", 8, 0, nullable: false),
			new DmoField("ccuHitRate", "int", 8, 0, nullable: false),
			new DmoField("ccuRepositions", "int", 8, 0, nullable: false),
			new DmoField("ccuSheetLoadTimeSec", "int", 8, 0, nullable: false),
			new DmoField("ccuTurns", "int", 8, 0, nullable: false),
			new DmoField("ccuToolChangeTimeSec", "int", 8, 0, nullable: false),
			new DmoField("ccuToolChangeTimeTotal", "int", 8, 0, nullable: false),
			new DmoField("ccuRepositionTimeSec", "int", 8, 0, nullable: false),
			new DmoField("ccuTimeToPiece", "numeric", 12, 2, nullable: false),
			new DmoField("ccuRepositionTime", "numeric", 12, 2, nullable: false),
			new DmoField("ccuSheetLoadTime", "numeric", 12, 2, nullable: false),
			new DmoField("ccuTotalTimeSeconds", "int", 8, 0, nullable: false),
			new DmoField("ccuTotalTimeMinutes", "numeric", 12, 2, nullable: false),
			new DmoField("ccuSheetsPerHour", "numeric", 12, 2, nullable: false),
			new DmoField("ccuPartsPerHour", "numeric", 12, 2, nullable: false),
			new DmoField("ccuCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ccuCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ccuUniqueId", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CCUPUNCHCALCULATORID", unique: true),
			new DmoIndex("CCUUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
