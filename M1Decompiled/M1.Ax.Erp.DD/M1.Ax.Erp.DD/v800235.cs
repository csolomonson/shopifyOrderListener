using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.235", "Correct form call in WorkFlowLines table for Implementation Checklist", "2012-03-15")]
public class v800235
{
	public v800235(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLines", "wflCode"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE WorkFlowLines SET wflCode = 'Call Forms.Show.UserAdministration' WHERE wflCode LIKE '%frmUserAdmin%'");
		}
	}
}
