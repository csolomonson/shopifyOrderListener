using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartOrgReferences to support unicode", "2013-10-17")]
public class v810RebuildPartOrgReferences
{
	public v810RebuildPartOrgReferences(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOrgReferences", new DmoField[15]
		{
			new DmoField("imzPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imzPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imzOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imzOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imzOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imzPurchased", "bit", 1, 0, nullable: false),
			new DmoField("imzSold", "bit", 1, 0, nullable: false),
			new DmoField("imzPurchaseUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("imzConversionFactor", "numeric", 14, 8, nullable: false),
			new DmoField("imzMinimumPurchaseQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("imzLotSize", "numeric", 15, 5, nullable: false),
			new DmoField("imzInactive", "bit", 1, 0, nullable: false),
			new DmoField("imzCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imzCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imzUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("IMZPARTID,IMZPARTREVISIONID,IMZORGANIZATIONID", unique: true),
			new DmoIndex("IMZUNIQUEID", unique: true),
			new DmoIndex("imzPartID", unique: false),
			new DmoIndex("imzPartRevisionID", unique: false),
			new DmoIndex("imzOrganizationID", unique: false),
			new DmoIndex("imzOrgPartID", unique: false),
			new DmoIndex("imzInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
