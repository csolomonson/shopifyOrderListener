using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLandedCostChargeDetailInformationDto
{
	public string rmiCreatedBy { get; set; }

	public DateTime? rmiCreatedDate { get; set; }

	public Guid rmiUniqueID { get; set; }

	public decimal rmiEstTotalCost { get; set; }

	public decimal rmiEstTotalCostForeign { get; set; }

	public short rmiLandedCostChargeID { get; set; }

	public string rmiLandedCostID { get; set; }

	public string rmiPurchaseOrderID { get; set; }

	public short rmiPurchaseOrderLineID { get; set; }

	public byte[] rmiRowVersion { get; set; }

	public int rmiLandedCostChargeDetailID { get; set; }

	public decimal rmiTotalCost { get; set; }

	public decimal rmiTotalCostForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
