using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LotNumbers to support unicode", "2013-10-17")]
public class v810RebuildLotNumbers
{
	public v810RebuildLotNumbers(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", new DmoField[14]
		{
			new DmoField("ablPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("ablPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("ablPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ablPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("ablLotNumberID", "nvarchar", 30, 0, nullable: false),
			new DmoField("ablStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("ablQuantityOnHand", "numeric", 15, 5, nullable: false),
			new DmoField("ablAddedByUserID", "nvarchar", 20, 0, nullable: false),
			new DmoField("ablAddedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ablQuantityToInspect", "numeric", 15, 5, nullable: false),
			new DmoField("ablQuantityToReturn", "numeric", 15, 5, nullable: false),
			new DmoField("ablCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ablCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ablUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("ABLPARTID,ABLPARTREVISIONID,ABLPARTWAREHOUSELOCATIONID,ABLPARTBINID,ABLLOTNUMBERID", unique: true),
			new DmoIndex("ABLUNIQUEID", unique: true),
			new DmoIndex("ablPartID", unique: false),
			new DmoIndex("ablPartRevisionID", unique: false),
			new DmoIndex("ablPartWarehouseLocationID", unique: false),
			new DmoIndex("ablPartBinID", unique: false),
			new DmoIndex("ablLotNumberID", unique: false),
			new DmoIndex("ablStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
