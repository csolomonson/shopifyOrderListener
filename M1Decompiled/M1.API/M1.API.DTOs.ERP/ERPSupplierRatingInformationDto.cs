using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSupplierRatingInformationDto
{
	public string cmsSupplierRatingID { get; set; }

	public string cmsCreatedBy { get; set; }

	public DateTime? cmsCreatedDate { get; set; }

	public string cmsDescription { get; set; }

	public Guid cmsUniqueID { get; set; }

	public byte[] cmsRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
