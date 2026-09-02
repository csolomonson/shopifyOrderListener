using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchasePlannerSessionInformationDto
{
	public string ppsBuyerEmployeeID { get; set; }

	public DateTime? ppsCompletedDate { get; set; }

	public string ppsCreatedBy { get; set; }

	public DateTime? ppsCreatedDate { get; set; }

	public DateTime? ppsCutoffDate { get; set; }

	public DateTime? ppsCutoffDatePosupply { get; set; }

	public Guid ppsUniqueID { get; set; }

	public bool ppsCalculateForAllParts { get; set; }

	public bool ppsCompleted { get; set; }

	public bool ppsFirmOnly { get; set; }

	public bool ppsGenerated { get; set; }

	public string ppsJobIDs { get; set; }

	public string ppsPartClassIDs { get; set; }

	public string ppsPartIDs { get; set; }

	public string ppsPlantID { get; set; }

	public byte[] ppsRowVersion { get; set; }

	public string ppsSalesOrderIDs { get; set; }

	public string ppsSessionID { get; set; }

	public decimal ppsSessionSubtotalBase { get; set; }

	public bool ppsShowAllDemandForPartsOnJobs { get; set; }

	public string ppsSupplierIDs { get; set; }

	public string ppsWarehouseID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
