using System;
using System.Collections.Generic;
using M1.API.DTOs.BOM.Transaction;

namespace M1.API.DTOs.Custom;

public class ReceiptDto
{
	public string ReceiptID { get; set; }

	public DateTime? ReceiptDate { get; set; }

	public string DeliveryDocket { get; set; }

	public string SupplierOrganizationID { get; set; }

	public string PurchaseLocationID { get; set; }

	public string ApInvoiceLocationID { get; set; }

	public string ApInvoiceContactID { get; set; }

	public string ShippingMethodID { get; set; }

	public decimal ReceiptSubtotal { get; set; }

	public decimal FreightCharge { get; set; }

	public decimal ReceiptTotal { get; set; }

	public string ProjectID { get; set; }

	public string CurrencyRateID { get; set; }

	public decimal ExchangeRate { get; set; }

	public bool CustomRate { get; set; }

	public bool ReversalEntry { get; set; }

	public bool Reversed { get; set; }

	public bool PostedToGl { get; set; }

	public DateTime? PostedDate { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; } = DateTime.Now;

	public bool Closed { get; set; }

	public DateTime? ClosedDate { get; set; }

	public Guid UniqueID { get; set; }

	public string PlantDepartmentID { get; set; }

	public string PlantID { get; set; }

	public byte[] RowVersion { get; set; }

	public List<BOMReceiptLineDto> ReceiptLines { get; set; } = new List<BOMReceiptLineDto>();
}
