using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartForecastLineInformationDto
{
	public decimal inlActualBalance { get; set; }

	public decimal inlActualQuantity { get; set; }

	public string inlCreatedBy { get; set; }

	public DateTime? inlCreatedDate { get; set; }

	public DateTime? inlEndDate { get; set; }

	public Guid inlUniqueID { get; set; }

	public decimal inlForecastBalance { get; set; }

	public decimal inlForecastQuantity { get; set; }

	public bool inlIncludeInMRP { get; set; }

	public short inlPartForecastPeriodID { get; set; }

	public short inlPartForecastYearID { get; set; }

	public string inlPartID { get; set; }

	public string inlPartRevisionID { get; set; }

	public decimal inlRemainingQuantity { get; set; }

	public decimal inlRemainingQuantityBalance { get; set; }

	public byte[] inlRowVersion { get; set; }

	public DateTime? inlStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
