using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchasePlannerRequirementInformationDto
{
	public string pprCreatedBy { get; set; }

	public DateTime? pprCreatedDate { get; set; }

	public DateTime? pprDueDate { get; set; }

	public Guid pprUniqueID { get; set; }

	public int pprJobAssemblyID { get; set; }

	public string pprJobID { get; set; }

	public int pprJobMaterialID { get; set; }

	public int pprLineID { get; set; }

	public decimal pprPlannedReceiptQuantity { get; set; }

	public decimal pprPlannedRequirementQuantity { get; set; }

	public decimal pprProjectedBalance { get; set; }

	public decimal pprPullFromStockQuantity { get; set; }

	public DateTime? pprPurchaseOrderDate { get; set; }

	public string pprPurchaseOrderID { get; set; }

	public decimal pprPurchaseToJobQuantity { get; set; }

	public byte pprPurchaseType { get; set; }

	public int pprRequirementID { get; set; }

	public byte[] pprRowVersion { get; set; }

	public short pprSalesOrderDeliveryID { get; set; }

	public string pprSalesOrderID { get; set; }

	public short pprSalesOrderLineID { get; set; }

	public string pprSessionID { get; set; }

	public string pprSource { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
