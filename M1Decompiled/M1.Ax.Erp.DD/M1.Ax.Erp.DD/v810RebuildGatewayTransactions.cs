using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GatewayTransactions to support unicode", "2013-10-17")]
public class v810RebuildGatewayTransactions
{
	public v810RebuildGatewayTransactions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GatewayTransactions", new DmoField[16]
		{
			new DmoField("lmjGatewayTransactionID", "smallint", 4, 0, nullable: false),
			new DmoField("lmjEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmjGatewayHeaderID", "smallint", 4, 0, nullable: false),
			new DmoField("lmjTerminalID", "smallint", 4, 0, nullable: false),
			new DmoField("lmjTransactionLevel", "smallint", 4, 0, nullable: false),
			new DmoField("lmjShiftID", "smallint", 3, 0, nullable: false),
			new DmoField("lmjJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmjJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("lmjJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("lmjJobType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmjTimecardID", "int", 9, 0, nullable: false),
			new DmoField("lmjGoodQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("lmjScrapQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("lmjWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmjProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmjIssueMaterial", "bit", 1, 0, nullable: false)
		}, new DmoIndex[1]
		{
			new DmoIndex("LMJGATEWAYTRANSACTIONID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
