using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SheetCalculators to support unicode", "2013-10-17")]
public class v810RebuildSheetCalculators
{
	public v810RebuildSheetCalculators(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SheetCalculators", new DmoField[16]
		{
			new DmoField("ccsSheetCalculatorID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("ccsSheetSizeX", "numeric", 12, 3, nullable: false),
			new DmoField("ccsSheetSizeY", "numeric", 12, 3, nullable: false),
			new DmoField("ccsTotalTrimX", "numeric", 12, 3, nullable: false),
			new DmoField("ccsTotalTrimY", "numeric", 12, 3, nullable: false),
			new DmoField("ccsPartSpacingX", "numeric", 12, 3, nullable: false),
			new DmoField("ccsPartSpacingY", "numeric", 12, 3, nullable: false),
			new DmoField("ccsPartSizeX", "numeric", 12, 3, nullable: false),
			new DmoField("ccsPartSizeY", "numeric", 12, 3, nullable: false),
			new DmoField("ccsGrain", "bit", 1, 0, nullable: false),
			new DmoField("ccs0Rotation", "numeric", 12, 3, nullable: false),
			new DmoField("ccs90Rotation", "numeric", 12, 3, nullable: false),
			new DmoField("ccsMeasurementType", "nvarchar", 1, 0, nullable: false),
			new DmoField("ccsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ccsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ccsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CCSSHEETCALCULATORID", unique: true),
			new DmoIndex("CCSUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
