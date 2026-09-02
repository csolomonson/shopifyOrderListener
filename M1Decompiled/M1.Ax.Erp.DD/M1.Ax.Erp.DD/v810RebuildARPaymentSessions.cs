using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARPaymentSessions to support unicode", "2013-10-17")]
public class v810RebuildARPaymentSessions
{
	public v810RebuildARPaymentSessions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentSessions", new DmoField[29]
		{
			new DmoField("arsARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("arsPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arsPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arsReceiptDate", "date", 14, 0, nullable: true),
			new DmoField("arsGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("arsGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("arsARGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arsCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arsBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arsDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arsDepositAmount", "money", 12, 2, nullable: false),
			new DmoField("arsPOSOnDemand", "bit", 1, 0, nullable: false),
			new DmoField("arsPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arsGroupBySettlement", "bit", 1, 0, nullable: false),
			new DmoField("arsSettlementStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("arsSettlementEndTime", "datetime", 14, 0, nullable: true),
			new DmoField("arsPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("arsPostedDate", "date", 14, 0, nullable: true),
			new DmoField("arsAPGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arsAPDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arsOpenPaymentLoad", "bit", 1, 0, nullable: false),
			new DmoField("arsCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arsCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("arsExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("arsDepositAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arsAvalaraTaxCalculated", "bit", 1, 0, nullable: false),
			new DmoField("arsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("ARSARPAYMENTSESSIONID", unique: true),
			new DmoIndex("ARSUNIQUEID", unique: true),
			new DmoIndex("arsPlantDepartmentID", unique: false),
			new DmoIndex("arsPlantID", unique: false),
			new DmoIndex("arsGLFiscalYearID", unique: false),
			new DmoIndex("arsBankAccountID", unique: false),
			new DmoIndex("arsPOSSessionID", unique: false),
			new DmoIndex("arsGroupBySettlement", unique: false),
			new DmoIndex("arsPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
