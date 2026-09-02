using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.044", "Add ImplementationChecklist table", "2013-09-23")]
public class v810044h
{
	public v810044h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ImplementationCheckList"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ImplementationCheckList");
		}
	}
}
