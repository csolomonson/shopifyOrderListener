using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartDto
{
	[JsonProperty("impPartID", Order = 1)]
	[Required(ErrorMessage = "impPartID is required.")]
	[MaxLength(30)]
	public string impPartID { get; set; }

	[JsonProperty("impContractLength", Order = 2)]
	public short impContractLength { get; set; }

	[JsonProperty("impContractLengthType", Order = 3)]
	[MaxLength(1)]
	public string impContractLengthType { get; set; }

	[JsonProperty("impCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string impCreatedBy { get; set; }

	[JsonProperty("impCreatedDate", Order = 5)]
	public DateTime? impCreatedDate { get; set; }

	[JsonProperty("impCycleCodeID", Order = 6)]
	[MaxLength(5)]
	public string impCycleCodeID { get; set; }

	[JsonProperty("impDeliveryType", Order = 7)]
	public byte impDeliveryType { get; set; }

	[JsonProperty("impUniqueID", Order = 8)]
	public Guid impUniqueID { get; set; }

	[JsonProperty("impInactiveDate", Order = 9)]
	public DateTime? impInactiveDate { get; set; }

	[JsonProperty("impInactive", Order = 10)]
	public bool impInactive { get; set; }

	[JsonProperty("impAlwaysNonTaxable", Order = 11)]
	public bool impAlwaysNonTaxable { get; set; }

	[JsonProperty("impBuyForInventory", Order = 12)]
	public bool impBuyForInventory { get; set; }

	[JsonProperty("impNonPhysicalShipment", Order = 13)]
	public bool impNonPhysicalShipment { get; set; }

	[JsonProperty("impNonStockedItem", Order = 14)]
	public bool impNonStockedItem { get; set; }

	[JsonProperty("impPhantomOrKitPart", Order = 15)]
	public bool impPhantomOrKitPart { get; set; }

	[JsonProperty("impTrackLotNumbers", Order = 16)]
	public bool impTrackLotNumbers { get; set; }

	[JsonProperty("impTrackSerialNumbers", Order = 17)]
	public bool impTrackSerialNumbers { get; set; }

	[JsonProperty("impLongDescriptionRtf", Order = 18)]
	public string impLongDescriptionRtf { get; set; }

	[JsonProperty("impLongDescriptionText", Order = 19)]
	public string impLongDescriptionText { get; set; }

	[JsonProperty("impNextSerialNumberIDFormula", Order = 20)]
	[MaxLength(50)]
	public string impNextSerialNumberIDFormula { get; set; }

	[JsonProperty("impNonTaxReasonID", Order = 21)]
	[MaxLength(5)]
	public string impNonTaxReasonID { get; set; }

	[JsonProperty("impOEMOrganizationID", Order = 22)]
	[MaxLength(10)]
	public string impOEMOrganizationID { get; set; }

	[JsonProperty("impPartClassID", Order = 23)]
	[MaxLength(5)]
	public string impPartClassID { get; set; }

	[JsonProperty("impPartGroupID", Order = 24)]
	[MaxLength(5)]
	public string impPartGroupID { get; set; }

	[JsonProperty("impPartType", Order = 25)]
	[Required(ErrorMessage = "impPartType is required.")]
	public byte impPartType { get; set; }

	[JsonProperty("impReorderMethod", Order = 26)]
	public byte impReorderMethod { get; set; }

	[JsonProperty("impRowVersion", Order = 27)]
	public byte[] impRowVersion { get; set; }

	[JsonProperty("impSecondTaxCodeID", Order = 28)]
	[MaxLength(5)]
	public string impSecondTaxCodeID { get; set; }

	[JsonProperty("impShortDescription", Order = 29)]
	[Required(ErrorMessage = "impShortDescription is required.")]
	[MaxLength(50)]
	public string impShortDescription { get; set; }

	[JsonProperty("impTaxCodeID", Order = 30)]
	[MaxLength(5)]
	public string impTaxCodeID { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
