using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPExpenseInformationDto
{
	public string lmxExpenseID { get; set; }

	public string lmxCreatedBy { get; set; }

	public DateTime? lmxCreatedDate { get; set; }

	public string lmxDescription { get; set; }

	public Guid lmxUniqueID { get; set; }

	public byte[] lmxRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
