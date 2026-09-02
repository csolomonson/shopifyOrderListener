using System;

namespace M1.API.DTOs.Custom;

public class MaterialIssueLineInformationDto
{
	public string MaterialIssueID { get; set; }

	public short MaterialIssueLineID { get; set; }

	public string JobID { get; set; }

	public int JobAssemblyID { get; set; }

	public bool CreateJobSeq { get; set; }

	public int JobMaterialID { get; set; }

	public byte JobType { get; set; }

	public decimal EstimatedQuantity { get; set; }

	public decimal JobOpenQuantity { get; set; }

	public bool IssueComplete { get; set; }

	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string PartWarehouseLocationID { get; set; }

	public string PartBinID { get; set; }

	public decimal InvIssueQuantity { get; set; }

	public decimal InvScrapQuantity { get; set; }

	public string Reference { get; set; }

	public string HeatLot { get; set; }

	public string MiscIssueReasonID { get; set; }

	public string ProjectID { get; set; }

	public string ProjectAreaID { get; set; }

	public decimal JobMatScrapQuantity { get; set; }

	public decimal JobAsmScrapQuantity { get; set; }

	public decimal JobAsmIssueQuantity { get; set; }

	public decimal JobMatIssueQuantity { get; set; }

	public decimal JobMatReturnScrapQuantity { get; set; }

	public decimal JobMatReturnIssueQuantity { get; set; }

	public string ReverseMaterialIssueID { get; set; }

	public short ReverseMaterialIssueLineID { get; set; }

	public bool Reversed { get; set; }

	public bool KitPart { get; set; }

	public bool Posted { get; set; }

	public byte IssueType { get; set; }

	public string LongDescriptionText { get; set; }

	public string PlantID { get; set; }

	public decimal QuantityAllocated { get; set; }

	public decimal QuantityOnHand { get; set; }

	public Guid UniqueID { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public byte[] RowVersion { get; set; }
}
