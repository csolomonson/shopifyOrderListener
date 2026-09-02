using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert BarCalculators to support unicode", "2013-10-17")]
public class v810RebuildBarCalculators
{
	public v810RebuildBarCalculators(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "BarCalculators", new DmoField[19]
		{
			new DmoField("ccbBarCalculatorID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("ccbPartLength", "numeric", 12, 3, nullable: false),
			new DmoField("ccbCutWidth", "numeric", 12, 3, nullable: false),
			new DmoField("ccbFaceOff", "numeric", 12, 3, nullable: false),
			new DmoField("ccbDroplength", "numeric", 12, 3, nullable: false),
			new DmoField("ccbBarlength", "numeric", 12, 3, nullable: false),
			new DmoField("ccbPartsperbar", "numeric", 12, 3, nullable: false),
			new DmoField("ccbUsableBar", "numeric", 12, 3, nullable: false),
			new DmoField("ccbPartsRequired", "numeric", 12, 3, nullable: false),
			new DmoField("ccbScrapPercentage", "numeric", 12, 2, nullable: false),
			new DmoField("ccbPartstomake", "numeric", 12, 3, nullable: false),
			new DmoField("ccbBarsrequired", "numeric", 12, 2, nullable: false),
			new DmoField("ccbfacingbothends", "bit", 1, 0, nullable: false),
			new DmoField("ccbround", "bit", 1, 0, nullable: false),
			new DmoField("ccbMeasurementType", "nvarchar", 1, 0, nullable: false),
			new DmoField("ccbQuantityPerParent", "numeric", 12, 3, nullable: false),
			new DmoField("ccbCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ccbCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ccbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CCBBARCALCULATORID", unique: true),
			new DmoIndex("CCBUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
