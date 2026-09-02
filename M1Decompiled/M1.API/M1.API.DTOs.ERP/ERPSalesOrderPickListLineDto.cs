using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderPickListLineDto
{
	[JsonProperty("omyCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string omyCreatedBy { get; set; }

	[JsonProperty("omyCreatedDate", Order = 2)]
	public DateTime? omyCreatedDate { get; set; }

	[JsonProperty("omyDeliveryDate", Order = 3)]
	public DateTime? omyDeliveryDate { get; set; }

	[JsonProperty("omyUniqueID", Order = 4)]
	public Guid omyUniqueID { get; set; }

	[JsonProperty("omyOpenQuantity", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omyOpenQuantity { get; set; }

	[JsonProperty("omyPartBinID", Order = 6)]
	[MaxLength(15)]
	public string omyPartBinID { get; set; }

	[JsonProperty("omyPartID", Order = 7)]
	[MaxLength(30)]
	public string omyPartID { get; set; }

	[JsonProperty("omyPartRevisionID", Order = 8)]
	[MaxLength(15)]
	public string omyPartRevisionID { get; set; }

	[JsonProperty("omyPartWareHouseLocationID", Order = 9)]
	[MaxLength(5)]
	public string omyPartWareHouseLocationID { get; set; }

	[JsonProperty("omyPickDate", Order = 10)]
	public DateTime? omyPickDate { get; set; }

	[JsonProperty("omyPickListLineID", Order = 11)]
	[Required(ErrorMessage = "omyPickListLineID is required.")]
	public short omyPickListLineID { get; set; }

	[JsonProperty("omyPickListSessionID", Order = 12)]
	public int omyPickListSessionID { get; set; }

	[JsonProperty("omyPickQuantity", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omyPickQuantity { get; set; }

	[JsonProperty("omyRowVersion", Order = 14)]
	public byte[] omyRowVersion { get; set; }

	[JsonProperty("omySalesOrderDeliveryID", Order = 15)]
	public short omySalesOrderDeliveryID { get; set; }

	[JsonProperty("omySalesOrderID", Order = 16)]
	[MaxLength(10)]
	public string omySalesOrderID { get; set; }

	[JsonProperty("omySalesOrderLineID", Order = 17)]
	public short omySalesOrderLineID { get; set; }

	[JsonProperty("omyStatus", Order = 18)]
	public byte omyStatus { get; set; }

	[JsonProperty("customFields", Order = 19)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
