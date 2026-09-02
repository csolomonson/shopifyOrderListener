using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobMaterialComponentInformationDto
{
	public decimal jmtAdditionalQuantity { get; set; }

	public string jmtCreatedBy { get; set; }

	public DateTime? jmtCreatedDate { get; set; }

	public string jmtDescription { get; set; }

	public Guid jmtUniqueID { get; set; }

	public bool jmtClosed { get; set; }

	public bool jmtPullAllFromStock { get; set; }

	public bool jmtReceivedComplete { get; set; }

	public int jmtJobAssemblyID { get; set; }

	public string jmtJobID { get; set; }

	public int jmtJobMaterialID { get; set; }

	public decimal jmtMaterialQuantity { get; set; }

	public decimal jmtParentQuantity { get; set; }

	public string jmtPartBinID { get; set; }

	public string jmtPartID { get; set; }

	public string jmtPartRevisionID { get; set; }

	public string jmtPartWarehouseLocationID { get; set; }

	public decimal jmtQuantityAllocated { get; set; }

	public decimal jmtQuantityPerParent { get; set; }

	public decimal jmtQuantityReceived { get; set; }

	public decimal jmtQuantityToInspect { get; set; }

	public decimal jmtQuantityToReturn { get; set; }

	public byte[] jmtRowVersion { get; set; }

	public decimal jmtScrapQuantityReceived { get; set; }

	public int jmtJobMaterialComponentID { get; set; }

	public string jmtUnitOfMeasure { get; set; }

	public decimal jmtWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
