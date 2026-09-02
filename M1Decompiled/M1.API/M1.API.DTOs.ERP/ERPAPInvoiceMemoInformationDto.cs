using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPInvoiceMemoInformationDto
{
	public string apiApInvoiceID { get; set; }

	public string apiCreatedBy { get; set; }

	public DateTime? apiCreatedDate { get; set; }

	public Guid apiUniqueID { get; set; }

	public string apiLongDescriptionRtf { get; set; }

	public string apiLongDescriptionText { get; set; }

	public DateTime? apiMemoDate { get; set; }

	public byte[] apiRowVersion { get; set; }

	public short apiApInvoiceMemoID { get; set; }

	public string apiShortDescription { get; set; }

	public bool apiShowInApInvoices { get; set; }

	public bool apiShowInApPayments { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
