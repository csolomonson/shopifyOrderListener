using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseRequisitions to support unicode", "2013-10-17")]
public class v810RebuildWarehouseRequisitions
{
	public v810RebuildWarehouseRequisitions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitions", new DmoField[16]
		{
			new DmoField("wqpWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wqpSourceWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wqpDestinationWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wqpRequestedShipDate", "date", 14, 0, nullable: true),
			new DmoField("wqpRequisitionDate", "date", 14, 0, nullable: true),
			new DmoField("wqpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wqpShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wqpRequisitionCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wqpRequisitionCommentsText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wqpReadyToPrint", "bit", 1, 0, nullable: false),
			new DmoField("wqpStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("wqpClosed", "bit", 1, 0, nullable: false),
			new DmoField("wqpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("wqpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wqpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wqpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("WQPWAREHOUSEREQUISITIONID", unique: true),
			new DmoIndex("WQPUNIQUEID", unique: true),
			new DmoIndex("wqpSourceWarehouseID", unique: false),
			new DmoIndex("wqpDestinationWarehouseID", unique: false),
			new DmoIndex("wqpRequisitionDate", unique: false),
			new DmoIndex("wqpReadyToPrint", unique: false),
			new DmoIndex("wqpStatus", unique: false),
			new DmoIndex("wqpClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
