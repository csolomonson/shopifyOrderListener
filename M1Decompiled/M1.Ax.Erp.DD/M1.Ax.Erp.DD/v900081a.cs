using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.081", "Add fields to Organizations table", "2015-09-14")]
public class v900081a
{
	public v900081a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoAddressValidationResult"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoAddressValidationResult", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
