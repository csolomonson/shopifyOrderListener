using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.50.018", "Resize serial number status fields", "2009-12-14")]
public class v750018a
{
	public v750018a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntStatus"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntStatus", "numeric", 2, 0, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsStatus"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsStatus", "numeric", 2, 0, parms.Messages);
		}
	}
}
