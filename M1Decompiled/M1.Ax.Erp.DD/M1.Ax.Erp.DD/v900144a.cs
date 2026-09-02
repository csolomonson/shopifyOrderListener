using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.144", "Alter fields in DocumentLinks table", "2016-03-03")]
public class v900144a
{
	public v900144a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DocumentLinks", "xalDescription"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DocumentLinks", "xalDescription", "nvarchar", 255, 0, isNullable: false, parms.Messages);
		}
	}
}
