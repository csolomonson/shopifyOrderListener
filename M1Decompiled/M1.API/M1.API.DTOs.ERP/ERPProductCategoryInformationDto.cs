using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductCategoryInformationDto
{
	public string incProductCategoryID { get; set; }

	public string incCreatedBy { get; set; }

	public DateTime? incCreatedDate { get; set; }

	public string incDescription { get; set; }

	public Guid incUniqueID { get; set; }

	public string incImageFilePath { get; set; }

	public DateTime? incInactiveDate { get; set; }

	public bool incInactive { get; set; }

	public byte[] INCRowVersion { get; set; }

	public string incStructureCode { get; set; }

	public string incStructureID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
