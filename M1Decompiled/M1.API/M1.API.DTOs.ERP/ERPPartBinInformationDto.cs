using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartBinInformationDto
{
	public decimal imbBinQuantityOnHand { get; set; }

	public string imbPartBinID { get; set; }

	public decimal imbConversionFactor { get; set; }

	public string imbCreatedBy { get; set; }

	public DateTime? imbCreatedDate { get; set; }

	public string imbDescription { get; set; }

	public Guid imbUniqueID { get; set; }

	public DateTime? imbInactiveBinDate { get; set; }

	public bool imbInactiveBin { get; set; }

	public bool imbDefaultBin { get; set; }

	public string imbPartID { get; set; }

	public string imbPartRevisionID { get; set; }

	public decimal imbQuantityAllocated { get; set; }

	public decimal imbQuantityOnHand { get; set; }

	public decimal imbQuantityOnOrderPurchases { get; set; }

	public decimal imbQuantityOnOrderSales { get; set; }

	public decimal imbQuantityToInspect { get; set; }

	public decimal imbQuantityToReturn { get; set; }

	public decimal imbQuantityToReturnJob { get; set; }

	public byte[] imbRowVersion { get; set; }

	public string imbWarehouseID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
