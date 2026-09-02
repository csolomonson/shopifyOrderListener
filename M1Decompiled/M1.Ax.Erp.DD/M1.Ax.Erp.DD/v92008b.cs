using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.008", "Remove the require warehouse field to DatasetProperties table", "2016-11-02")]
public class v92008b
{
	public v92008b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadRequireWarehouse"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadRequireWarehouse", dropTriggers: true);
		}
	}
}
