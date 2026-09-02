using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchasePlannerOrderDetailDto
{
	[JsonProperty("ppoConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppoConversionFactor { get; set; }

	[JsonProperty("ppoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string ppoCreatedBy { get; set; }

	[JsonProperty("ppoCreatedDate", Order = 3)]
	public DateTime? ppoCreatedDate { get; set; }

	[JsonProperty("ppoCurrencyRateID", Order = 4)]
	[MaxLength(5)]
	public string ppoCurrencyRateID { get; set; }

	[JsonProperty("ppoDataMissing", Order = 5)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ppoDataMissing { get; set; }

	[JsonProperty("ppoDueDate", Order = 6)]
	public DateTime? ppoDueDate { get; set; }

	[JsonProperty("ppoUniqueID", Order = 7)]
	public Guid ppoUniqueID { get; set; }

	[JsonProperty("ppoExtendedCostBase", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppoExtendedCostBase { get; set; }

	[JsonProperty("ppoInventoryQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppoInventoryQuantity { get; set; }

	[JsonProperty("ppoInventoryUnitOfMeasure", Order = 10)]
	[MaxLength(2)]
	public string ppoInventoryUnitOfMeasure { get; set; }

	[JsonProperty("ppoCompleted", Order = 11)]
	public bool ppoCompleted { get; set; }

	[JsonProperty("ppoSupplierRequirement", Order = 12)]
	public bool ppoSupplierRequirement { get; set; }

	[JsonProperty("ppoJobAssemblyID", Order = 13)]
	public int ppoJobAssemblyID { get; set; }

	[JsonProperty("ppoJobID", Order = 14)]
	[MaxLength(20)]
	public string ppoJobID { get; set; }

	[JsonProperty("ppoJobMaterialID", Order = 15)]
	public int ppoJobMaterialID { get; set; }

	[JsonProperty("ppoLeadTime", Order = 16)]
	public short ppoLeadTime { get; set; }

	[JsonProperty("ppoLineID", Order = 17)]
	[Required(ErrorMessage = "ppoLineID is required.")]
	public int ppoLineID { get; set; }

	[JsonProperty("ppoOrderDetailID", Order = 18)]
	[Required(ErrorMessage = "ppoOrderDetailID is required.")]
	public int ppoOrderDetailID { get; set; }

	[JsonProperty("ppoPartBinID", Order = 19)]
	[Required(ErrorMessage = "ppoPartBinID is required.")]
	[MaxLength(15)]
	public string ppoPartBinID { get; set; }

	[JsonProperty("ppoPartID", Order = 20)]
	[Required(ErrorMessage = "ppoPartID is required.")]
	[MaxLength(30)]
	public string ppoPartID { get; set; }

	[JsonProperty("ppoPartRevisionID", Order = 21)]
	[MaxLength(15)]
	public string ppoPartRevisionID { get; set; }

	[JsonProperty("ppoPartWarehouseLocationID", Order = 22)]
	[Required(ErrorMessage = "ppoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string ppoPartWarehouseLocationID { get; set; }

	[JsonProperty("ppoProjectAreaID", Order = 23)]
	[MaxLength(15)]
	public string ppoProjectAreaID { get; set; }

	[JsonProperty("ppoProjectID", Order = 24)]
	[MaxLength(10)]
	public string ppoProjectID { get; set; }

	[JsonProperty("ppoPurchaseLocationID", Order = 25)]
	[MaxLength(5)]
	public string ppoPurchaseLocationID { get; set; }

	[JsonProperty("ppoPurchaseQuantity", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppoPurchaseQuantity { get; set; }

	[JsonProperty("ppoPurchaseType", Order = 27)]
	public byte ppoPurchaseType { get; set; }

	[JsonProperty("ppoPurchaseUnitOfMeasure", Order = 28)]
	[MaxLength(2)]
	public string ppoPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("ppoRowVersion", Order = 29)]
	public byte[] ppoRowVersion { get; set; }

	[JsonProperty("ppoSalesOrderDeliveryID", Order = 30)]
	public short ppoSalesOrderDeliveryID { get; set; }

	[JsonProperty("ppoSalesOrderID", Order = 31)]
	[MaxLength(10)]
	public string ppoSalesOrderID { get; set; }

	[JsonProperty("ppoSalesOrderLineID", Order = 32)]
	public short ppoSalesOrderLineID { get; set; }

	[JsonProperty("ppoSessionID", Order = 33)]
	[Required(ErrorMessage = "ppoSessionID is required.")]
	[MaxLength(10)]
	public string ppoSessionID { get; set; }

	[JsonProperty("ppoSupplierOrganizationID", Order = 34)]
	[Required(ErrorMessage = "ppoSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string ppoSupplierOrganizationID { get; set; }

	[JsonProperty("ppoUnitCostBase", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppoUnitCostBase { get; set; }

	[JsonProperty("ppoUnitCostForeign", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppoUnitCostForeign { get; set; }

	[JsonProperty("customFields", Order = 37)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
