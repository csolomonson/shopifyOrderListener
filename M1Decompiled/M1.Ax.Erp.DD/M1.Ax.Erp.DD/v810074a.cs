using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.074", "Remove type from FormDefinitions table", "2014-07-20")]
public class v810074a
{
	public v810074a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FormDefinitions", "xaoType"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoCustom = Case When xaoType = 2 Then 1 Else 0 End");
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormDefinitions", "xaoType", dropTriggers: true);
		}
	}
}
