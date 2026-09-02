using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMRPSessionInformationDto
{
	public DateTime? mrpCompletedDate { get; set; }

	public string mrpCreatedBy { get; set; }

	public DateTime? mrpCreatedDate { get; set; }

	public string mrpCustomerIDs { get; set; }

	public DateTime? mrpCutoffDate { get; set; }

	public Guid mrpUniqueID { get; set; }

	public bool mrpCompleted { get; set; }

	public bool mrpConsolidatePartForecastJobs { get; set; }

	public bool mrpGenerated { get; set; }

	public bool mrpIncludePartForecasts { get; set; }

	public string mrpPartClassIDs { get; set; }

	public string mrpPartGroupIDs { get; set; }

	public string mrpPartIDs { get; set; }

	public string mrpPlantIDs { get; set; }

	public byte[] mrpRowVersion { get; set; }

	public string mrpSessionID { get; set; }

	public string mrpWarehouseIDs { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
