using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RMAReceipts to support unicode", "2013-10-17")]
public class v810RebuildRMAReceipts
{
	public v810RebuildRMAReceipts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceipts", new DmoField[23]
		{
			new DmoField("rrpRMAReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrpDeliveryDocket", "nvarchar", 20, 0, nullable: false),
			new DmoField("rrpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrpARInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpARInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrpShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpReceiptDate", "date", 14, 0, nullable: true),
			new DmoField("rrpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpFreightCharge", "money", 12, 2, nullable: false),
			new DmoField("rrpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrpClosed", "bit", 1, 0, nullable: false),
			new DmoField("rrpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("rrpCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrpCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("rrpExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("rrpFreightChargeForeign", "money", 12, 2, nullable: false),
			new DmoField("rrpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rrpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rrpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("RRPRMARECEIPTID", unique: true),
			new DmoIndex("RRPUNIQUEID", unique: true),
			new DmoIndex("rrpDeliveryDocket", unique: false),
			new DmoIndex("rrpPlantDepartmentID", unique: false),
			new DmoIndex("rrpPlantID", unique: false),
			new DmoIndex("rrpCustomerOrganizationID", unique: false),
			new DmoIndex("rrpARInvoiceLocationID", unique: false),
			new DmoIndex("rrpShipOrganizationID", unique: false),
			new DmoIndex("rrpShipLocationID", unique: false),
			new DmoIndex("rrpProjectID", unique: false),
			new DmoIndex("rrpClosed", unique: false),
			new DmoIndex("rrpClosedDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
