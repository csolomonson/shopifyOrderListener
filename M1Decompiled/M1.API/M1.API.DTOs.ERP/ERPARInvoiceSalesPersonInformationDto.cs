using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARInvoiceSalesPersonInformationDto
{
	public decimal arjAmount { get; set; }

	public string arjArInvoiceID { get; set; }

	public string arjCreatedBy { get; set; }

	public DateTime? arjCreatedDate { get; set; }

	public Guid arjUniqueID { get; set; }

	public bool arjPostedToGl { get; set; }

	public decimal arjPercent { get; set; }

	public decimal arjRate { get; set; }

	public byte[] arjRowVersion { get; set; }

	public string arjSalesEmployeeID { get; set; }

	public short arjSequenceID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
