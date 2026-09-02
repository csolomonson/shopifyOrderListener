using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GatewayPrompts to support unicode", "2013-10-17")]
public class v810RebuildGatewayPrompts
{
	public v810RebuildGatewayPrompts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GatewayPrompts", new DmoField[9]
		{
			new DmoField("lmzGatewayPromptID", "smallint", 4, 0, nullable: false),
			new DmoField("lmzGatewayHeaderID", "smallint", 4, 0, nullable: false),
			new DmoField("lmzDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmzRequired", "bit", 1, 0, nullable: false),
			new DmoField("lmzDisabled", "bit", 1, 0, nullable: false),
			new DmoField("lmzDisplayedSequence", "smallint", 4, 0, nullable: false),
			new DmoField("lmzErrorMessage", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmzErrorResolution", "smallint", 4, 0, nullable: false),
			new DmoField("lmzDefault", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[1]
		{
			new DmoIndex("LMZGATEWAYHEADERID,LMZGATEWAYPROMPTID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
