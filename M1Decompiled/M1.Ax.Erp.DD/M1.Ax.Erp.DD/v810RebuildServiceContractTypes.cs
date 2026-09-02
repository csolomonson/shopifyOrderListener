using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ServiceContractTypes to support unicode", "2013-10-17")]
public class v810RebuildServiceContractTypes
{
	public v810RebuildServiceContractTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContractTypes", new DmoField[7]
		{
			new DmoField("kbyServiceContractTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("kbyDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbyInactive", "bit", 1, 0, nullable: false),
			new DmoField("kbyInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("kbyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("KBYSERVICECONTRACTTYPEID", unique: true),
			new DmoIndex("KBYUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
