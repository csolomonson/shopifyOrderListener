using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationMemoDto
{
	[JsonProperty("cmmContactID", Order = 1)]
	[MaxLength(5)]
	public string cmmContactID { get; set; }

	[JsonProperty("cmmCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmmCreatedBy { get; set; }

	[JsonProperty("cmmCreatedDate", Order = 3)]
	public DateTime? cmmCreatedDate { get; set; }

	[JsonProperty("cmmUniqueID", Order = 4)]
	public Guid cmmUniqueID { get; set; }

	[JsonProperty("cmmLocationID", Order = 5)]
	[MaxLength(5)]
	public string cmmLocationID { get; set; }

	[JsonProperty("cmmLongDescriptionRtf", Order = 6)]
	public string cmmLongDescriptionRtf { get; set; }

	[JsonProperty("cmmLongDescriptionText", Order = 7)]
	public string cmmLongDescriptionText { get; set; }

	[JsonProperty("cmmMemoDate", Order = 8)]
	[Required(ErrorMessage = "cmmMemoDate is required.")]
	public DateTime? cmmMemoDate { get; set; }

	[JsonProperty("cmmOrganizationID", Order = 9)]
	[Required(ErrorMessage = "cmmOrganizationID is required.")]
	[MaxLength(10)]
	public string cmmOrganizationID { get; set; }

	[JsonProperty("cmmRowVersion", Order = 10)]
	public byte[] cmmRowVersion { get; set; }

	[JsonProperty("cmmOrganizationMemoID", Order = 11)]
	[Required(ErrorMessage = "cmmOrganizationMemoID is required.")]
	public short cmmOrganizationMemoID { get; set; }

	[JsonProperty("cmmShortDescription", Order = 12)]
	[Required(ErrorMessage = "cmmShortDescription is required.")]
	[MaxLength(50)]
	public string cmmShortDescription { get; set; }

	[JsonProperty("cmmShowInApInvoices", Order = 13)]
	public bool cmmShowInApInvoices { get; set; }

	[JsonProperty("cmmShowInApPayments", Order = 14)]
	public bool cmmShowInApPayments { get; set; }

	[JsonProperty("cmmShowInArInvoices", Order = 15)]
	public bool cmmShowInArInvoices { get; set; }

	[JsonProperty("cmmShowInArPayments", Order = 16)]
	public bool cmmShowInArPayments { get; set; }

	[JsonProperty("cmmShowInCalls", Order = 17)]
	public bool cmmShowInCalls { get; set; }

	[JsonProperty("cmmShowInDmrClaims", Order = 18)]
	public bool cmmShowInDmrClaims { get; set; }

	[JsonProperty("cmmShowInDmrShipments", Order = 19)]
	public bool cmmShowInDmrShipments { get; set; }

	[JsonProperty("cmmShowInLeads", Order = 20)]
	public bool cmmShowInLeads { get; set; }

	[JsonProperty("cmmShowInOrganizations", Order = 21)]
	public bool cmmShowInOrganizations { get; set; }

	[JsonProperty("cmmShowInPriceAndAvailability", Order = 22)]
	public bool cmmShowInPriceAndAvailability { get; set; }

	[JsonProperty("cmmShowInPurchaseOrders", Order = 23)]
	public bool cmmShowInPurchaseOrders { get; set; }

	[JsonProperty("cmmShowInQuotes", Order = 24)]
	public bool cmmShowInQuotes { get; set; }

	[JsonProperty("cmmShowInReceipts", Order = 25)]
	public bool cmmShowInReceipts { get; set; }

	[JsonProperty("cmmShowInRfqs", Order = 26)]
	public bool cmmShowInRfqs { get; set; }

	[JsonProperty("cmmShowInRmaClaims", Order = 27)]
	public bool cmmShowInRmaClaims { get; set; }

	[JsonProperty("cmmShowInRmaReceipts", Order = 28)]
	public bool cmmShowInRmaReceipts { get; set; }

	[JsonProperty("cmmShowInSalesOrders", Order = 29)]
	public bool cmmShowInSalesOrders { get; set; }

	[JsonProperty("cmmShowInShipments", Order = 30)]
	public bool cmmShowInShipments { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
