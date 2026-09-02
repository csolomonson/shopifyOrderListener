using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxYearTotalAllowances to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxYearTotalAllowances
{
	public v810RebuildIncomeTaxYearTotalAllowances(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotalAllowances", new DmoField[11]
		{
			new DmoField("lneIncomeTaxYearID", "smallint", 4, 0, nullable: false),
			new DmoField("lnePlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lneIncomeTaxYearTotalID", "smallint", 4, 0, nullable: false),
			new DmoField("lneIncomeTaxYearAllowanceID", "smallint", 4, 0, nullable: false),
			new DmoField("lneAllowanceID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lneDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lneAmount", "money", 8, 0, nullable: false),
			new DmoField("lneClosed", "bit", 1, 0, nullable: false),
			new DmoField("lneCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lneCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lneUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("LNEINCOMETAXYEARID,LNEPLANTID,LNEINCOMETAXYEARTOTALID,LNEINCOMETAXYEARALLOWANCEID", unique: true),
			new DmoIndex("LNEUNIQUEID", unique: true),
			new DmoIndex("lneIncomeTaxYearID", unique: false),
			new DmoIndex("lnePlantID", unique: false),
			new DmoIndex("lneIncomeTaxYearTotalID", unique: false),
			new DmoIndex("lneIncomeTaxYearAllowanceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
