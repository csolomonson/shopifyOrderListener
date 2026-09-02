using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderPickListSessionInformationDto
{
	public string omsCreatedBy { get; set; }

	public DateTime? omsCreatedDate { get; set; }

	public byte omsDevice { get; set; }

	public Guid omsUniqueID { get; set; }

	public bool omsPullFromStockOnly { get; set; }

	public int omsPickListSessionID { get; set; }

	public string omsPlantDepartmentID { get; set; }

	public string omsPlantID { get; set; }

	public DateTime? omsPostedDate { get; set; }

	public byte[] omsRowVersion { get; set; }

	public DateTime? omsSessionDate { get; set; }

	public byte omsStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
