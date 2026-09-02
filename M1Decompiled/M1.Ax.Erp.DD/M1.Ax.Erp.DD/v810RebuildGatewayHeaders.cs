using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GatewayHeaders to support unicode", "2013-10-17")]
public class v810RebuildGatewayHeaders
{
	public v810RebuildGatewayHeaders(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GatewayHeaders", new DmoField[2]
		{
			new DmoField("lmgGatewayHeaderID", "smallint", 4, 0, nullable: false),
			new DmoField("lmgDescription", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[1]
		{
			new DmoIndex("LMGGATEWAYHEADERID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
