using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetTypeMethodInformationDto
{
	public string famAssetTypeID { get; set; }

	public string famBookDepreciationMethod { get; set; }

	public decimal famBookMultiplier { get; set; }

	public string famCalculationMethod { get; set; }

	public string famCreatedBy { get; set; }

	public DateTime? famCreatedDate { get; set; }

	public Guid famUniqueID { get; set; }

	public bool famCurrentMethod { get; set; }

	public string famMonthCalculationType { get; set; }

	public byte[] famRowVersion { get; set; }

	public short famAssetTypeMethodID { get; set; }

	public DateTime? famStartDate { get; set; }

	public string famTaxDepreciationMethod { get; set; }

	public decimal famTaxMultiplier { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
