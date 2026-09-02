using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("PartWarehouseLocations")]
public class PartWarehouseLocationsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.ChangeIDType != 1)
		{
			new Part().RefreshPreviousQOH(parm.Database, parm.SqlTransaction, " AND imtPartID = " + parm.NewKeyValues[0].ToSql() + " AND imtPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " AND imtPartWarehouseLocationID = " + parm.NewKeyValues[2].ToSql());
		}
	}
}
