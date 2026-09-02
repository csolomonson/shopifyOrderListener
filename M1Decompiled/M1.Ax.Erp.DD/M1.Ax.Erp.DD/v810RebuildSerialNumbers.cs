using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SerialNumbers to support unicode", "2013-10-17")]
public class v810RebuildSerialNumbers
{
	public v810RebuildSerialNumbers(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", new DmoField[12]
		{
			new DmoField("imsPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imsPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imsSerialNumberID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imsPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imsPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imsStatus", "tinyint", 2, 0, nullable: false),
			new DmoField("imsExpirationDate", "date", 14, 0, nullable: true),
			new DmoField("imsAddedByUserID", "nvarchar", 20, 0, nullable: false),
			new DmoField("imsAddedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("IMSPARTID,IMSPARTREVISIONID,IMSSERIALNUMBERID", unique: true),
			new DmoIndex("IMSUNIQUEID", unique: true),
			new DmoIndex("imsPartID", unique: false),
			new DmoIndex("imsPartRevisionID", unique: false),
			new DmoIndex("imsSerialNumberID", unique: false),
			new DmoIndex("imsPartWarehouseLocationID", unique: false),
			new DmoIndex("imsPartBinID", unique: false),
			new DmoIndex("imsStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
