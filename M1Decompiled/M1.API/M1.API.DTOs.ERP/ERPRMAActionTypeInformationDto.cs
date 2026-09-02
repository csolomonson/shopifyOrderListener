using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAActionTypeInformationDto
{
	public string ratRmaActionTypeID { get; set; }

	public string ratDescription { get; set; }

	public Guid ratUniqueID { get; set; }

	public byte[] ratRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
