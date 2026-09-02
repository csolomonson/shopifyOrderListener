using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RFQSuppliers to support unicode", "2013-10-17")]
public class v810RebuildRFQSuppliers
{
	public v810RebuildRFQSuppliers(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RFQSuppliers", new DmoField[19]
		{
			new DmoField("rqsRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqsRFQLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rqsRFQSupplierID", "smallint", 4, 0, nullable: false),
			new DmoField("rqsSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqsPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rqsPurchaseContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rqsOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rqsDueDate", "date", 14, 0, nullable: true),
			new DmoField("rqsCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rqsCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("rqsExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("rqsComplete", "bit", 1, 0, nullable: false),
			new DmoField("rqsSelectedSupplier", "bit", 1, 0, nullable: false),
			new DmoField("rqsSelectedSupplierDate", "date", 14, 0, nullable: true),
			new DmoField("rqsUpdatedPartPrices", "bit", 1, 0, nullable: false),
			new DmoField("rqsClosed", "bit", 1, 0, nullable: false),
			new DmoField("rqsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rqsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rqsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("RQSRFQID,RQSRFQLINEID,RQSRFQSUPPLIERID", unique: true),
			new DmoIndex("RQSUNIQUEID", unique: true),
			new DmoIndex("rqsRFQID", unique: false),
			new DmoIndex("rqsRFQLineID", unique: false),
			new DmoIndex("rqsRFQSupplierID", unique: false),
			new DmoIndex("rqsSupplierOrganizationID", unique: false),
			new DmoIndex("rqsPurchaseLocationID", unique: false),
			new DmoIndex("rqsOrgPartID", unique: false),
			new DmoIndex("rqsComplete", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
