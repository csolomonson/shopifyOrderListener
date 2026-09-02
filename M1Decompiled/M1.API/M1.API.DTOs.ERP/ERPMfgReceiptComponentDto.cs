using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMfgReceiptComponentDto
{
	[JsonProperty("rmnAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnAdditionalQuantity { get; set; }

	[JsonProperty("rmnCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string rmnCreatedBy { get; set; }

	[JsonProperty("rmnCreatedDate", Order = 3)]
	public DateTime? rmnCreatedDate { get; set; }

	[JsonProperty("rmnDescription", Order = 4)]
	[MaxLength(50)]
	public string rmnDescription { get; set; }

	[JsonProperty("rmnUniqueID", Order = 5)]
	public Guid rmnUniqueID { get; set; }

	[JsonProperty("rmnExtendedCost", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnExtendedCost { get; set; }

	[JsonProperty("rmnInvParentQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnInvParentQuantity { get; set; }

	[JsonProperty("rmnInvReceiptQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnInvReceiptQuantity { get; set; }

	[JsonProperty("rmnPosted", Order = 9)]
	public bool rmnPosted { get; set; }

	[JsonProperty("rmnReceivedComplete", Order = 10)]
	public bool rmnReceivedComplete { get; set; }

	[JsonProperty("rmnReversed", Order = 11)]
	public bool rmnReversed { get; set; }

	[JsonProperty("rmnJobAssemblyID", Order = 12)]
	public int rmnJobAssemblyID { get; set; }

	[JsonProperty("rmnJobID", Order = 13)]
	[MaxLength(20)]
	public string rmnJobID { get; set; }

	[JsonProperty("rmnJobMaterialComponentID", Order = 14)]
	public int rmnJobMaterialComponentID { get; set; }

	[JsonProperty("rmnJobMaterialID", Order = 15)]
	public int rmnJobMaterialID { get; set; }

	[JsonProperty("rmnJobMatParentQuantity", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnJobMatParentQuantity { get; set; }

	[JsonProperty("rmnJobMatReceiptQuantity", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnJobMatReceiptQuantity { get; set; }

	[JsonProperty("rmnMfgReceiptID", Order = 18)]
	[MaxLength(10)]
	public string rmnMfgReceiptID { get; set; }

	[JsonProperty("rmnPartBinID", Order = 19)]
	[Required(ErrorMessage = "rmnPartBinID is required.")]
	[MaxLength(15)]
	public string rmnPartBinID { get; set; }

	[JsonProperty("rmnPartID", Order = 20)]
	[Required(ErrorMessage = "rmnPartID is required.")]
	[MaxLength(30)]
	public string rmnPartID { get; set; }

	[JsonProperty("rmnPartRevisionID", Order = 21)]
	[MaxLength(15)]
	public string rmnPartRevisionID { get; set; }

	[JsonProperty("rmnPartWarehouseLocationID", Order = 22)]
	[Required(ErrorMessage = "rmnPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string rmnPartWarehouseLocationID { get; set; }

	[JsonProperty("rmnQuantityPerParent", Order = 23)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnQuantityPerParent { get; set; }

	[JsonProperty("rmnReverseMfgReceiptCompID", Order = 24)]
	public int rmnReverseMfgReceiptCompID { get; set; }

	[JsonProperty("rmnReverseMfgReceiptID", Order = 25)]
	[MaxLength(10)]
	public string rmnReverseMfgReceiptID { get; set; }

	[JsonProperty("rmnRowVersion", Order = 26)]
	public byte[] rmnRowVersion { get; set; }

	[JsonProperty("rmnMfgReceiptComponentID", Order = 27)]
	[Required(ErrorMessage = "rmnMfgReceiptComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int rmnMfgReceiptComponentID { get; set; }

	[JsonProperty("rmnUnitCost", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnUnitCost { get; set; }

	[JsonProperty("rmnUnitOfMeasure", Order = 29)]
	[MaxLength(2)]
	public string rmnUnitOfMeasure { get; set; }

	[JsonProperty("rmnWeight", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmnWeight { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
