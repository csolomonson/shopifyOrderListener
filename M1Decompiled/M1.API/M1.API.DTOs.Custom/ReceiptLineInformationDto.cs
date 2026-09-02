using System;

namespace M1.API.DTOs.Custom;

public class ReceiptLineInformationDto
{
	public short ReceiptLineID { get; set; }

	public string ReceiptID { get; set; }

	public string PurchaseOrderID { get; set; }

	public short PurchaseOrderLineID { get; set; }

	public string JobID { get; set; }

	public int JobAssemblyID { get; set; }

	public byte JobType { get; set; }

	public int JobMaterialID { get; set; }

	public int JobOperationID { get; set; }

	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string OrgPartID { get; set; }

	public string OrgPartShortDescription { get; set; }

	public string Description { get; set; }

	public string PartWarehouseLocationID { get; set; }

	public string PartBinID { get; set; }

	public decimal PurchaseQuantityReceived { get; set; }

	public string PurchaseUnitOfMeasure { get; set; }

	public decimal PurchaseUnitCost { get; set; }

	public decimal SetupCharge { get; set; }

	public decimal ConversionFactor { get; set; }

	public string InventoryUnitOfMeasure { get; set; }

	public decimal InventoryUnitCost { get; set; }

	public bool PoReceivedComplete { get; set; }

	public bool JobReceivedComplete { get; set; }

	public bool RequiresInspection { get; set; }

	public string Reference { get; set; }

	public string HeatLot { get; set; }

	public string ProjectID { get; set; }

	public string ProjectAreaID { get; set; }

	public bool Closed { get; set; }

	public bool PostedToGl { get; set; }

	public bool Reversed { get; set; }

	public string ReverseReceiptID { get; set; }

	public short ReverseReceiptLineID { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public Guid UniqueID { get; set; }

	public byte[] RowVersion { get; set; }

	public decimal JobOprQuantityReceived { get; set; }

	public decimal JobMatQuantityReceived { get; set; }
}
