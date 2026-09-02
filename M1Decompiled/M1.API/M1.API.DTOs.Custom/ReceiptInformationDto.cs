using System;

namespace M1.API.DTOs.Custom;

public class ReceiptInformationDto
{
	public string ReceiptID { get; set; } = string.Empty;

	public DateTime? ReceiptDate { get; set; }

	public string DeliveryDocket { get; set; } = string.Empty;

	public string SupplierOrganizationID { get; set; } = string.Empty;

	public string PurchaseLocationID { get; set; } = string.Empty;

	public string ApInvoiceLocationID { get; set; } = string.Empty;

	public string ApInvoiceContactID { get; set; } = string.Empty;

	public string ShippingMethodID { get; set; } = string.Empty;

	public decimal ReceiptSubtotal { get; set; }

	public decimal FreightCharge { get; set; }

	public decimal ReceiptTotal { get; set; }

	public string ProjectID { get; set; } = string.Empty;

	public string CurrencyRateID { get; set; } = string.Empty;

	public decimal ExchangeRate { get; set; }

	public bool CustomRate { get; set; }

	public bool ReversalEntry { get; set; }

	public bool Reversed { get; set; }

	public bool PostedToGl { get; set; }

	public DateTime? PostedDate { get; set; }

	public string CreatedBy { get; set; } = string.Empty;

	public DateTime? CreatedDate { get; set; } = DateTime.Now;

	public bool Closed { get; set; }

	public DateTime? ClosedDate { get; set; }

	public Guid UniqueID { get; set; }

	public string PlantDepartmentID { get; set; }

	public string PlantID { get; set; }

	public byte[] RowVersion { get; set; }

	public bool NestlinkProcessed { get; set; }
}
