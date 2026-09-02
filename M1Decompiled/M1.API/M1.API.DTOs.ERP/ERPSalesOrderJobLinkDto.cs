using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderJobLinkDto
{
	[JsonProperty("omjCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string omjCreatedBy { get; set; }

	[JsonProperty("omjCreatedDate", Order = 2)]
	public DateTime? omjCreatedDate { get; set; }

	[JsonProperty("omjUniqueID", Order = 3)]
	public Guid omjUniqueID { get; set; }

	[JsonProperty("omjClosed", Order = 4)]
	public bool omjClosed { get; set; }

	[JsonProperty("omjJobID", Order = 5)]
	[Required(ErrorMessage = "omjJobID is required.")]
	[MaxLength(20)]
	public string omjJobID { get; set; }

	[JsonProperty("omjLinkType", Order = 6)]
	[Required(ErrorMessage = "omjLinkType is required.")]
	public byte omjLinkType { get; set; }

	[JsonProperty("omjRowVersion", Order = 7)]
	public byte[] omjRowVersion { get; set; }

	[JsonProperty("omjSalesOrderDeliveryID", Order = 8)]
	public short omjSalesOrderDeliveryID { get; set; }

	[JsonProperty("omjSalesOrderID", Order = 9)]
	[Required(ErrorMessage = "omjSalesOrderID is required.")]
	[MaxLength(10)]
	public string omjSalesOrderID { get; set; }

	[JsonProperty("omjSalesOrderLineID", Order = 10)]
	[Required(ErrorMessage = "omjSalesOrderLineID is required.")]
	public short omjSalesOrderLineID { get; set; }

	[JsonProperty("omjSalesOrderJobLinkID", Order = 11)]
	[Required(ErrorMessage = "omjSalesOrderJobLinkID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int omjSalesOrderJobLinkID { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
