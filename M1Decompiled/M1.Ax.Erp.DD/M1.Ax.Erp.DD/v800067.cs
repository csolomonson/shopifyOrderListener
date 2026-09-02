using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.067", "Add ARPayflowPro Table", "2010-09-20")]
public class v800067
{
	public v800067(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ARPayflowPro"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPayflowPro");
		}
	}
}
