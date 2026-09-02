using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Import;

[ImportProcessing("PartWarehouseLocations")]
public class PartWarehouseLocationsImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		parm.Database.ExecuteCommand(new SqlCommand("Update PartRevisions Set imrMinimumQuantity = (Select Sum(IsNull(imlMinimumQuantity,0)) From " + parm.TempTable + " Where imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID Group By imlPartID,imlPartRevisionID), imrMaximumQuantity = (Select Sum(IsNull(imlMaximumQuantity,0)) From " + parm.TempTable + " Where imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID Group By imlPartID,imlPartRevisionID) Where imrPartID+imrPartRevisionID In (Select imlPartID+imlPartRevisionID From " + parm.TempTable + ")"));
	}
}
