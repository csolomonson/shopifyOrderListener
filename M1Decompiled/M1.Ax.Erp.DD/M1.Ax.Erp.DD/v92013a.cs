using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.013", "Add fields to InspectionComponents table", "2016-11-08")]
public class v92013a
{
	public v92013a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamInvQuantityToInspect"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamInvQuantityToInspect", dropTriggers: true);
		}
	}
}
