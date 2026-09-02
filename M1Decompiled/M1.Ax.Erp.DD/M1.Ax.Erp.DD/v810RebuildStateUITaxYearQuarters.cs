using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert StateUITaxYearQuarters to support unicode", "2013-10-17")]
public class v810RebuildStateUITaxYearQuarters
{
	public v810RebuildStateUITaxYearQuarters(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StateUITaxYearQuarters", new DmoField[22]
		{
			new DmoField("puqStateUITaxYearID", "smallint", 4, 0, nullable: false),
			new DmoField("puqPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("puqStateUITaxYearQuarterID", "tinyint", 1, 0, nullable: false),
			new DmoField("puqNumberOfEmployees", "int", 7, 0, nullable: false),
			new DmoField("puqTotalWages", "money", 12, 2, nullable: false),
			new DmoField("puqTotalTaxableWages", "money", 12, 2, nullable: false),
			new DmoField("puqUITaxRate", "numeric", 6, 2, nullable: false),
			new DmoField("puqUITaxesDue", "money", 10, 2, nullable: false),
			new DmoField("puqMonth1Employment", "int", 7, 0, nullable: false),
			new DmoField("puqMonth2Employment", "int", 7, 0, nullable: false),
			new DmoField("puqMonth3Employment", "int", 7, 0, nullable: false),
			new DmoField("puqCountyCode", "nvarchar", 3, 0, nullable: false),
			new DmoField("puqOutsideCountyEmployees", "int", 7, 0, nullable: false),
			new DmoField("puqInterest", "money", 12, 2, nullable: false),
			new DmoField("puqPenalty", "money", 12, 2, nullable: false),
			new DmoField("puqBalanceDuePriorPeriod", "money", 12, 2, nullable: false),
			new DmoField("puqTotalDue", "money", 12, 2, nullable: false),
			new DmoField("puqReportOtherState", "bit", 1, 0, nullable: false),
			new DmoField("puqClosed", "bit", 1, 0, nullable: false),
			new DmoField("puqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("puqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("puqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PUQSTATEUITAXYEARID,PUQPLANTID,PUQSTATEUITAXYEARQUARTERID", unique: true),
			new DmoIndex("PUQUNIQUEID", unique: true),
			new DmoIndex("puqStateUITaxYearID", unique: false),
			new DmoIndex("puqPlantID", unique: false),
			new DmoIndex("puqStateUITaxYearQuarterID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
