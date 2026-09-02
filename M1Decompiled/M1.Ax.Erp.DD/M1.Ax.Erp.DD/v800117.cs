using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.117", "Set default value for Purchasing Wizard Display Type", "2011-03-11")]
public class v800117
{
	public v800117(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapPMPOWizardDisplayType = 1 WHERE xapPMPOWizardDisplayType = 0");
	}
}
