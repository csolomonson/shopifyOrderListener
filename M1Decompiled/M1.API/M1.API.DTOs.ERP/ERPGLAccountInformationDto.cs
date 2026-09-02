using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLAccountInformationDto
{
	public string glaGlAccountID { get; set; }

	public string glaCreatedBy { get; set; }

	public DateTime? glaCreatedDate { get; set; }

	public Guid glaUniqueID { get; set; }

	public string glaExternalGlCode { get; set; }

	public string glaGlChartID { get; set; }

	public string glaGlDepartmentID { get; set; }

	public string glaGlDivisionID { get; set; }

	public DateTime? glaInactiveDate { get; set; }

	public bool glaInactive { get; set; }

	public byte[] glaRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
