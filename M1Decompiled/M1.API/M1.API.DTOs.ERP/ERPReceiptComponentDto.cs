using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPReceiptComponentDto
{
	[JsonProperty("rmoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoAdditionalQuantity { get; set; }

	[JsonProperty("rmoConversionFactor", Order = 2)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoConversionFactor { get; set; }

	[JsonProperty("rmoCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string rmoCreatedBy { get; set; }

	[JsonProperty("rmoCreatedDate", Order = 4)]
	public DateTime? rmoCreatedDate { get; set; }

	[JsonProperty("rmoDescription", Order = 5)]
	[MaxLength(50)]
	public string rmoDescription { get; set; }

	[JsonProperty("rmoUniqueID", Order = 6)]
	public Guid rmoUniqueID { get; set; }

	[JsonProperty("rmoExtendedCostBase", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoExtendedCostBase { get; set; }

	[JsonProperty("rmoExtendedCostForeign", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoExtendedCostForeign { get; set; }

	[JsonProperty("rmoInspParentQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoInspParentQuantity { get; set; }

	[JsonProperty("rmoInventoryUnitCost", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoInventoryUnitCost { get; set; }

	[JsonProperty("rmoInventoryUnitCostForeign", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoInventoryUnitCostForeign { get; set; }

	[JsonProperty("rmoInvParentQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoInvParentQuantity { get; set; }

	[JsonProperty("rmoInvQuantityReceived", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoInvQuantityReceived { get; set; }

	[JsonProperty("rmoClosed", Order = 14)]
	public bool rmoClosed { get; set; }

	[JsonProperty("rmoInspectionComplete", Order = 15)]
	public bool rmoInspectionComplete { get; set; }

	[JsonProperty("rmoJobReceivedComplete", Order = 16)]
	public bool rmoJobReceivedComplete { get; set; }

	[JsonProperty("rmoPostedToGl", Order = 17)]
	public bool rmoPostedToGl { get; set; }

	[JsonProperty("rmoReceivedComplete", Order = 18)]
	public bool rmoReceivedComplete { get; set; }

	[JsonProperty("rmoReversed", Order = 19)]
	public bool rmoReversed { get; set; }

	[JsonProperty("rmoJobAssemblyID", Order = 20)]
	public int rmoJobAssemblyID { get; set; }

	[JsonProperty("rmoJobID", Order = 21)]
	[MaxLength(20)]
	public string rmoJobID { get; set; }

	[JsonProperty("rmoJobMaterialComponentID", Order = 22)]
	public int rmoJobMaterialComponentID { get; set; }

	[JsonProperty("rmoJobMaterialID", Order = 23)]
	public int rmoJobMaterialID { get; set; }

	[JsonProperty("rmoJobParentQuantity", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoJobParentQuantity { get; set; }

	[JsonProperty("rmoJobQuantityReceived", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoJobQuantityReceived { get; set; }

	[JsonProperty("rmoPartBinID", Order = 26)]
	[Required(ErrorMessage = "rmoPartBinID is required.")]
	[MaxLength(15)]
	public string rmoPartBinID { get; set; }

	[JsonProperty("rmoPartID", Order = 27)]
	[Required(ErrorMessage = "rmoPartID is required.")]
	[MaxLength(30)]
	public string rmoPartID { get; set; }

	[JsonProperty("rmoPartRevisionID", Order = 28)]
	[MaxLength(15)]
	public string rmoPartRevisionID { get; set; }

	[JsonProperty("rmoPartWarehouseLocationID", Order = 29)]
	[Required(ErrorMessage = "rmoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string rmoPartWarehouseLocationID { get; set; }

	[JsonProperty("rmoPurchaseOrderComponentID", Order = 30)]
	public short rmoPurchaseOrderComponentID { get; set; }

	[JsonProperty("rmoPurchaseOrderID", Order = 31)]
	[MaxLength(10)]
	public string rmoPurchaseOrderID { get; set; }

	[JsonProperty("rmoPurchaseOrderLineID", Order = 32)]
	public short rmoPurchaseOrderLineID { get; set; }

	[JsonProperty("rmoPurchaseUnitCost", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoPurchaseUnitCost { get; set; }

	[JsonProperty("rmoPurchaseUnitCostForeign", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoPurchaseUnitCostForeign { get; set; }

	[JsonProperty("rmoQuantityPerParent", Order = 35)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoQuantityPerParent { get; set; }

	[JsonProperty("rmoQuantityToInspect", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoQuantityToInspect { get; set; }

	[JsonProperty("rmoReceiptID", Order = 37)]
	[Required(ErrorMessage = "rmoReceiptID is required.")]
	[MaxLength(10)]
	public string rmoReceiptID { get; set; }

	[JsonProperty("rmoReceiptLineID", Order = 38)]
	[Required(ErrorMessage = "rmoReceiptLineID is required.")]
	public short rmoReceiptLineID { get; set; }

	[JsonProperty("rmoReverseReceiptComponentID", Order = 39)]
	public short rmoReverseReceiptComponentID { get; set; }

	[JsonProperty("rmoReverseReceiptID", Order = 40)]
	[MaxLength(10)]
	public string rmoReverseReceiptID { get; set; }

	[JsonProperty("rmoReverseReceiptLineID", Order = 41)]
	public short rmoReverseReceiptLineID { get; set; }

	[JsonProperty("rmoRowVersion", Order = 42)]
	public byte[] rmoRowVersion { get; set; }

	[JsonProperty("rmoReceiptComponentID", Order = 43)]
	[Required(ErrorMessage = "rmoReceiptComponentID is required.")]
	public short rmoReceiptComponentID { get; set; }

	[JsonProperty("rmoUnitOfMeasure", Order = 44)]
	[MaxLength(2)]
	public string rmoUnitOfMeasure { get; set; }

	[JsonProperty("rmoWeight", Order = 45)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmoWeight { get; set; }

	[JsonProperty("customFields", Order = 46)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
