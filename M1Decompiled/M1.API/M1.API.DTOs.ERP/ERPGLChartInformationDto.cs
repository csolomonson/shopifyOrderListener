using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLChartInformationDto
{
	public byte glcAccountType { get; set; }

	public byte glcCashFlowCategory { get; set; }

	public string glcGlChartID { get; set; }

	public byte glcCogsAccountType { get; set; }

	public string glcCreatedBy { get; set; }

	public DateTime? glcCreatedDate { get; set; }

	public string glcDescription { get; set; }

	public Guid glcUniqueID { get; set; }

	public string glcGlCategoryID { get; set; }

	public bool glcCashEquivalents { get; set; }

	public bool glcParentAccount { get; set; }

	public byte glcNormalBalance { get; set; }

	public string glcParentDescription { get; set; }

	public string glcParentGlChartID { get; set; }

	public byte[] glcRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
