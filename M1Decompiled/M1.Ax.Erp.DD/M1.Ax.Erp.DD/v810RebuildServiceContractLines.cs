using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ServiceContractLines to support unicode", "2013-10-17")]
public class v810RebuildServiceContractLines
{
	public v810RebuildServiceContractLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContractLines", new DmoField[13]
		{
			new DmoField("kbnServiceContractID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbnServiceContractLineID", "smallint", 4, 0, nullable: false),
			new DmoField("kbnSerialNumberID", "nvarchar", 30, 0, nullable: false),
			new DmoField("kbnPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("kbnPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("kbnPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbnContractLength", "smallint", 4, 0, nullable: false),
			new DmoField("kbnContractLengthType", "nvarchar", 1, 0, nullable: false),
			new DmoField("kbnStartDate", "date", 14, 0, nullable: true),
			new DmoField("kbnEndDate", "date", 14, 0, nullable: true),
			new DmoField("kbnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("KBNSERVICECONTRACTID,KBNSERVICECONTRACTLINEID", unique: true),
			new DmoIndex("KBNUNIQUEID", unique: true),
			new DmoIndex("kbnServiceContractID", unique: false),
			new DmoIndex("kbnServiceContractLineID", unique: false),
			new DmoIndex("kbnSerialNumberID", unique: false),
			new DmoIndex("kbnPartID", unique: false),
			new DmoIndex("kbnPartRevisionID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
