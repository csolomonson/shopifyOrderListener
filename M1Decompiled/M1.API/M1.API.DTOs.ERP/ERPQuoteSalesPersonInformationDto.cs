using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteSalesPersonInformationDto
{
	public string qmjCreatedBy { get; set; }

	public DateTime? qmjCreatedDate { get; set; }

	public Guid qmjUniqueID { get; set; }

	public bool qmjClosed { get; set; }

	public bool qmjCreatedFromMobile { get; set; }

	public decimal qmjPercent { get; set; }

	public string qmjQuoteID { get; set; }

	public byte[] qmjRowVersion { get; set; }

	public string qmjSalesEmployeeID { get; set; }

	public short qmjSequenceID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
