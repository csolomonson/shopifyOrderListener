using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderCarts to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderCarts
{
	public v810RebuildSalesOrderCarts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderCarts", new DmoField[24]
		{
			new DmoField("omcWebConfigMode", "tinyint", 1, 0, nullable: false),
			new DmoField("omcSalesOrderCartID", "identity", 4, 0, nullable: false),
			new DmoField("omcCartID", "nvarchar", 50, 0, nullable: false),
			new DmoField("omcCartType", "tinyint", 1, 0, nullable: false),
			new DmoField("omcPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omcConfigPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omcPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omcConfigPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omcQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omcUnitPrice", "numeric", 15, 5, nullable: false),
			new DmoField("omcPartUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("omcPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("omcPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omcPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omcDateCreated", "datetime", 14, 0, nullable: true),
			new DmoField("omcQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omcQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omcConfigured", "bit", 1, 0, nullable: false),
			new DmoField("omcWebConfigPriceRule", "tinyint", 1, 0, nullable: false),
			new DmoField("omcConfigComplete", "bit", 1, 0, nullable: false),
			new DmoField("omcPriceRuleComplete", "bit", 1, 0, nullable: false),
			new DmoField("omcCartName", "nvarchar", 50, 0, nullable: false),
			new DmoField("omcCheckout", "bit", 1, 0, nullable: false),
			new DmoField("omcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("OMCSALESORDERCARTID", unique: true),
			new DmoIndex("omcCartID", unique: false),
			new DmoIndex("omcPartID", unique: false),
			new DmoIndex("omcConfigPartID", unique: false),
			new DmoIndex("omcPartRevisionID", unique: false),
			new DmoIndex("omcConfigPartRevisionID", unique: false),
			new DmoIndex("omcQuoteID", unique: false),
			new DmoIndex("omcQuoteLineID", unique: false),
			new DmoIndex("omcConfigured", unique: false),
			new DmoIndex("omcConfigComplete", unique: false),
			new DmoIndex("omcPriceRuleComplete", unique: false),
			new DmoIndex("omcUniqueID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
