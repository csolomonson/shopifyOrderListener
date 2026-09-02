using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAReceiptComponentDto
{
	[JsonProperty("rroAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroAdditionalQuantity { get; set; }

	[JsonProperty("rroCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string rroCreatedBy { get; set; }

	[JsonProperty("rroCreatedDate", Order = 3)]
	public DateTime? rroCreatedDate { get; set; }

	[JsonProperty("rroDescription", Order = 4)]
	[Required(ErrorMessage = "rroDescription is required.")]
	[MaxLength(50)]
	public string rroDescription { get; set; }

	[JsonProperty("rroUniqueID", Order = 5)]
	public Guid rroUniqueID { get; set; }

	[JsonProperty("rroExtendedCost", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroExtendedCost { get; set; }

	[JsonProperty("rroExtendedCostForeign", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroExtendedCostForeign { get; set; }

	[JsonProperty("rroInspParentQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroInspParentQuantity { get; set; }

	[JsonProperty("rroClosed", Order = 9)]
	public bool rroClosed { get; set; }

	[JsonProperty("rroInspectionComplete", Order = 10)]
	public bool rroInspectionComplete { get; set; }

	[JsonProperty("rroPosted", Order = 11)]
	public bool rroPosted { get; set; }

	[JsonProperty("rroReceivedComplete", Order = 12)]
	public bool rroReceivedComplete { get; set; }

	[JsonProperty("rroReversed", Order = 13)]
	public bool rroReversed { get; set; }

	[JsonProperty("rroParentQuantity", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroParentQuantity { get; set; }

	[JsonProperty("rroPartBinID", Order = 15)]
	[Required(ErrorMessage = "rroPartBinID is required.")]
	[MaxLength(15)]
	public string rroPartBinID { get; set; }

	[JsonProperty("rroPartID", Order = 16)]
	[Required(ErrorMessage = "rroPartID is required.")]
	[MaxLength(30)]
	public string rroPartID { get; set; }

	[JsonProperty("rroPartRevisionID", Order = 17)]
	[MaxLength(15)]
	public string rroPartRevisionID { get; set; }

	[JsonProperty("rroPartWarehouseLocationID", Order = 18)]
	[Required(ErrorMessage = "rroPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string rroPartWarehouseLocationID { get; set; }

	[JsonProperty("rroQuantityPerParent", Order = 19)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroQuantityPerParent { get; set; }

	[JsonProperty("rroQuantityReceived", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroQuantityReceived { get; set; }

	[JsonProperty("rroQuantityToInspect", Order = 21)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroQuantityToInspect { get; set; }

	[JsonProperty("rroReverseRmaReceiptCompID", Order = 22)]
	public int rroReverseRmaReceiptCompID { get; set; }

	[JsonProperty("rroReverseRmaReceiptID", Order = 23)]
	[MaxLength(10)]
	public string rroReverseRmaReceiptID { get; set; }

	[JsonProperty("rroReverseRmaReceiptLineID", Order = 24)]
	public short rroReverseRmaReceiptLineID { get; set; }

	[JsonProperty("rroRmaClaimComponentID", Order = 25)]
	public int rroRmaClaimComponentID { get; set; }

	[JsonProperty("rroRmaClaimID", Order = 26)]
	[MaxLength(10)]
	public string rroRmaClaimID { get; set; }

	[JsonProperty("rroRmaClaimLineID", Order = 27)]
	public short rroRmaClaimLineID { get; set; }

	[JsonProperty("rroRmaReceiptID", Order = 28)]
	[Required(ErrorMessage = "rroRmaReceiptID is required.")]
	[MaxLength(10)]
	public string rroRmaReceiptID { get; set; }

	[JsonProperty("rroRmaReceiptLineID", Order = 29)]
	[Required(ErrorMessage = "rroRmaReceiptLineID is required.")]
	public short rroRmaReceiptLineID { get; set; }

	[JsonProperty("rroRowVersion", Order = 30)]
	public byte[] rroRowVersion { get; set; }

	[JsonProperty("rroRmaReceiptComponentID", Order = 31)]
	[Required(ErrorMessage = "rroRmaReceiptComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int rroRmaReceiptComponentID { get; set; }

	[JsonProperty("rroUnitCost", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroUnitCost { get; set; }

	[JsonProperty("rroUnitCostForeign", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroUnitCostForeign { get; set; }

	[JsonProperty("rroUnitOfMeasure", Order = 34)]
	[MaxLength(2)]
	public string rroUnitOfMeasure { get; set; }

	[JsonProperty("rroWeight", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rroWeight { get; set; }

	[JsonProperty("customFields", Order = 36)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
