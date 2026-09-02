using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LaserCalculators to support unicode", "2013-10-17")]
public class v810RebuildLaserCalculators
{
	public v810RebuildLaserCalculators(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LaserCalculators", new DmoField[30]
		{
			new DmoField("ccpLaserCalculatorID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("ccpLaserMaterialTypeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ccpThickness", "numeric", 12, 3, nullable: false),
			new DmoField("ccpExternalFeed", "numeric", 12, 3, nullable: false),
			new DmoField("ccpLeadInOutFeed", "numeric", 12, 3, nullable: false),
			new DmoField("ccpLeadInOut", "numeric", 12, 3, nullable: false),
			new DmoField("ccpQuantity", "numeric", 12, 3, nullable: false),
			new DmoField("ccplength", "numeric", 12, 3, nullable: false),
			new DmoField("ccpWidth", "numeric", 12, 3, nullable: false),
			new DmoField("ccpRate", "numeric", 12, 3, nullable: false),
			new DmoField("ccpdescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("ccpTotalCutTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpSquare", "bit", 1, 0, nullable: false),
			new DmoField("ccpRectangle", "bit", 1, 0, nullable: false),
			new DmoField("ccpRound", "bit", 1, 0, nullable: false),
			new DmoField("ccpObround", "bit", 1, 0, nullable: false),
			new DmoField("ccpOther", "bit", 1, 0, nullable: false),
			new DmoField("ccpNumberOfHoles", "int", 8, 0, nullable: false),
			new DmoField("ccpPartPerimeter", "numeric", 12, 3, nullable: false),
			new DmoField("ccpLeadInOutTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpPerimeterCutTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpTotalLeadInOutTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpMeasurementType", "nvarchar", 1, 0, nullable: false),
			new DmoField("ccpHoleCutTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpPiercedHoles", "numeric", 12, 3, nullable: false),
			new DmoField("ccpPierceTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpTotalPierceTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ccpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ccpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CCPLASERCALCULATORID", unique: true),
			new DmoIndex("CCPUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
