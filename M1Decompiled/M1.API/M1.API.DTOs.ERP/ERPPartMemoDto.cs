using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartMemoDto
{
	[JsonProperty("imkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imkCreatedBy { get; set; }

	[JsonProperty("imkCreatedDate", Order = 2)]
	public DateTime? imkCreatedDate { get; set; }

	[JsonProperty("imkUniqueID", Order = 3)]
	public Guid imkUniqueID { get; set; }

	[JsonProperty("imkLongDescriptionRtf", Order = 4)]
	public string imkLongDescriptionRtf { get; set; }

	[JsonProperty("imkLongDescriptionText", Order = 5)]
	public string imkLongDescriptionText { get; set; }

	[JsonProperty("imkMemoDate", Order = 6)]
	[Required(ErrorMessage = "imkMemoDate is required.")]
	public DateTime? imkMemoDate { get; set; }

	[JsonProperty("imkPartID", Order = 7)]
	[Required(ErrorMessage = "imkPartID is required.")]
	[MaxLength(30)]
	public string imkPartID { get; set; }

	[JsonProperty("imkPartRevisionID", Order = 8)]
	[MaxLength(15)]
	public string imkPartRevisionID { get; set; }

	[JsonProperty("imkRowVersion", Order = 9)]
	public byte[] imkRowVersion { get; set; }

	[JsonProperty("imkPartMemoID", Order = 10)]
	[Required(ErrorMessage = "imkPartMemoID is required.")]
	public short imkPartMemoID { get; set; }

	[JsonProperty("imkShortDescription", Order = 11)]
	[Required(ErrorMessage = "imkShortDescription is required.")]
	[MaxLength(50)]
	public string imkShortDescription { get; set; }

	[JsonProperty("imkShowInApInvoices", Order = 12)]
	public bool imkShowInApInvoices { get; set; }

	[JsonProperty("imkShowInArInvoices", Order = 13)]
	public bool imkShowInArInvoices { get; set; }

	[JsonProperty("imkShowInCalls", Order = 14)]
	public bool imkShowInCalls { get; set; }

	[JsonProperty("imkShowInChangeRequests", Order = 15)]
	public bool imkShowInChangeRequests { get; set; }

	[JsonProperty("imkShowInDmrClaims", Order = 16)]
	public bool imkShowInDmrClaims { get; set; }

	[JsonProperty("imkShowInDmrShipments", Order = 17)]
	public bool imkShowInDmrShipments { get; set; }

	[JsonProperty("imkShowInInspections", Order = 18)]
	public bool imkShowInInspections { get; set; }

	[JsonProperty("imkShowInJobAssemblies", Order = 19)]
	public bool imkShowInJobAssemblies { get; set; }

	[JsonProperty("imkShowInJobMaterials", Order = 20)]
	public bool imkShowInJobMaterials { get; set; }

	[JsonProperty("imkShowInJobOperations", Order = 21)]
	public bool imkShowInJobOperations { get; set; }

	[JsonProperty("imkShowInJobs", Order = 22)]
	public bool imkShowInJobs { get; set; }

	[JsonProperty("imkShowInKnowledgebasePages", Order = 23)]
	public bool imkShowInKnowledgebasePages { get; set; }

	[JsonProperty("imkShowInLeads", Order = 24)]
	public bool imkShowInLeads { get; set; }

	[JsonProperty("imkShowInNonconformances", Order = 25)]
	public bool imkShowInNonconformances { get; set; }

	[JsonProperty("imkShowInPartAssemblies", Order = 26)]
	public bool imkShowInPartAssemblies { get; set; }

	[JsonProperty("imkShowInPartMaterials", Order = 27)]
	public bool imkShowInPartMaterials { get; set; }

	[JsonProperty("imkShowInPartOperations", Order = 28)]
	public bool imkShowInPartOperations { get; set; }

	[JsonProperty("imkShowInPartRevisions", Order = 29)]
	public bool imkShowInPartRevisions { get; set; }

	[JsonProperty("imkShowInPriceAndAvailability", Order = 30)]
	public bool imkShowInPriceAndAvailability { get; set; }

	[JsonProperty("imkShowInPurchaseOrders", Order = 31)]
	public bool imkShowInPurchaseOrders { get; set; }

	[JsonProperty("imkShowInQuoteAssemblies", Order = 32)]
	public bool imkShowInQuoteAssemblies { get; set; }

	[JsonProperty("imkShowInQuoteLines", Order = 33)]
	public bool imkShowInQuoteLines { get; set; }

	[JsonProperty("imkShowInQuoteMaterials", Order = 34)]
	public bool imkShowInQuoteMaterials { get; set; }

	[JsonProperty("imkShowInQuoteOperations", Order = 35)]
	public bool imkShowInQuoteOperations { get; set; }

	[JsonProperty("imkShowInReceipts", Order = 36)]
	public bool imkShowInReceipts { get; set; }

	[JsonProperty("imkShowInRfqs", Order = 37)]
	public bool imkShowInRfqs { get; set; }

	[JsonProperty("imkShowInRmaClaims", Order = 38)]
	public bool imkShowInRmaClaims { get; set; }

	[JsonProperty("imkShowInRmaReceipts", Order = 39)]
	public bool imkShowInRmaReceipts { get; set; }

	[JsonProperty("imkShowInSalesOrders", Order = 40)]
	public bool imkShowInSalesOrders { get; set; }

	[JsonProperty("imkShowInServiceContracts", Order = 41)]
	public bool imkShowInServiceContracts { get; set; }

	[JsonProperty("imkShowInShipments", Order = 42)]
	public bool imkShowInShipments { get; set; }

	[JsonProperty("imkShowInWarehouseReceipts", Order = 43)]
	public bool imkShowInWarehouseReceipts { get; set; }

	[JsonProperty("imkShowInWarehouseRequisitions", Order = 44)]
	public bool imkShowInWarehouseRequisitions { get; set; }

	[JsonProperty("imkShowInWarehouseTransfers", Order = 45)]
	public bool imkShowInWarehouseTransfers { get; set; }

	[JsonProperty("customFields", Order = 46)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
