using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollSessions to support unicode", "2013-10-17")]
public class v810RebuildPayrollSessions
{
	public v810RebuildPayrollSessions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollSessions", new DmoField[31]
		{
			new DmoField("pasPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("pasPayrollDate", "date", 14, 0, nullable: true),
			new DmoField("pasTaxYear", "smallint", 4, 0, nullable: false),
			new DmoField("pasGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("pasGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("pasBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pasCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pasPayFrequency", "tinyint", 1, 0, nullable: false),
			new DmoField("pasPayrollStartDate", "date", 14, 0, nullable: true),
			new DmoField("pasPayrollEndDate", "date", 14, 0, nullable: true),
			new DmoField("pasDeductionPeriod", "tinyint", 1, 0, nullable: false),
			new DmoField("pasPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("pasEFTDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("pasEFTSettlementDate", "date", 14, 0, nullable: true),
			new DmoField("pasOpenPayrollLoad", "bit", 1, 0, nullable: false),
			new DmoField("pasPayrollNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pasPayrollNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pasProcessed", "bit", 1, 0, nullable: false),
			new DmoField("pasCompleted", "bit", 1, 0, nullable: false),
			new DmoField("pasCompletedDate", "date", 14, 0, nullable: true),
			new DmoField("pasPaymentsPrinted", "bit", 1, 0, nullable: false),
			new DmoField("pasEFTReferenceNumber", "nvarchar", 16, 0, nullable: false),
			new DmoField("pasPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("pasPostedDate", "date", 14, 0, nullable: true),
			new DmoField("pasCalculationType", "tinyint", 1, 0, nullable: false),
			new DmoField("pasPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pasTransferredToSTP", "bit", 1, 0, nullable: false),
			new DmoField("pasSTPSessionID", "int", 9, 0, nullable: false),
			new DmoField("pasCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pasCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pasUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("PASPAYROLLSESSIONID", unique: true),
			new DmoIndex("PASUNIQUEID", unique: true),
			new DmoIndex("pasPayrollDate", unique: false),
			new DmoIndex("pasTaxYear", unique: false),
			new DmoIndex("pasCompleted", unique: false),
			new DmoIndex("pasPostedToGL", unique: false),
			new DmoIndex("pasPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
