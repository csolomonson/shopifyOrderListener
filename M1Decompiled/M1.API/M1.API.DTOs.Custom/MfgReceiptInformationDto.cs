using System;

namespace M1.API.DTOs.Custom;

public class MfgReceiptInformationDto
{
	public string MfgReceiptID { get; set; }

	public byte ReceiptType { get; set; }

	public DateTime? ReceiptDate { get; set; }

	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string PartWarehouseLocationID { get; set; }

	public string PartBinID { get; set; }

	public bool Posted { get; set; }

	public DateTime? PostedDate { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public Guid UniqueID { get; set; }

	public byte[] RowVersion { get; set; }

	public string ProjectID { get; set; }

	public string ProjectAreaID { get; set; }

	public decimal MiscInvQuantityReceived { get; set; }

	public decimal InventoryQuantityReceived { get; set; }

	public decimal JobOprQuantityReceived { get; set; }

	public decimal JobAsmQuantityReceived { get; set; }

	public decimal JobMatQuantityReceived { get; set; }

	public string Reference { get; set; }

	public string HeatLot { get; set; }
}
