using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.169", "Update Implementation Checklist", "2011-09-21")]
public class v800169
{
	public v800169(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Forms.OpenObject \"PaymentTerm\"' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 12");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Forms.OpenObject \"Aging\"' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 13");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Forms.OpenObject \"ContactMethod\"' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 23");
	}
}
