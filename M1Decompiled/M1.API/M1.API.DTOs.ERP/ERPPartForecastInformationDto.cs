using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartForecastInformationDto
{
	public decimal inpAnnualQuantity { get; set; }

	public string inpCreatedBy { get; set; }

	public DateTime? inpCreatedDate { get; set; }

	public DateTime? inpEndDate { get; set; }

	public Guid inpUniqueID { get; set; }

	public string inpForecastMethod { get; set; }

	public byte inpForecastNumberOfYears { get; set; }

	public string inpIntervalType { get; set; }

	public short inpPartForecastYearID { get; set; }

	public string inpPartID { get; set; }

	public string inpPartRevisionID { get; set; }

	public byte[] inpRowVersion { get; set; }

	public DateTime? inpStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
