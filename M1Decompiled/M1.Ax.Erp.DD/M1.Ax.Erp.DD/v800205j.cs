using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Alter state field in ServiceContractOwners", "2011-12-06")]
public class v800205j
{
	public v800205j(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ServiceContractOwners", "kboPhysicalLocationState"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContractOwners", "kboPhysicalLocationState", "char", 3, 0, parms.Messages);
		}
	}
}
