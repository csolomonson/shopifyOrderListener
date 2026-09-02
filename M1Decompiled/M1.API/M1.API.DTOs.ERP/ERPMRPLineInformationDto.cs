using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMRPLineInformationDto
{
	public string mrlCreatedBy { get; set; }

	public DateTime? mrlCreatedDate { get; set; }

	public Guid mrlUniqueID { get; set; }

	public decimal mrlForecastDemand { get; set; }

	public decimal mrlInvQtyInProduction { get; set; }

	public bool mrlCompleted { get; set; }

	public bool mrlDataMissing { get; set; }

	public int mrlLineID { get; set; }

	public decimal mrlMaximumQuantity { get; set; }

	public decimal mrlMfgLotSize { get; set; }

	public decimal mrlMinimumQuantity { get; set; }

	public string mrlPartID { get; set; }

	public string mrlPartRevisionID { get; set; }

	public string mrlPartShortDescription { get; set; }

	public string mrlPlantIDs { get; set; }

	public decimal mrlQuantityAllocated { get; set; }

	public decimal mrlQuantityOnHand { get; set; }

	public decimal mrlQuantityToInspect { get; set; }

	public byte[] mrlRowVersion { get; set; }

	public string mrlSessionID { get; set; }

	public string mrlWarehouseIDs { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
