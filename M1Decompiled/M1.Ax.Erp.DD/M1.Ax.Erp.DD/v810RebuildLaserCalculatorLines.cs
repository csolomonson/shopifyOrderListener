using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LaserCalculatorLines to support unicode", "2013-10-17")]
public class v810RebuildLaserCalculatorLines
{
	public v810RebuildLaserCalculatorLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LaserCalculatorLines", new DmoField[11]
		{
			new DmoField("cclLaserCalculatorID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("cclLaserCalculatorLineID", "int", 5, 0, nullable: false),
			new DmoField("ccldescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("cclquantity", "numeric", 11, 3, nullable: false),
			new DmoField("ccllength", "numeric", 11, 3, nullable: false),
			new DmoField("cclwidth", "numeric", 11, 3, nullable: false),
			new DmoField("cclrate", "numeric", 11, 3, nullable: false),
			new DmoField("cclcuttime", "numeric", 11, 2, nullable: false),
			new DmoField("cclCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cclCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cclUniqueid", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("CCLLASERCALCULATORID,CCLLASERCALCULATORLINEID", unique: true),
			new DmoIndex("CCLUNIQUEID", unique: true),
			new DmoIndex("cclLaserCalculatorID", unique: false),
			new DmoIndex("cclLaserCalculatorLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
