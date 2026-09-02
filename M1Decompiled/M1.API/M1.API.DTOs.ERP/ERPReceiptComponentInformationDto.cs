using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPReceiptComponentInformationDto
{
	public decimal rmoAdditionalQuantity { get; set; }

	public decimal rmoConversionFactor { get; set; }

	public string rmoCreatedBy { get; set; }

	public DateTime? rmoCreatedDate { get; set; }

	public string rmoDescription { get; set; }

	public Guid rmoUniqueID { get; set; }

	public decimal rmoExtendedCostBase { get; set; }

	public decimal rmoExtendedCostForeign { get; set; }

	public decimal rmoInspParentQuantity { get; set; }

	public decimal rmoInventoryUnitCost { get; set; }

	public decimal rmoInventoryUnitCostForeign { get; set; }

	public decimal rmoInvParentQuantity { get; set; }

	public decimal rmoInvQuantityReceived { get; set; }

	public bool rmoClosed { get; set; }

	public bool rmoInspectionComplete { get; set; }

	public bool rmoJobReceivedComplete { get; set; }

	public bool rmoPostedToGl { get; set; }

	public bool rmoReceivedComplete { get; set; }

	public bool rmoReversed { get; set; }

	public int rmoJobAssemblyID { get; set; }

	public string rmoJobID { get; set; }

	public int rmoJobMaterialComponentID { get; set; }

	public int rmoJobMaterialID { get; set; }

	public decimal rmoJobParentQuantity { get; set; }

	public decimal rmoJobQuantityReceived { get; set; }

	public string rmoPartBinID { get; set; }

	public string rmoPartID { get; set; }

	public string rmoPartRevisionID { get; set; }

	public string rmoPartWarehouseLocationID { get; set; }

	public short rmoPurchaseOrderComponentID { get; set; }

	public string rmoPurchaseOrderID { get; set; }

	public short rmoPurchaseOrderLineID { get; set; }

	public decimal rmoPurchaseUnitCost { get; set; }

	public decimal rmoPurchaseUnitCostForeign { get; set; }

	public decimal rmoQuantityPerParent { get; set; }

	public decimal rmoQuantityToInspect { get; set; }

	public string rmoReceiptID { get; set; }

	public short rmoReceiptLineID { get; set; }

	public short rmoReverseReceiptComponentID { get; set; }

	public string rmoReverseReceiptID { get; set; }

	public short rmoReverseReceiptLineID { get; set; }

	public byte[] rmoRowVersion { get; set; }

	public short rmoReceiptComponentID { get; set; }

	public string rmoUnitOfMeasure { get; set; }

	public decimal rmoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
