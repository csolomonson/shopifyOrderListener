using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RFQQuantities to support unicode", "2013-10-17")]
public class v810RebuildRFQQuantities
{
	public v810RebuildRFQQuantities(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RFQQuantities", new DmoField[12]
		{
			new DmoField("rqqRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqqRFQLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rqqRFQSupplierID", "smallint", 4, 0, nullable: false),
			new DmoField("rqqRFQQuantityID", "smallint", 4, 0, nullable: false),
			new DmoField("rqqQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("rqqPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("rqqPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("rqqLeadTime", "smallint", 3, 0, nullable: false),
			new DmoField("rqqClosed", "bit", 1, 0, nullable: false),
			new DmoField("rqqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rqqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rqqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("RQQRFQID,RQQRFQLINEID,RQQRFQSUPPLIERID,RQQRFQQUANTITYID", unique: true),
			new DmoIndex("RQQUNIQUEID", unique: true),
			new DmoIndex("rqqRFQID", unique: false),
			new DmoIndex("rqqRFQLineID", unique: false),
			new DmoIndex("rqqRFQSupplierID", unique: false),
			new DmoIndex("rqqRFQQuantityID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
