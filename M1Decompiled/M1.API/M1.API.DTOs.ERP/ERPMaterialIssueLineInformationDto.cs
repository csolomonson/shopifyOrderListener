using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMaterialIssueLineInformationDto
{
	public string injCreatedBy { get; set; }

	public DateTime? injCreatedDate { get; set; }

	public Guid injUniqueID { get; set; }

	public decimal injEstimatedQuantity { get; set; }

	public string injHeatLot { get; set; }

	public decimal injInvIssueQuantity { get; set; }

	public decimal injInvScrapQuantity { get; set; }

	public bool injCreateJobSeq { get; set; }

	public bool injIssueComplete { get; set; }

	public bool injKitPart { get; set; }

	public bool injPosted { get; set; }

	public bool injReversed { get; set; }

	public byte injIssueType { get; set; }

	public decimal injJobAsmIssueQuantity { get; set; }

	public decimal injJobAsmScrapQuantity { get; set; }

	public int injJobAssemblyID { get; set; }

	public string injJobID { get; set; }

	public int injJobMaterialID { get; set; }

	public decimal injJobMatIssueQuantity { get; set; }

	public decimal injJobMatReturnIssueQuantity { get; set; }

	public decimal injJobMatReturnScrapQuantity { get; set; }

	public decimal injJobMatScrapQuantity { get; set; }

	public decimal injJobOpenQuantity { get; set; }

	public byte injJobType { get; set; }

	public string injLongDescriptionRtf { get; set; }

	public string injLongDescriptionText { get; set; }

	public string injMaterialIssueID { get; set; }

	public string injMiscIssueReasonID { get; set; }

	public string injPartBinID { get; set; }

	public string injPartID { get; set; }

	public string injPartRevisionID { get; set; }

	public string injPartWarehouseLocationID { get; set; }

	public string injPlantID { get; set; }

	public string injProjectAreaID { get; set; }

	public string injProjectID { get; set; }

	public decimal injQuantityAllocated { get; set; }

	public decimal injQuantityOnHand { get; set; }

	public string injReference { get; set; }

	public string injReverseMaterialIssueID { get; set; }

	public short injReverseMaterialIssueLineID { get; set; }

	public byte[] injRowVersion { get; set; }

	public short injMaterialIssueLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
