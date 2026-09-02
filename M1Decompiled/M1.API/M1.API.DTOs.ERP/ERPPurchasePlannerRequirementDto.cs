using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchasePlannerRequirementDto
{
	[JsonProperty("pprCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string pprCreatedBy { get; set; }

	[JsonProperty("pprCreatedDate", Order = 2)]
	public DateTime? pprCreatedDate { get; set; }

	[JsonProperty("pprDueDate", Order = 3)]
	public DateTime? pprDueDate { get; set; }

	[JsonProperty("pprUniqueID", Order = 4)]
	public Guid pprUniqueID { get; set; }

	[JsonProperty("pprJobAssemblyID", Order = 5)]
	public int pprJobAssemblyID { get; set; }

	[JsonProperty("pprJobID", Order = 6)]
	[MaxLength(20)]
	public string pprJobID { get; set; }

	[JsonProperty("pprJobMaterialID", Order = 7)]
	public int pprJobMaterialID { get; set; }

	[JsonProperty("pprLineID", Order = 8)]
	[Required(ErrorMessage = "pprLineID is required.")]
	public int pprLineID { get; set; }

	[JsonProperty("pprPlannedReceiptQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pprPlannedReceiptQuantity { get; set; }

	[JsonProperty("pprPlannedRequirementQuantity", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pprPlannedRequirementQuantity { get; set; }

	[JsonProperty("pprProjectedBalance", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pprProjectedBalance { get; set; }

	[JsonProperty("pprPullFromStockQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pprPullFromStockQuantity { get; set; }

	[JsonProperty("pprPurchaseOrderDate", Order = 13)]
	public DateTime? pprPurchaseOrderDate { get; set; }

	[JsonProperty("pprPurchaseOrderID", Order = 14)]
	[MaxLength(10)]
	public string pprPurchaseOrderID { get; set; }

	[JsonProperty("pprPurchaseToJobQuantity", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pprPurchaseToJobQuantity { get; set; }

	[JsonProperty("pprPurchaseType", Order = 16)]
	public byte pprPurchaseType { get; set; }

	[JsonProperty("pprRequirementID", Order = 17)]
	[Required(ErrorMessage = "pprRequirementID is required.")]
	public int pprRequirementID { get; set; }

	[JsonProperty("pprRowVersion", Order = 18)]
	public byte[] pprRowVersion { get; set; }

	[JsonProperty("pprSalesOrderDeliveryID", Order = 19)]
	public short pprSalesOrderDeliveryID { get; set; }

	[JsonProperty("pprSalesOrderID", Order = 20)]
	[MaxLength(10)]
	public string pprSalesOrderID { get; set; }

	[JsonProperty("pprSalesOrderLineID", Order = 21)]
	public short pprSalesOrderLineID { get; set; }

	[JsonProperty("pprSessionID", Order = 22)]
	[Required(ErrorMessage = "pprSessionID is required.")]
	[MaxLength(10)]
	public string pprSessionID { get; set; }

	[JsonProperty("pprSource", Order = 23)]
	[MaxLength(50)]
	public string pprSource { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
