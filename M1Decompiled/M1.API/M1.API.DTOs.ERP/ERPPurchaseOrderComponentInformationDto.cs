using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderComponentInformationDto
{
	public decimal pmoAdditionalQuantity { get; set; }

	public string pmoCreatedBy { get; set; }

	public DateTime? pmoCreatedDate { get; set; }

	public decimal pmoDeliveryQuantity { get; set; }

	public string pmoDescription { get; set; }

	public Guid pmoUniqueID { get; set; }

	public decimal pmoExtendedCostBase { get; set; }

	public decimal pmoExtendedCostForeign { get; set; }

	public bool pmoClosed { get; set; }

	public bool pmoIntraCompanyPosted { get; set; }

	public bool pmoReceivedComplete { get; set; }

	public int pmoJobAssemblyID { get; set; }

	public string pmoJobID { get; set; }

	public int pmoJobMaterialComponentID { get; set; }

	public int pmoJobMaterialID { get; set; }

	public decimal pmoParentQuantity { get; set; }

	public string pmoPartBinID { get; set; }

	public string pmoPartID { get; set; }

	public string pmoPartRevisionID { get; set; }

	public string pmoPartWarehouseLocationID { get; set; }

	public string pmoPurchaseOrderID { get; set; }

	public short pmoPurchaseOrderLineID { get; set; }

	public decimal pmoPurchaseUnitCost { get; set; }

	public decimal pmoPurchaseUnitCostForeign { get; set; }

	public decimal pmoQuantityPerParent { get; set; }

	public decimal pmoQuantityReceived { get; set; }

	public byte[] pmoRowVersion { get; set; }

	public short pmoPurchaseOrderComponentID { get; set; }

	public string pmoUnitOfMeasure { get; set; }

	public decimal pmoWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
