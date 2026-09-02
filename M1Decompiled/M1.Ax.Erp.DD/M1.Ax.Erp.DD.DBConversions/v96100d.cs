using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.100", "Changing length on Object Data Run field in Top Activities Log table", "2023-02-26")]
public class v96100d
{
	public v96100d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TopActivitiesLog", "rxlObjectDataRun"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TopActivitiesLog", "rxlObjectDataRun", "nvarchar(max)", 50, 0, isNullable: false, parms.Messages);
		}
	}
}
