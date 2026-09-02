using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartBinDetailInformationDto
{
	public string imgCreatedBy { get; set; }

	public DateTime? imgCreatedDate { get; set; }

	public Guid imgUniqueID { get; set; }

	public decimal imgOriginalQuantity { get; set; }

	public string imgPartBinID { get; set; }

	public string imgPartID { get; set; }

	public string imgPartRevisionID { get; set; }

	public byte imgQuantityType { get; set; }

	public decimal imgRemainingQuantity { get; set; }

	public byte[] imgRowVersion { get; set; }

	public int imgPartBinDetailID { get; set; }

	public string imgSourceTableName { get; set; }

	public Guid imgSourceTableUniqueID { get; set; }

	public DateTime? imgTransactionDate { get; set; }

	public decimal imgUnitDutyCost { get; set; }

	public decimal imgUnitFreightCost { get; set; }

	public decimal imgUnitLaborCost { get; set; }

	public decimal imgUnitMaterialCost { get; set; }

	public decimal imgUnitMiscCost { get; set; }

	public decimal imgUnitOverheadCost { get; set; }

	public decimal imgUnitSubcontractCost { get; set; }

	public string imgWarehouseID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
