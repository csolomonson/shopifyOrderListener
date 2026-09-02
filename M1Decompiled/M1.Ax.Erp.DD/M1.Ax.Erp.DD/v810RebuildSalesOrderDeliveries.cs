using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderDeliveries to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderDeliveries
{
	public v810RebuildSalesOrderDeliveries(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", new DmoField[38]
		{
			new DmoField("omdSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omdSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omdSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("omdPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omdPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omdPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omdDeliveryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omdDeliveryDate", "date", 14, 0, nullable: true),
			new DmoField("omdDeliveryType", "tinyint", 1, 0, nullable: false),
			new DmoField("omdFirm", "bit", 1, 0, nullable: false),
			new DmoField("omdAmountToInvoice", "money", 12, 2, nullable: false),
			new DmoField("omdAmountToInvoiceForeign", "money", 12, 2, nullable: false),
			new DmoField("omdDifferentLocation", "bit", 1, 0, nullable: false),
			new DmoField("omdCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omdShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdFreightAmountBase", "money", 12, 2, nullable: false),
			new DmoField("omdFreightAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("omdQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("omdQuantityInvoiced", "numeric", 15, 5, nullable: false),
			new DmoField("omdShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("omdInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("omdClosed", "bit", 1, 0, nullable: false),
			new DmoField("omdRequiresInspection", "bit", 1, 0, nullable: false),
			new DmoField("omdPurchaseUnitCostBase", "numeric", 15, 5, nullable: false),
			new DmoField("omdPickInProgress", "bit", 1, 0, nullable: false),
			new DmoField("omdPurchaseUnitCostForeign", "numeric", 15, 5, nullable: false),
			new DmoField("omdSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omdPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("omdReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("omdAvalaraNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[20]
		{
			new DmoIndex("OMDSALESORDERID,OMDSALESORDERLINEID,OMDSALESORDERDELIVERYID", unique: true),
			new DmoIndex("OMDUNIQUEID", unique: true),
			new DmoIndex("omdSalesOrderID", unique: false),
			new DmoIndex("omdSalesOrderLineID", unique: false),
			new DmoIndex("omdSalesOrderDeliveryID", unique: false),
			new DmoIndex("omdPartID", unique: false),
			new DmoIndex("omdPartRevisionID", unique: false),
			new DmoIndex("omdPartWarehouseLocationID", unique: false),
			new DmoIndex("omdPartBinID", unique: false),
			new DmoIndex("omdDeliveryDate", unique: false),
			new DmoIndex("omdDeliveryType", unique: false),
			new DmoIndex("omdDifferentLocation", unique: false),
			new DmoIndex("omdCustomerOrganizationID", unique: false),
			new DmoIndex("omdShipLocationID", unique: false),
			new DmoIndex("omdShipContactID", unique: false),
			new DmoIndex("omdShippedComplete", unique: false),
			new DmoIndex("omdInvoicedComplete", unique: false),
			new DmoIndex("omdClosed", unique: false),
			new DmoIndex("omdSupplierOrganizationID", unique: false),
			new DmoIndex("omdReceivedComplete", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
