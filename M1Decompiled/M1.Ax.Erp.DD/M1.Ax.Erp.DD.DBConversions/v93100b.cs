using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.3.100", "Update MaxLength of xadMailProvider field to DatasetProperties table", "2021-02-11")]
public class v93100b
{
	public v93100b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadMailProvider"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadMailProvider", "nvarchar", 15, 0, isNullable: false, parms.Messages);
		}
	}
}
