using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FedexLog to support unicode", "2013-10-17")]
public class v810RebuildFedexLog
{
	public v810RebuildFedexLog(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FedexLog", new DmoField[19]
		{
			new DmoField("fxlLogID", "identity", 4, 0, nullable: false),
			new DmoField("fxlUti", "nvarchar", 4, 0, nullable: false),
			new DmoField("fxlAccountNumber", "numeric", 12, 0, nullable: false),
			new DmoField("fxlMeterNumber", "numeric", 10, 0, nullable: false),
			new DmoField("fxlFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fxlCartID", "nvarchar", 50, 0, nullable: false),
			new DmoField("fxlSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fxlRequestOut", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fxlReplyIn", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fxlRequestDate", "datetime", 14, 0, nullable: true),
			new DmoField("fxlReplyDate", "datetime", 14, 0, nullable: true),
			new DmoField("fxlReplyErrorCode", "nvarchar", 8, 0, nullable: false),
			new DmoField("fxlReplyErrorMessage", "nvarchar", 120, 0, nullable: false),
			new DmoField("fxlReplySoftErrorCode", "nvarchar", 8, 0, nullable: false),
			new DmoField("fxlReplySoftErrorType", "nvarchar", 25, 0, nullable: false),
			new DmoField("fxlReplySoftErrorMessage", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fxlClosed", "bit", 1, 0, nullable: false),
			new DmoField("fxlClosedDate", "date", 14, 0, nullable: true),
			new DmoField("fxlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("FXLLOGID", unique: true),
			new DmoIndex("FXLUNIQUEID", unique: true),
			new DmoIndex("fxlFreightShipmentID", unique: false),
			new DmoIndex("fxlSalesOrderID", unique: false),
			new DmoIndex("fxlClosed", unique: false),
			new DmoIndex("fxlClosedDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
