using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert POSLog to support unicode", "2013-10-17")]
public class v810RebuildPOSLog
{
	public v810RebuildPOSLog(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "POSLog", new DmoField[8]
		{
			new DmoField("psgPOSLogID", "identity", 4, 0, nullable: false),
			new DmoField("psgPOSActivityType", "tinyint", 2, 0, nullable: false),
			new DmoField("psgPOSEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("psgPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("psgPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("psgPOSTransactionPaymentID", "tinyint", 2, 0, nullable: false),
			new DmoField("psgActivityTimestamp", "date", 14, 0, nullable: true),
			new DmoField("psgCashDropAmt", "money", 12, 2, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PSGPOSLOGID", unique: true),
			new DmoIndex("psgPOSEmployeeID", unique: false),
			new DmoIndex("psgPOSSessionID", unique: false),
			new DmoIndex("psgPOSTransactionID", unique: false),
			new DmoIndex("psgPOSTransactionPaymentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
