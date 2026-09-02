using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert StateUITaxYearQuarterTotals to support unicode", "2013-10-17")]
public class v810RebuildStateUITaxYearQuarterTotals
{
	public v810RebuildStateUITaxYearQuarterTotals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StateUITaxYearQuarterTotals", new DmoField[18]
		{
			new DmoField("putStateUITaxYearID", "smallint", 4, 0, nullable: false),
			new DmoField("putPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("putStateUITaxYearQuarterID", "tinyint", 1, 0, nullable: false),
			new DmoField("putStateUITaxYearQtrTotalID", "smallint", 4, 0, nullable: false),
			new DmoField("putEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("putEmployeeSSN", "nvarchar", 11, 0, nullable: false),
			new DmoField("putEmployeeFirstName", "nvarchar", 40, 0, nullable: false),
			new DmoField("putEmployeeLastName", "nvarchar", 20, 0, nullable: false),
			new DmoField("putWages", "money", 12, 2, nullable: false),
			new DmoField("putTaxableWages", "money", 12, 2, nullable: false),
			new DmoField("putTaxes", "money", 12, 2, nullable: false),
			new DmoField("putMonth1Employment", "bit", 1, 0, nullable: false),
			new DmoField("putMonth2Employment", "bit", 1, 0, nullable: false),
			new DmoField("putMonth3Employment", "bit", 1, 0, nullable: false),
			new DmoField("putClosed", "bit", 1, 0, nullable: false),
			new DmoField("putCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("putCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("putUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("PUTSTATEUITAXYEARID,PUTPLANTID,PUTSTATEUITAXYEARQUARTERID,PUTSTATEUITAXYEARQTRTOTALID", unique: true),
			new DmoIndex("PUTUNIQUEID", unique: true),
			new DmoIndex("putStateUITaxYearID", unique: false),
			new DmoIndex("putPlantID", unique: false),
			new DmoIndex("putStateUITaxYearQuarterID", unique: false),
			new DmoIndex("putStateUITaxYearQtrTotalID", unique: false),
			new DmoIndex("putEmployeeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
