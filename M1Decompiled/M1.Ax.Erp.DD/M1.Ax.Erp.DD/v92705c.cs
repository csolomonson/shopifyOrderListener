using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.705", "Remove fields from MRPLines table", "2018-05-21")]
public class v92705c
{
	public v92705c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlMaximumQuantity"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlMaximumQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlQuantityOnHand"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlQuantityOnHand", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlMinimumQuantity"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlMinimumQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlLotSize"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlLotSize", dropTriggers: true);
		}
	}
}
