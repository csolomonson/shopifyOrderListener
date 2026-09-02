using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.020", "Add fields to ProductionProperties table", "2015-02-20")]
public class v900020b
{
	public v900020b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapRQDefaultDueDate") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapPMDefaultDueDate"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapRQDefaultDueDate", "xapPMDefaultDueDate", dropTriggers: true);
		}
	}
}
