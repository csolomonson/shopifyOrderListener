using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderComponentDto
{
	[JsonProperty("omoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoAdditionalQuantity { get; set; }

	[JsonProperty("omoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string omoCreatedBy { get; set; }

	[JsonProperty("omoCreatedDate", Order = 3)]
	public DateTime? omoCreatedDate { get; set; }

	[JsonProperty("omoDeliveryQuantity", Order = 4)]
	[Required(ErrorMessage = "omoDeliveryQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoDeliveryQuantity { get; set; }

	[JsonProperty("omoDescription", Order = 5)]
	[Required(ErrorMessage = "omoDescription is required.")]
	[MaxLength(50)]
	public string omoDescription { get; set; }

	[JsonProperty("omoUniqueID", Order = 6)]
	public Guid omoUniqueID { get; set; }

	[JsonProperty("omoClosed", Order = 7)]
	public bool omoClosed { get; set; }

	[JsonProperty("omoShippedComplete", Order = 8)]
	public bool omoShippedComplete { get; set; }

	[JsonProperty("omoParentQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoParentQuantity { get; set; }

	[JsonProperty("omoPartBinID", Order = 10)]
	[Required(ErrorMessage = "omoPartBinID is required.")]
	[MaxLength(15)]
	public string omoPartBinID { get; set; }

	[JsonProperty("omoPartID", Order = 11)]
	[Required(ErrorMessage = "omoPartID is required.")]
	[MaxLength(30)]
	public string omoPartID { get; set; }

	[JsonProperty("omoPartRevisionID", Order = 12)]
	[MaxLength(15)]
	public string omoPartRevisionID { get; set; }

	[JsonProperty("omoPartWarehouseLocationID", Order = 13)]
	[Required(ErrorMessage = "omoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string omoPartWarehouseLocationID { get; set; }

	[JsonProperty("omoQuantityAllocated", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoQuantityAllocated { get; set; }

	[JsonProperty("omoQuantityPerParent", Order = 15)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoQuantityPerParent { get; set; }

	[JsonProperty("omoQuantityShipped", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoQuantityShipped { get; set; }

	[JsonProperty("omoRowVersion", Order = 17)]
	public byte[] omoRowVersion { get; set; }

	[JsonProperty("omoSalesOrderDeliveryID", Order = 18)]
	[Required(ErrorMessage = "omoSalesOrderDeliveryID is required.")]
	public short omoSalesOrderDeliveryID { get; set; }

	[JsonProperty("omoSalesOrderID", Order = 19)]
	[Required(ErrorMessage = "omoSalesOrderID is required.")]
	[MaxLength(10)]
	public string omoSalesOrderID { get; set; }

	[JsonProperty("omoSalesOrderLineID", Order = 20)]
	[Required(ErrorMessage = "omoSalesOrderLineID is required.")]
	public short omoSalesOrderLineID { get; set; }

	[JsonProperty("omoSalesOrderComponentID", Order = 21)]
	[Required(ErrorMessage = "omoSalesOrderComponentID is required.")]
	public short omoSalesOrderComponentID { get; set; }

	[JsonProperty("omoUnitOfMeasure", Order = 22)]
	[MaxLength(2)]
	public string omoUnitOfMeasure { get; set; }

	[JsonProperty("omoWeight", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omoWeight { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
