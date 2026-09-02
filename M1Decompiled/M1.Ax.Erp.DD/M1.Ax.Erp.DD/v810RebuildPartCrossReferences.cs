using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartCrossReferences to support unicode", "2013-10-17")]
public class v810RebuildPartCrossReferences
{
	public v810RebuildPartCrossReferences(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartCrossReferences", new DmoField[16]
		{
			new DmoField("imxPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imxPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imxOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imxLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imxOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imxOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imxPurchased", "bit", 1, 0, nullable: false),
			new DmoField("imxSold", "bit", 1, 0, nullable: false),
			new DmoField("imxPurchaseUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("imxConversionFactor", "numeric", 14, 8, nullable: false),
			new DmoField("imxMinimumPurchaseQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("imxLotSize", "numeric", 15, 5, nullable: false),
			new DmoField("imxInactive", "bit", 1, 0, nullable: false),
			new DmoField("imxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("IMXPARTID,IMXPARTREVISIONID,IMXORGANIZATIONID,IMXLOCATIONID", unique: true),
			new DmoIndex("IMXUNIQUEID", unique: true),
			new DmoIndex("imxPartID", unique: false),
			new DmoIndex("imxPartRevisionID", unique: false),
			new DmoIndex("imxOrganizationID", unique: false),
			new DmoIndex("imxLocationID", unique: false),
			new DmoIndex("imxOrgPartID", unique: false),
			new DmoIndex("imxInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
