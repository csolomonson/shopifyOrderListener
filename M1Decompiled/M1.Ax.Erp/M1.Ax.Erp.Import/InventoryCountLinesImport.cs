using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Import;

[ImportProcessing("InventoryCountLines")]
public class InventoryCountLinesImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into InventoryCounts (imnInventoryCountID,imnStatus) Select imqInventoryCountID,2 From InventoryCountLines Where imqInventoryCountID Not In (Select imnInventoryCountID From InventoryCounts) And imqInventoryCountID In (Select imqInventoryCountID From " + parm.TempTable + ") Group By imqInventoryCountID"));
	}
}
