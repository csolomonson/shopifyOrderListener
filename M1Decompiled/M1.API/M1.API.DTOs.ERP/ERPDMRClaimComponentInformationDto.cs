using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDMRClaimComponentInformationDto
{
	public decimal dmoAdditionalQuantity { get; set; }

	public string dmoCreatedBy { get; set; }

	public DateTime? dmoCreatedDate { get; set; }

	public string dmoDescription { get; set; }

	public string dmoDmrClaimID { get; set; }

	public short dmoDmrClaimLineID { get; set; }

	public Guid dmoUniqueID { get; set; }

	public int dmoInspectionComponentID { get; set; }

	public string dmoInspectionID { get; set; }

	public short dmoInspectionLineID { get; set; }

	public bool dmoShippedComplete { get; set; }

	public int dmoJobAssemblyID { get; set; }

	public string dmoJobID { get; set; }

	public int dmoJobMaterialComponentID { get; set; }

	public int dmoJobMaterialID { get; set; }

	public decimal dmoParentQuantity { get; set; }

	public string dmoPartBinID { get; set; }

	public string dmoPartID { get; set; }

	public string dmoPartRevisionID { get; set; }

	public string dmoPartWarehouseLocationID { get; set; }

	public decimal dmoQuantity { get; set; }

	public decimal dmoQuantityPerParent { get; set; }

	public decimal dmoQuantityShipped { get; set; }

	public byte[] dmoRowVersion { get; set; }

	public int dmoDmrClaimComponentID { get; set; }

	public string dmoUnitOfMeasure { get; set; }

	public decimal dmoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
