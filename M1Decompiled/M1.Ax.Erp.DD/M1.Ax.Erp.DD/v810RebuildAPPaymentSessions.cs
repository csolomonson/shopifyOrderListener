using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APPaymentSessions to support unicode", "2013-10-17")]
public class v810RebuildAPPaymentSessions
{
	public v810RebuildAPPaymentSessions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentSessions", new DmoField[28]
		{
			new DmoField("apsAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("apsSessionType", "tinyint", 1, 0, nullable: false),
			new DmoField("apsPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apsPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apsPaymentDate", "date", 14, 0, nullable: true),
			new DmoField("apsGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("apsGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("apsBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apsCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apsPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("apsPaymentsPrinted", "bit", 1, 0, nullable: false),
			new DmoField("apsEFTDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("apsEFTSettlementDate", "date", 14, 0, nullable: true),
			new DmoField("apsCompleted", "bit", 1, 0, nullable: false),
			new DmoField("apsCompletedDate", "date", 14, 0, nullable: true),
			new DmoField("apsEFTReferenceNumber", "nvarchar", 16, 0, nullable: false),
			new DmoField("apsPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("apsPostedDate", "date", 14, 0, nullable: true),
			new DmoField("apsAPGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apsOpenPaymentLoad", "bit", 1, 0, nullable: false),
			new DmoField("apsARGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apsCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apsCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("apsPaymentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apsExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("apsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("apsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("APSAPPAYMENTSESSIONID", unique: true),
			new DmoIndex("APSUNIQUEID", unique: true),
			new DmoIndex("apsPlantDepartmentID", unique: false),
			new DmoIndex("apsPlantID", unique: false),
			new DmoIndex("apsCompleted", unique: false),
			new DmoIndex("apsPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
