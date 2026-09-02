using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.154", "Update Implementation Checklist", "2011-08-23")]
public class v800154
{
	public v800154(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call Forms.Show.UserAdministration' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 2");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call App.PropsShowDataset' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 60");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call App.PropsShowProduction' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 61");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call App.PropsShowFinancial' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 62");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call App.PropsShowShipping' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 63");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call App.PropsShowDataCollection' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 64");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update workflowlines SET wflCode = 'Call App.PropsShowWebGear' WHERE wflWorkFlowID = 'IMPLCHECKL' and wflWorkFlowLineID = 65");
	}
}
