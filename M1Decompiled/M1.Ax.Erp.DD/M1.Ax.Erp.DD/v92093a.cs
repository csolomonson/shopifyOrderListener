using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.093", "Add field to DatasetProperties table", "2017-02-02")]
public class v92093a
{
	public v92093a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadVersion92UpgradeDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadVersion92UpgradeDate", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadVersion92UpgradeDate = GetDate();");
		}
	}
}
