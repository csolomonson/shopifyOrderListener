using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductCategoryLineInformationDto
{
	public string insCreatedBy { get; set; }

	public DateTime? insCreatedDate { get; set; }

	public string insDescription { get; set; }

	public Guid insUniqueID { get; set; }

	public string insImageFilePath { get; set; }

	public DateTime? insInactiveDate { get; set; }

	public bool insInactive { get; set; }

	public byte insLevel { get; set; }

	public short insParentLineID { get; set; }

	public string insProductCategoryID { get; set; }

	public byte[] INSRowVersion { get; set; }

	public short insProductCategoryLineID { get; set; }

	public string insStructureCode { get; set; }

	public string insStructureID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
