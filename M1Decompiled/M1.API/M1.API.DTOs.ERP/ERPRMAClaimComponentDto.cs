using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAClaimComponentDto
{
	[JsonProperty("raoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal raoAdditionalQuantity { get; set; }

	[JsonProperty("raoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string raoCreatedBy { get; set; }

	[JsonProperty("raoCreatedDate", Order = 3)]
	public DateTime? raoCreatedDate { get; set; }

	[JsonProperty("raoDescription", Order = 4)]
	[Required(ErrorMessage = "raoDescription is required.")]
	[MaxLength(50)]
	public string raoDescription { get; set; }

	[JsonProperty("raoUniqueID", Order = 5)]
	public Guid raoUniqueID { get; set; }

	[JsonProperty("raoReceivedComplete", Order = 6)]
	public bool raoReceivedComplete { get; set; }

	[JsonProperty("raoParentQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal raoParentQuantity { get; set; }

	[JsonProperty("raoPartBinID", Order = 8)]
	[Required(ErrorMessage = "raoPartBinID is required.")]
	[MaxLength(15)]
	public string raoPartBinID { get; set; }

	[JsonProperty("raoPartID", Order = 9)]
	[Required(ErrorMessage = "raoPartID is required.")]
	[MaxLength(30)]
	public string raoPartID { get; set; }

	[JsonProperty("raoPartRevisionID", Order = 10)]
	[MaxLength(15)]
	public string raoPartRevisionID { get; set; }

	[JsonProperty("raoPartWarehouseLocationID", Order = 11)]
	[Required(ErrorMessage = "raoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string raoPartWarehouseLocationID { get; set; }

	[JsonProperty("raoQuantity", Order = 12)]
	[Required(ErrorMessage = "raoQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal raoQuantity { get; set; }

	[JsonProperty("raoQuantityPerParent", Order = 13)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal raoQuantityPerParent { get; set; }

	[JsonProperty("raoQuantityReceived", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal raoQuantityReceived { get; set; }

	[JsonProperty("raoRmaClaimID", Order = 15)]
	[Required(ErrorMessage = "raoRmaClaimID is required.")]
	[MaxLength(10)]
	public string raoRmaClaimID { get; set; }

	[JsonProperty("raoRmaClaimLineID", Order = 16)]
	[Required(ErrorMessage = "raoRmaClaimLineID is required.")]
	public short raoRmaClaimLineID { get; set; }

	[JsonProperty("raoRowVersion", Order = 17)]
	public byte[] raoRowVersion { get; set; }

	[JsonProperty("raoRmaClaimComponentID", Order = 18)]
	[Required(ErrorMessage = "raoRmaClaimComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int raoRmaClaimComponentID { get; set; }

	[JsonProperty("raoShipmentComponentID", Order = 19)]
	public short raoShipmentComponentID { get; set; }

	[JsonProperty("raoShipmentID", Order = 20)]
	[MaxLength(10)]
	public string raoShipmentID { get; set; }

	[JsonProperty("raoShipmentLineID", Order = 21)]
	public short raoShipmentLineID { get; set; }

	[JsonProperty("raoUnitOfMeasure", Order = 22)]
	[MaxLength(2)]
	public string raoUnitOfMeasure { get; set; }

	[JsonProperty("raoWeight", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal raoWeight { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
