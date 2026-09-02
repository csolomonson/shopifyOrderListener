using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.012", "Alter field in DocumentLinks table", "2016-02-16")]
public class v91012a
{
	public v91012a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DocumentLinks", "xalFileName"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DocumentLinks", "xalFileName", "nvarchar(max)", 50, 0, isNullable: true, parms.Messages);
		}
	}
}
