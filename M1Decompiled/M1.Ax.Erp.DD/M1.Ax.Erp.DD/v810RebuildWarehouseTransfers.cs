using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseTransfers to support unicode", "2013-10-17")]
public class v810RebuildWarehouseTransfers
{
	public v810RebuildWarehouseTransfers(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransfers", new DmoField[18]
		{
			new DmoField("mwpWarehouseTransferID", "nvarchar", 10, 0, nullable: false),
			new DmoField("mwpSourceWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mwpDestinationWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mwpShipDate", "datetime", 14, 0, nullable: true),
			new DmoField("mwpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mwpShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mwpFreightCharge", "money", 12, 2, nullable: false),
			new DmoField("mwpShippingCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("mwpShippingCommentsText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("mwpPrintLabels", "bit", 1, 0, nullable: false),
			new DmoField("mwpNumberOfLabels", "smallint", 3, 0, nullable: false),
			new DmoField("mwpPrintPacker", "bit", 1, 0, nullable: false),
			new DmoField("mwpTrackingNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("mwpClosed", "bit", 1, 0, nullable: false),
			new DmoField("mwpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("mwpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("mwpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("mwpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("MWPWAREHOUSETRANSFERID", unique: true),
			new DmoIndex("MWPUNIQUEID", unique: true),
			new DmoIndex("mwpSourceWarehouseID", unique: false),
			new DmoIndex("mwpDestinationWarehouseID", unique: false),
			new DmoIndex("mwpClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
