using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARInvoiceMemoInformationDto
{
	public string ariArInvoiceID { get; set; }

	public string ariCreatedBy { get; set; }

	public DateTime? ariCreatedDate { get; set; }

	public Guid ariUniqueID { get; set; }

	public string ariLongDescriptionRtf { get; set; }

	public string ariLongDescriptionText { get; set; }

	public DateTime? ariMemoDate { get; set; }

	public byte[] ariRowVersion { get; set; }

	public short ariArInvoiceMemoID { get; set; }

	public string ariShortDescription { get; set; }

	public bool ariShowInArInvoices { get; set; }

	public bool ariShowInArPayments { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
