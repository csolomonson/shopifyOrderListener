using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAgingBucketInformationDto
{
	public int xaaBucket1DaysOver { get; set; }

	public string xaaBucket1Description { get; set; }

	public int xaaBucket2DaysOver { get; set; }

	public string xaaBucket2Description { get; set; }

	public int xaaBucket3DaysOver { get; set; }

	public string xaaBucket3Description { get; set; }

	public int xaaBucket4DaysOver { get; set; }

	public string xaaBucket4Description { get; set; }

	public int xaaBucket5DaysOver { get; set; }

	public string xaaBucket5Description { get; set; }

	public byte xaaCalculationType { get; set; }

	public string xaaAgingBucketID { get; set; }

	public string xaaCreatedBy { get; set; }

	public DateTime? xaaCreatedDate { get; set; }

	public string xaaDescription { get; set; }

	public Guid xaaUniqueID { get; set; }

	public byte[] xaaRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
