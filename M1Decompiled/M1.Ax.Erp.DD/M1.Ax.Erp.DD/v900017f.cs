using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.017", "Add fields to Form941Years table", "2015-02-12")]
public class v900017f
{
	public v900017f(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form941Years", "ptyEIN"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form941Years", "ptyEIN", "nvarchar", 20, 0, isNullable: false, parms.Messages);
		}
	}
}
