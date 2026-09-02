using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartPrices to support unicode", "2013-10-17")]
public class v810RebuildPartPrices
{
	public v810RebuildPartPrices(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartPrices", new DmoField[17]
		{
			new DmoField("imiPartPriceID", "int", 9, 0, nullable: false),
			new DmoField("imiPriceType", "tinyint", 1, 0, nullable: false),
			new DmoField("imiPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imiPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imiPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imiCustomerGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imiOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imiStartDate", "date", 14, 0, nullable: true),
			new DmoField("imiEndDate", "date", 14, 0, nullable: true),
			new DmoField("imiCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imiInventoryPrice", "bit", 1, 0, nullable: false),
			new DmoField("imiQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imiRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imiLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("IMIPARTPRICEID", unique: true),
			new DmoIndex("IMIUNIQUEID", unique: true),
			new DmoIndex("imiPartID", unique: false),
			new DmoIndex("imiPartRevisionID", unique: false),
			new DmoIndex("imiOrganizationID", unique: false),
			new DmoIndex("imiQuoteID", unique: false),
			new DmoIndex("imiRFQID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
