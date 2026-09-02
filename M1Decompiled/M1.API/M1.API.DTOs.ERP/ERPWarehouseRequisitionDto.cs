using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseRequisitionDto
{
	[JsonProperty("wqpClosedDate", Order = 1)]
	public DateTime? wqpClosedDate { get; set; }

	[JsonProperty("wqpWarehouseRequisitionID", Order = 2)]
	[Required(ErrorMessage = "wqpWarehouseRequisitionID is required.")]
	[MaxLength(10)]
	public string wqpWarehouseRequisitionID { get; set; }

	[JsonProperty("wqpCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string wqpCreatedBy { get; set; }

	[JsonProperty("wqpCreatedDate", Order = 4)]
	public DateTime? wqpCreatedDate { get; set; }

	[JsonProperty("wqpDestinationWarehouseID", Order = 5)]
	[Required(ErrorMessage = "wqpDestinationWarehouseID is required.")]
	[MaxLength(5)]
	public string wqpDestinationWarehouseID { get; set; }

	[JsonProperty("wqpUniqueID", Order = 6)]
	public Guid wqpUniqueID { get; set; }

	[JsonProperty("wqpClosed", Order = 7)]
	public bool wqpClosed { get; set; }

	[JsonProperty("wqpReadyToPrint", Order = 8)]
	public bool wqpReadyToPrint { get; set; }

	[JsonProperty("wqpRequestedShipDate", Order = 9)]
	[Required(ErrorMessage = "wqpRequestedShipDate is required.")]
	public DateTime? wqpRequestedShipDate { get; set; }

	[JsonProperty("wqpRequisitionCommentsRTF", Order = 10)]
	[MaxLength(50)]
	public string wqpRequisitionCommentsRTF { get; set; }

	[JsonProperty("wqpRequisitionCommentsText", Order = 11)]
	[MaxLength(50)]
	public string wqpRequisitionCommentsText { get; set; }

	[JsonProperty("wqpRequisitionDate", Order = 12)]
	[Required(ErrorMessage = "wqpRequisitionDate is required.")]
	public DateTime? wqpRequisitionDate { get; set; }

	[JsonProperty("wqpRowVersion", Order = 13)]
	public byte[] wqpRowVersion { get; set; }

	[JsonProperty("wqpShippingMethodID", Order = 14)]
	[MaxLength(5)]
	public string wqpShippingMethodID { get; set; }

	[JsonProperty("wqpShippingPaymentTypeID", Order = 15)]
	[MaxLength(5)]
	public string wqpShippingPaymentTypeID { get; set; }

	[JsonProperty("wqpSourceWarehouseID", Order = 16)]
	[Required(ErrorMessage = "wqpSourceWarehouseID is required.")]
	[MaxLength(5)]
	public string wqpSourceWarehouseID { get; set; }

	[JsonProperty("wqpStatus", Order = 17)]
	[Required(ErrorMessage = "wqpStatus is required.")]
	public byte wqpStatus { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
