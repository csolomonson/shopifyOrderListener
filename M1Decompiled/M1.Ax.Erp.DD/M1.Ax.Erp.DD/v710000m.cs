using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Plant to Projected Payments/Recurring Payments", "2008-05-13")]
public class v710000m
{
	public v710000m(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProjectedPayments", "gloPlantID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProjectedPayments", "gloPlantID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProjectedPayments", "gloPlantDepartmentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProjectedPayments", "gloPlantDepartmentID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprPlantID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprPlantID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprPlantDepartmentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprPlantDepartmentID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
