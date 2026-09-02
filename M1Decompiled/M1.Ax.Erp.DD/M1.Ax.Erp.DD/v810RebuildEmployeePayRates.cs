using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeePayRates to support unicode", "2013-10-17")]
public class v810RebuildEmployeePayRates
{
	public v810RebuildEmployeePayRates(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePayRates", new DmoField[17]
		{
			new DmoField("lnrEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lnrEmployeePayRateID", "smallint", 4, 0, nullable: false),
			new DmoField("lnrAwardRate", "numeric", 8, 4, nullable: false),
			new DmoField("lnrAwardID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnrAdditionalRate", "numeric", 8, 4, nullable: false),
			new DmoField("lnrAdditionalPercent", "numeric", 6, 2, nullable: false),
			new DmoField("lnrPayRate", "numeric", 8, 4, nullable: false),
			new DmoField("lnrPayFrequencyAmount", "money", 12, 2, nullable: false),
			new DmoField("lnrSalaryAmount", "money", 12, 2, nullable: false),
			new DmoField("lnrStartDate", "date", 14, 0, nullable: true),
			new DmoField("lnrReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnrPayRateNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lnrPayRatenotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lnrAwardLineID", "smallint", 4, 0, nullable: false),
			new DmoField("lnrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lnrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lnrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LNREMPLOYEEID,LNREMPLOYEEPAYRATEID", unique: true),
			new DmoIndex("LNRUNIQUEID", unique: true),
			new DmoIndex("lnrEmployeeID", unique: false),
			new DmoIndex("lnrEmployeePayRateID", unique: false),
			new DmoIndex("lnrReasonID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
