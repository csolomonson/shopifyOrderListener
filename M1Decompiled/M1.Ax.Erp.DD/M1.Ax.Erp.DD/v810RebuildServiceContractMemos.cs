using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ServiceContractMemos to support unicode", "2013-10-17")]
public class v810RebuildServiceContractMemos
{
	public v810RebuildServiceContractMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContractMemos", new DmoField[9]
		{
			new DmoField("kbmServiceContractID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbmServiceContractMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("kbmMemoDate", "date", 14, 0, nullable: true),
			new DmoField("kbmShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbmLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbmLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbmCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbmCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("KBMSERVICECONTRACTID,KBMSERVICECONTRACTMEMOID", unique: true),
			new DmoIndex("KBMUNIQUEID", unique: true),
			new DmoIndex("kbmServiceContractID", unique: false),
			new DmoIndex("kbmServiceContractMemoID", unique: false),
			new DmoIndex("kbmMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
