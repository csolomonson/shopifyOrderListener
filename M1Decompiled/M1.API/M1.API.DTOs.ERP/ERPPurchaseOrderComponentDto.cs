using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderComponentDto
{
	[JsonProperty("pmoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoAdditionalQuantity { get; set; }

	[JsonProperty("pmoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string pmoCreatedBy { get; set; }

	[JsonProperty("pmoCreatedDate", Order = 3)]
	public DateTime? pmoCreatedDate { get; set; }

	[JsonProperty("pmoDeliveryQuantity", Order = 4)]
	[Required(ErrorMessage = "pmoDeliveryQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoDeliveryQuantity { get; set; }

	[JsonProperty("pmoDescription", Order = 5)]
	[Required(ErrorMessage = "pmoDescription is required.")]
	[MaxLength(50)]
	public string pmoDescription { get; set; }

	[JsonProperty("pmoUniqueID", Order = 6)]
	public Guid pmoUniqueID { get; set; }

	[JsonProperty("pmoExtendedCostBase", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoExtendedCostBase { get; set; }

	[JsonProperty("pmoExtendedCostForeign", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoExtendedCostForeign { get; set; }

	[JsonProperty("pmoClosed", Order = 9)]
	public bool pmoClosed { get; set; }

	[JsonProperty("pmoIntraCompanyPosted", Order = 10)]
	public bool pmoIntraCompanyPosted { get; set; }

	[JsonProperty("pmoReceivedComplete", Order = 11)]
	public bool pmoReceivedComplete { get; set; }

	[JsonProperty("pmoJobAssemblyID", Order = 12)]
	public int pmoJobAssemblyID { get; set; }

	[JsonProperty("pmoJobID", Order = 13)]
	[MaxLength(20)]
	public string pmoJobID { get; set; }

	[JsonProperty("pmoJobMaterialComponentID", Order = 14)]
	public int pmoJobMaterialComponentID { get; set; }

	[JsonProperty("pmoJobMaterialID", Order = 15)]
	public int pmoJobMaterialID { get; set; }

	[JsonProperty("pmoParentQuantity", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoParentQuantity { get; set; }

	[JsonProperty("pmoPartBinID", Order = 17)]
	[Required(ErrorMessage = "pmoPartBinID is required.")]
	[MaxLength(15)]
	public string pmoPartBinID { get; set; }

	[JsonProperty("pmoPartID", Order = 18)]
	[Required(ErrorMessage = "pmoPartID is required.")]
	[MaxLength(30)]
	public string pmoPartID { get; set; }

	[JsonProperty("pmoPartRevisionID", Order = 19)]
	[MaxLength(15)]
	public string pmoPartRevisionID { get; set; }

	[JsonProperty("pmoPartWarehouseLocationID", Order = 20)]
	[Required(ErrorMessage = "pmoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string pmoPartWarehouseLocationID { get; set; }

	[JsonProperty("pmoPurchaseOrderID", Order = 21)]
	[Required(ErrorMessage = "pmoPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmoPurchaseOrderID { get; set; }

	[JsonProperty("pmoPurchaseOrderLineID", Order = 22)]
	[Required(ErrorMessage = "pmoPurchaseOrderLineID is required.")]
	public short pmoPurchaseOrderLineID { get; set; }

	[JsonProperty("pmoPurchaseUnitCost", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoPurchaseUnitCost { get; set; }

	[JsonProperty("pmoPurchaseUnitCostForeign", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoPurchaseUnitCostForeign { get; set; }

	[JsonProperty("pmoQuantityPerParent", Order = 25)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoQuantityPerParent { get; set; }

	[JsonProperty("pmoQuantityReceived", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoQuantityReceived { get; set; }

	[JsonProperty("pmoRowVersion", Order = 27)]
	public byte[] pmoRowVersion { get; set; }

	[JsonProperty("pmoPurchaseOrderComponentID", Order = 28)]
	[Required(ErrorMessage = "pmoPurchaseOrderComponentID is required.")]
	public short pmoPurchaseOrderComponentID { get; set; }

	[JsonProperty("pmoUnitOfMeasure", Order = 29)]
	[MaxLength(2)]
	public string pmoUnitOfMeasure { get; set; }

	[JsonProperty("pmoWeight", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmoWeight { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
