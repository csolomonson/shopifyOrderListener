using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPNextIDInformationDto
{
	public byte xanAutoIncrement { get; set; }

	public string xanCreatedBy { get; set; }

	public DateTime? xanCreatedDate { get; set; }

	public string xanDatasets { get; set; }

	public Guid xanUniqueID { get; set; }

	public short xanIncrementAmount { get; set; }

	public byte xanLogChanges { get; set; }

	public string xanNextID { get; set; }

	public byte xanNumericOnly { get; set; }

	public byte[] xanRowVersion { get; set; }

	public string xanTable { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
