using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWorkCenterMachineInformationDto
{
	public string xaqDescription { get; set; }

	public Guid xaqUniqueID { get; set; }

	public byte[] xaqRowVersion { get; set; }

	public short xaqWorkCenterMachineID { get; set; }

	public string xaqWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
