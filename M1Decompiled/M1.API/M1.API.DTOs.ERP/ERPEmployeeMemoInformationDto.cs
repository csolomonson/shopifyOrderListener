using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeMemoInformationDto
{
	public string lmkCreatedBy { get; set; }

	public DateTime? lmkCreatedDate { get; set; }

	public string lmkEmployeeID { get; set; }

	public Guid lmkUniqueID { get; set; }

	public string lmkLongDescriptionRtf { get; set; }

	public string lmkLongDescriptionText { get; set; }

	public DateTime? lmkMemoDate { get; set; }

	public byte[] lmkRowVersion { get; set; }

	public short lmkEmployeeMemoID { get; set; }

	public string lmkShortDescription { get; set; }

	public bool lmkShowInEmployees { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
