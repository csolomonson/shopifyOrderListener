using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPInspectionInformationDto
{
	public string qapInspectionID { get; set; }

	public string qapCreatedBy { get; set; }

	public DateTime? qapCreatedDate { get; set; }

	public Guid qapUniqueID { get; set; }

	public bool qapPosted { get; set; }

	public bool qapReversalEntry { get; set; }

	public string qapOpenedByEmployeeID { get; set; }

	public DateTime? qapOpenedDate { get; set; }

	public string qapPlantDepartmentID { get; set; }

	public string qapPlantID { get; set; }

	public DateTime? qapPostedDate { get; set; }

	public string qapProjectID { get; set; }

	public byte[] qapRowVersion { get; set; }

	public string qapSourceTableName { get; set; }

	public Guid qapSourceTableUniqueID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
