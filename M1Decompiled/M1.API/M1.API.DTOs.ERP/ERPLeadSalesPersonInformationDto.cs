using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLeadSalesPersonInformationDto
{
	public string lojCreatedBy { get; set; }

	public DateTime? lojCreatedDate { get; set; }

	public Guid lojUniqueID { get; set; }

	public string lojLeadID { get; set; }

	public decimal lojPercent { get; set; }

	public byte[] lojRowVersion { get; set; }

	public string lojSalesEmployeeID { get; set; }

	public short lojSequenceID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
