using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Import;

[ImportProcessing("PartOrgReferences")]
public class PartOrgReferencesImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into PartCrossReferences (imxPartID,imxPartRevisionID,imxOrganizationID,imxLocationID) Select imzPartID,imzPartRevisionID,imzOrganizationID,'' As imzLocationID From PartOrgReferences Where imzPartID+imzPartRevisionID+imzOrganizationID Not In (Select imxPartID+imxPartRevisionID+imxOrganizationID From PartCrossReferences Where imxLocationID = '') And imzPartID+imzPartRevisionID+imzOrganizationID In (Select imzPartID+imzPartRevisionID+imzOrganizationID From " + parm.TempTable + ") Group By imzPartID,imzPartRevisionID,imzOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Update PartCrossReferences Set imxOrgPartID = dest.imzOrgPartID, imxPurchased = dest.imzPurchased, imxSold = dest.imzSold, imxPurchaseUnitOfMeasure = dest.imzPurchaseUnitOfMeasure, imxConversionFactor = dest.imzConversionFactor, imxMinimumPurchaseQuantity = dest.imzMinimumPurchaseQuantity, imxLotSize = dest.imzLotSize, imxInactive = dest.imzInactive, imxOrgPartShortDescription = dest.imzOrgPartShortDescription From PartCrossReferences Inner Join PartOrgReferences dest On imxPartID = imzPartID And imxPartRevisionID = imzPartRevisionID And imxOrganizationID = imzOrganizationID And imxLocationID = '' Inner Join " + parm.TempTable + " On dest.imzPartID = " + parm.TempTable + ".imzPartID And dest.imzPartRevisionID = " + parm.TempTable + ".imzPartRevisionID And dest.imzOrganizationID = " + parm.TempTable + ".imzOrganizationID"));
	}
}
