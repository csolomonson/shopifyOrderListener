using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartMaterialInformationDto
{
	public string immCreatedBy { get; set; }

	public DateTime? immCreatedDate { get; set; }

	public string immDocuments { get; set; }

	public Guid immUniqueID { get; set; }

	public decimal immEstimatedUnitCost { get; set; }

	public bool immBackflush { get; set; }

	public bool immManualPart { get; set; }

	public bool immUseDefaultWarehouseAndBin { get; set; }

	public short immLeadTime { get; set; }

	public int immMethodAssemblyID { get; set; }

	public string immMethodID { get; set; }

	public int immMethodMaterialID { get; set; }

	public string immMethodRevisionID { get; set; }

	public decimal immMinimumCharge { get; set; }

	public string immPartBinID { get; set; }

	public string immPartID { get; set; }

	public string immPartLongDescriptionRtf { get; set; }

	public string immPartLongDescriptionText { get; set; }

	public string immPartRevisionID { get; set; }

	public string immPartShortDescription { get; set; }

	public string immPartWarehouseLocationID { get; set; }

	public string immPurchaseLocationID { get; set; }

	public decimal immQuantityPerAssembly { get; set; }

	public int immRelatedPartOperationID { get; set; }

	public byte[] immRowVersion { get; set; }

	public decimal immScrapPercent { get; set; }

	public decimal immScrapQuantity { get; set; }

	public string immSupplierOrganizationID { get; set; }

	public string immUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
