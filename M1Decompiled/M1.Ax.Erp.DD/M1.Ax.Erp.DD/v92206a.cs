using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.206", "Update ImplementationCheckList actions", "2017-03-30")]
public class v92206a
{
	public v92206a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'App.OpenObject', 'Forms.OpenObject') where xicAction is not null and xicAction like '%App.OpenObject%';");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'App.OpenForm', 'Forms.OpenForm') where xicAction is not null and xicAction like '%App.OpenForm%';");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.ShowUserAdministrationForm', 'Forms.OpenForm \"M1.Forms.User.Administration.UserAdministrationForm\"') where xicAction is not null and xicAction like '%Call App.ShowUserAdministrationForm%';");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")', 'Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"') where xicAction is not null and xicAction like '%Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")%';");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.Ax(\"PayrollFunctions\").ShowLoadTaxTablesForm(\"Import\")', 'Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"') where xicAction is not null and xicAction like '%Call App.Ax(\"PayrollFunctions\").ShowLoadTaxTablesForm(\"Import\")%';");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.PropsShowDataset', 'Call Forms.Show.DatabaseOptions') where xicAction is not null and xicAction like '%Call App.PropsShowDataset%';");
	}
}
