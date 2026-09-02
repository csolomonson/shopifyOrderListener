using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LandedCostCharges to support unicode", "2013-10-17")]
public class v810RebuildLandedCostCharges
{
	public v810RebuildLandedCostCharges(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", new DmoField[26]
		{
			new DmoField("rmhLandedCostID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmhLandedCostChargeID", "smallint", 3, 0, nullable: false),
			new DmoField("rmhLandedCostCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmhDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rmhTotalCost", "money", 12, 2, nullable: false),
			new DmoField("rmhTotalCostForeign", "money", 12, 2, nullable: false),
			new DmoField("rmhAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmhAPInvoiceLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rmhCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmhExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("rmhCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("rmhSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmhSupplierLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmhSupplierContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmhInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("rmhLandedCostMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("rmhReverseLandedCostID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmhReverseLandedCostChargeID", "smallint", 3, 0, nullable: false),
			new DmoField("rmhReversed", "bit", 1, 0, nullable: false),
			new DmoField("rmhEstTotalCost", "money", 12, 2, nullable: false),
			new DmoField("rmhEstTotalCostForeign", "money", 12, 2, nullable: false),
			new DmoField("rmhInTransitJournalsCreated", "bit", 1, 0, nullable: false),
			new DmoField("rmhEstExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("rmhCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmhCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmhUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("RMHLANDEDCOSTID,RMHLANDEDCOSTCHARGEID", unique: true),
			new DmoIndex("RMHUNIQUEID", unique: true),
			new DmoIndex("rmhLandedCostID", unique: false),
			new DmoIndex("rmhLandedCostChargeID", unique: false),
			new DmoIndex("rmhLandedCostCategoryID", unique: false),
			new DmoIndex("rmhAPInvoiceID", unique: false),
			new DmoIndex("rmhAPInvoiceLineID", unique: false),
			new DmoIndex("rmhSupplierOrganizationID", unique: false),
			new DmoIndex("rmhInvoicedComplete", unique: false),
			new DmoIndex("rmhInTransitJournalsCreated", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
