using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.024", "Add fields to InspectionComponents table", "2016-11-21")]
public class v92024b
{
	public v92024b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamComponentQtyToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamComponentQtyToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamComponentQtyToInspect"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionComponents Set qamComponentQtyToInspect = qamParentQtyToInspect*qamQuantityPerParent");
		}
	}
}
