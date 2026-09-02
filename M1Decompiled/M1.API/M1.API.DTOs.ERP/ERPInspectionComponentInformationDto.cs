using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPInspectionComponentInformationDto
{
	public decimal qamAdditionalQuantity { get; set; }

	public decimal qamComponentQtyToInspect { get; set; }

	public string qamCreatedBy { get; set; }

	public DateTime? qamCreatedDate { get; set; }

	public string qamDescription { get; set; }

	public Guid qamUniqueID { get; set; }

	public string qamInspectionID { get; set; }

	public short qamInspectionLineID { get; set; }

	public byte qamInspectionType { get; set; }

	public decimal qamInvParentQtyAccepted { get; set; }

	public decimal qamInvParentQtyToReturn { get; set; }

	public decimal qamInvParentQtyToScrap { get; set; }

	public decimal qamInvQuantityAccepted { get; set; }

	public decimal qamInvQuantityToReturn { get; set; }

	public decimal qamInvQuantityToScrap { get; set; }

	public bool qamInspectionComplete { get; set; }

	public bool qamManualInspectionFinalized { get; set; }

	public bool qamPosted { get; set; }

	public int qamJobAssemblyID { get; set; }

	public string qamJobID { get; set; }

	public int qamJobMaterialComponentID { get; set; }

	public int qamJobMaterialID { get; set; }

	public decimal qamJobMatParentQtyAccepted { get; set; }

	public decimal qamJobMatParentQtyToReturn { get; set; }

	public decimal qamJobMatParentQtyToScrap { get; set; }

	public decimal qamJobMatQuantityAccepted { get; set; }

	public decimal qamJobMatQuantityToReturn { get; set; }

	public decimal qamJobMatQuantityToScrap { get; set; }

	public decimal qamParentQtyToInspect { get; set; }

	public string qamPartBinID { get; set; }

	public string qamPartID { get; set; }

	public string qamPartRevisionID { get; set; }

	public string qamPartWarehouseLocationID { get; set; }

	public decimal qamQuantityPerParent { get; set; }

	public int qamInspectionComponentID { get; set; }

	public string qamSourceTableName { get; set; }

	public Guid qamSourceTableUniqueID { get; set; }

	public string qamUnitOfMeasure { get; set; }

	public decimal qamWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
