using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseReceipts to support unicode", "2013-10-17")]
public class v810RebuildWarehouseReceipts
{
	public v810RebuildWarehouseReceipts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceipts", new DmoField[12]
		{
			new DmoField("wrpWarehouseReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wrpSourceWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wrpDestinationWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wrpReceiptDate", "datetime", 14, 0, nullable: true),
			new DmoField("wrpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wrpShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wrpFreightCharge", "money", 12, 2, nullable: false),
			new DmoField("wrpClosed", "bit", 1, 0, nullable: false),
			new DmoField("wrpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("wrpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wrpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wrpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("WRPWAREHOUSERECEIPTID", unique: true),
			new DmoIndex("WRPUNIQUEID", unique: true),
			new DmoIndex("wrpSourceWarehouseID", unique: false),
			new DmoIndex("wrpDestinationWarehouseID", unique: false),
			new DmoIndex("wrpClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
