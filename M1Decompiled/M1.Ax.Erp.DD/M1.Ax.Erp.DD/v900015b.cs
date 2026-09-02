using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.015", "Update set up users action for implementation checklist", "2015-01-27")]
public class v900015b
{
	public v900015b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ImplementationCheckList", "xicAction"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList set xicAction = 'Forms.OpenForm \"M1.Forms.User.Administration.UserAdministrationForm\"' where xicImplementationCheckListID = 2");
		}
	}
}
