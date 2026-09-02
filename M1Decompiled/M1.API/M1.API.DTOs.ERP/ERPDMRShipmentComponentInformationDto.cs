using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDMRShipmentComponentInformationDto
{
	public decimal dsoAdditionalQuantity { get; set; }

	public string dsoCreatedBy { get; set; }

	public DateTime? dsoCreatedDate { get; set; }

	public string dsoDescription { get; set; }

	public int dsoDmrClaimComponentID { get; set; }

	public string dsoDmrClaimID { get; set; }

	public short dsoDmrClaimLineID { get; set; }

	public string dsoDmrShipmentID { get; set; }

	public short dsoDmrShipmentLineID { get; set; }

	public Guid dsoUniqueID { get; set; }

	public int dsoInspectionComponentID { get; set; }

	public string dsoInspectionID { get; set; }

	public short dsoInspectionLineID { get; set; }

	public decimal dsoInvParentQuantity { get; set; }

	public decimal dsoInvQuantityShipped { get; set; }

	public bool dsoClosed { get; set; }

	public bool dsoPosted { get; set; }

	public bool dsoReversed { get; set; }

	public bool dsoShippedComplete { get; set; }

	public int dsoJobAssemblyID { get; set; }

	public string dsoJobID { get; set; }

	public int dsoJobMaterialComponentID { get; set; }

	public int dsoJobMaterialID { get; set; }

	public decimal dsoJobMatParentQuantity { get; set; }

	public decimal dsoJobMatQuantityShipped { get; set; }

	public string dsoPartBinID { get; set; }

	public string dsoPartID { get; set; }

	public string dsoPartRevisionID { get; set; }

	public string dsoPartWarehouseLocationID { get; set; }

	public decimal dsoQuantityPerParent { get; set; }

	public decimal dsoReturnParentQuantity { get; set; }

	public decimal dsoReturnQuantityShipped { get; set; }

	public int dsoReverseDmrShipmentCompID { get; set; }

	public string dsoReverseDmrShipmentID { get; set; }

	public short dsoReverseDmrShipmentLineID { get; set; }

	public byte[] dsoRowVersion { get; set; }

	public int dsoDmrShipmentComponentID { get; set; }

	public string dsoUnitOfMeasure { get; set; }

	public decimal dsoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
