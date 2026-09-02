using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchasePlannerOrderDetailInformationDto
{
	public decimal ppoConversionFactor { get; set; }

	public string ppoCreatedBy { get; set; }

	public DateTime? ppoCreatedDate { get; set; }

	public string ppoCurrencyRateID { get; set; }

	public int ppoDataMissing { get; set; }

	public DateTime? ppoDueDate { get; set; }

	public Guid ppoUniqueID { get; set; }

	public decimal ppoExtendedCostBase { get; set; }

	public decimal ppoInventoryQuantity { get; set; }

	public string ppoInventoryUnitOfMeasure { get; set; }

	public bool ppoCompleted { get; set; }

	public bool ppoSupplierRequirement { get; set; }

	public int ppoJobAssemblyID { get; set; }

	public string ppoJobID { get; set; }

	public int ppoJobMaterialID { get; set; }

	public short ppoLeadTime { get; set; }

	public int ppoLineID { get; set; }

	public int ppoOrderDetailID { get; set; }

	public string ppoPartBinID { get; set; }

	public string ppoPartID { get; set; }

	public string ppoPartRevisionID { get; set; }

	public string ppoPartWarehouseLocationID { get; set; }

	public string ppoProjectAreaID { get; set; }

	public string ppoProjectID { get; set; }

	public string ppoPurchaseLocationID { get; set; }

	public decimal ppoPurchaseQuantity { get; set; }

	public byte ppoPurchaseType { get; set; }

	public string ppoPurchaseUnitOfMeasure { get; set; }

	public byte[] ppoRowVersion { get; set; }

	public short ppoSalesOrderDeliveryID { get; set; }

	public string ppoSalesOrderID { get; set; }

	public short ppoSalesOrderLineID { get; set; }

	public string ppoSessionID { get; set; }

	public string ppoSupplierOrganizationID { get; set; }

	public decimal ppoUnitCostBase { get; set; }

	public decimal ppoUnitCostForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
