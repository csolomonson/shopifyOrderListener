using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetLowValuePoolInformationDto
{
	public DateTime? favClosedDate { get; set; }

	public string favCreatedBy { get; set; }

	public DateTime? favCreatedDate { get; set; }

	public decimal favEndingBalance { get; set; }

	public Guid favUniqueID { get; set; }

	public decimal favHighRate { get; set; }

	public decimal favHighRateDepreciation { get; set; }

	public decimal favImprovement { get; set; }

	public bool favClosed { get; set; }

	public decimal favLowCostAddition { get; set; }

	public decimal favLowRate { get; set; }

	public decimal favLowRateDepreciation { get; set; }

	public decimal favLowValueAddition { get; set; }

	public decimal favOpeningBalance { get; set; }

	public short favPoolYearID { get; set; }

	public byte[] favRowVersion { get; set; }

	public decimal favTermination { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
