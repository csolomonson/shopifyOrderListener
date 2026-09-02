using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("Warehouses")]
public class WarehousesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (!parm.NewIDExists && ((string)parm.NewKeyValues[0]).Trim().Length == 0)
		{
			parm.LastKeyCanBeEmpty = true;
			parm.NewIDExists = true;
		}
		parm.DeleteStatements.AppendLine("DELETE FROM PartWarehouseLocations WHERE imlPartWarehouseID = " + parm.OldKeyValues[0].ToSql() + " AND imlPartID+imlPartRevisionID IN (SELECT imlPartID+imlPartRevisionID FROM PartWarehouseLocations WHERE imlPartWarehouseID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartBins WHERE imbWarehouseID = " + parm.OldKeyValues[0].ToSql() + " AND imbPartID+imbPartRevisionID+imbPartBinID IN (SELECT imbPartID+imbPartRevisionID+imbPartBinID FROM PartBins WHERE imbWarehouseID = " + parm.NewKeyValues[0].ToSql() + ")");
		if (parm.NewKeyValues[0].ToString().Length == 0)
		{
			parm.DeleteStatements.AppendLine("DELETE FROM Warehouses WHERE imwWarehouseID = " + parm.OldKeyValues[0].ToSql());
		}
		if (parm.NewIDExists)
		{
			parm.UpdateStatements.AppendLine("UPDATE PartBins SET PartBins.imbQuantityOnHand = PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand, imbBinQuantityOnHand = CASE WHEN imbConversionFactor = 0 THEN PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand ELSE (PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand) / imbConversionFactor END, PartBins.imbQuantityAllocated = PartBins.imbQuantityAllocated + temppart.imbQuantityAllocated  From PartBins Inner Join (select imbPartID,imbPartRevisionID," + parm.NewKeyValues[0].ToSql() + " As imbWarehouseID,imbPartBinID, imbQuantityOnHand, imbQuantityAllocated from PartBins Where imbWarehouseID = " + parm.OldKeyValues[0].ToSql() + ") as temppart On PartBins.imbPartID = temppart.imbPartID and PartBins.imbPartRevisionID = temppart.imbPartRevisionID and PartBins.imbWarehouseID = temppart.imbWarehouseID and PartBins.imbPartBinID = temppart.imbPartBinID");
		}
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
