using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PaymentMethods to support unicode", "2013-10-17")]
public class v810RebuildPaymentMethods
{
	public v810RebuildPaymentMethods(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PaymentMethods", new DmoField[26]
		{
			new DmoField("xahPaymentMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xahDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xahBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xahARPaymentSessionRule", "tinyint", 2, 0, nullable: false),
			new DmoField("xahSettlementTime", "numeric", 5, 2, nullable: false),
			new DmoField("xahPMJCB", "bit", 1, 0, nullable: false),
			new DmoField("xahPMJAL", "bit", 1, 0, nullable: false),
			new DmoField("xahPMVisa", "bit", 1, 0, nullable: false),
			new DmoField("xahPMMasterCard", "bit", 1, 0, nullable: false),
			new DmoField("xahPMDiscover", "bit", 1, 0, nullable: false),
			new DmoField("xahPMAmex", "bit", 1, 0, nullable: false),
			new DmoField("xahPMDiners", "bit", 1, 0, nullable: false),
			new DmoField("xahPMEnroute", "bit", 1, 0, nullable: false),
			new DmoField("xahPMCheck", "bit", 1, 0, nullable: false),
			new DmoField("xahPMCash", "bit", 1, 0, nullable: false),
			new DmoField("xahPMPurchaseOrder", "bit", 1, 0, nullable: false),
			new DmoField("xahPMStoreCredit", "bit", 1, 0, nullable: false),
			new DmoField("xahUsePayFloGateway", "bit", 1, 0, nullable: false),
			new DmoField("xahPOSAppearanceSeq", "tinyint", 2, 0, nullable: false),
			new DmoField("xahDoNotOpenCashDrawer", "bit", 1, 0, nullable: false),
			new DmoField("xahRefundPriority", "tinyint", 2, 0, nullable: false),
			new DmoField("xahInactive", "bit", 1, 0, nullable: false),
			new DmoField("xahInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xahCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xahCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xahUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XAHPAYMENTMETHODID", unique: true),
			new DmoIndex("XAHUNIQUEID", unique: true),
			new DmoIndex("xahBankAccountID", unique: false),
			new DmoIndex("xahInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
