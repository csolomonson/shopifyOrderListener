using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartUnitSalePrices to support unicode", "2013-10-17")]
public class v810RebuildPartUnitSalePrices
{
	public v810RebuildPartUnitSalePrices(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartUnitSalePrices", new DmoField[11]
		{
			new DmoField("imhPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imhPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imhPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imhPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imhCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imhStartDate", "date", 14, 0, nullable: true),
			new DmoField("imhEndDate", "date", 14, 0, nullable: true),
			new DmoField("imhUnitSalePrice", "numeric", 15, 5, nullable: false),
			new DmoField("imhCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imhCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imhUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("IMHPARTID,IMHPARTREVISIONID,IMHPARTWAREHOUSELOCATIONID,IMHPARTBINID,IMHCURRENCYRATEID,IMHSTARTDATE", unique: true),
			new DmoIndex("IMHUNIQUEID", unique: true),
			new DmoIndex("imhPartID", unique: false),
			new DmoIndex("imhPartRevisionID", unique: false),
			new DmoIndex("imhPartWarehouseLocationID", unique: false),
			new DmoIndex("imhPartBinID", unique: false),
			new DmoIndex("imhCurrencyRateID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
