using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PurchaseOrderDeliveries to support unicode", "2013-10-17")]
public class v810RebuildPurchaseOrderDeliveries
{
	public v810RebuildPurchaseOrderDeliveries(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderDeliveries", new DmoField[25]
		{
			new DmoField("pmdPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmdPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pmdPurchaseOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("pmdDeliveryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pmdDeliveryDate", "date", 14, 0, nullable: true),
			new DmoField("pmdDeliveryType", "tinyint", 1, 0, nullable: false),
			new DmoField("pmdJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmdJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("pmdJobType", "tinyint", 1, 0, nullable: false),
			new DmoField("pmdJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("pmdJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("pmdOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmdLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmdContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmdShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmdQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("pmdInTransit", "bit", 1, 0, nullable: false),
			new DmoField("pmdTrackingNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("pmdQuantityInvoiced", "numeric", 15, 5, nullable: false),
			new DmoField("pmdReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("pmdInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("pmdClosed", "bit", 1, 0, nullable: false),
			new DmoField("pmdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[17]
		{
			new DmoIndex("PMDPURCHASEORDERID,PMDPURCHASEORDERLINEID,PMDPURCHASEORDERDELIVERYID", unique: true),
			new DmoIndex("PMDUNIQUEID", unique: true),
			new DmoIndex("pmdPurchaseOrderID", unique: false),
			new DmoIndex("pmdPurchaseOrderLineID", unique: false),
			new DmoIndex("pmdPurchaseOrderDeliveryID", unique: false),
			new DmoIndex("pmdDeliveryDate", unique: false),
			new DmoIndex("pmdJobID", unique: false),
			new DmoIndex("pmdJobAssemblyID", unique: false),
			new DmoIndex("pmdJobType", unique: false),
			new DmoIndex("pmdJobMaterialID", unique: false),
			new DmoIndex("pmdJobOperationID", unique: false),
			new DmoIndex("pmdOrganizationID", unique: false),
			new DmoIndex("pmdLocationID", unique: false),
			new DmoIndex("pmdContactID", unique: false),
			new DmoIndex("pmdReceivedComplete", unique: false),
			new DmoIndex("pmdInvoicedComplete", unique: false),
			new DmoIndex("pmdClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
