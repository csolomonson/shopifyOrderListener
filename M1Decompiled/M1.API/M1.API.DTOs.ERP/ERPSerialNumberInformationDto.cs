using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSerialNumberInformationDto
{
	public string imsAddedByUserID { get; set; }

	public DateTime? imsAddedDate { get; set; }

	public string imsSerialNumberID { get; set; }

	public string imsCreatedBy { get; set; }

	public DateTime? imsCreatedDate { get; set; }

	public Guid imsUniqueID { get; set; }

	public DateTime? imsExpirationDate { get; set; }

	public DateTime? imsInactiveDate { get; set; }

	public bool imsInactive { get; set; }

	public string imsPartID { get; set; }

	public string imsPartRevisionID { get; set; }

	public byte[] imsRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
