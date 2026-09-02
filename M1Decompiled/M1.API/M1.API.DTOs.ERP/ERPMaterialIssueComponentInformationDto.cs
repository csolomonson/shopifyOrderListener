using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMaterialIssueComponentInformationDto
{
	public decimal inkAdditionalQuantity { get; set; }

	public string inkCreatedBy { get; set; }

	public DateTime? inkCreatedDate { get; set; }

	public string inkDescription { get; set; }

	public Guid inkUniqueID { get; set; }

	public decimal inkInvIssueQuantity { get; set; }

	public decimal inkInvParentQuantity { get; set; }

	public decimal inkInvParentQuantityScrap { get; set; }

	public decimal inkInvScrapQuantity { get; set; }

	public bool inkPosted { get; set; }

	public bool inkReceivedComplete { get; set; }

	public bool inkReversed { get; set; }

	public int inkJobAssemblyID { get; set; }

	public string inkJobID { get; set; }

	public int inkJobMaterialComponentID { get; set; }

	public int inkJobMaterialID { get; set; }

	public decimal inkJobMatIssueQuantity { get; set; }

	public decimal inkJobMatParentQuantity { get; set; }

	public decimal inkJobMatParentQuantityScrap { get; set; }

	public decimal inkJobMatParentReturnQty { get; set; }

	public decimal inkJobMatParentReturnQtyScrap { get; set; }

	public decimal inkJobMatReturnIssueQuantity { get; set; }

	public decimal inkJobMatReturnScrapQuantity { get; set; }

	public decimal inkJobMatScrapQuantity { get; set; }

	public string inkMaterialIssueID { get; set; }

	public short inkMaterialIssueLineID { get; set; }

	public string inkPartBinID { get; set; }

	public string inkPartID { get; set; }

	public string inkPartRevisionID { get; set; }

	public string inkPartWarehouseLocationID { get; set; }

	public decimal inkQuantityPerParent { get; set; }

	public int inkReverseMaterialIssueCompID { get; set; }

	public string inkReverseMaterialIssueID { get; set; }

	public short inkReverseMaterialIssueLineID { get; set; }

	public byte[] inkRowVersion { get; set; }

	public int inkMaterialIssueComponentID { get; set; }

	public string inkUnitOfMeasure { get; set; }

	public decimal inkWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
