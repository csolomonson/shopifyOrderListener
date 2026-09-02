using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteLineDto
{
	[JsonProperty("qmlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string qmlCreatedBy { get; set; }

	[JsonProperty("qmlCreatedDate", Order = 2)]
	public DateTime? qmlCreatedDate { get; set; }

	[JsonProperty("qmlDocuments", Order = 3)]
	[MaxLength(50)]
	public string qmlDocuments { get; set; }

	[JsonProperty("qmlUniqueID", Order = 4)]
	public Guid qmlUniqueID { get; set; }

	[JsonProperty("qmlClosed", Order = 5)]
	public bool qmlClosed { get; set; }

	[JsonProperty("qmlCreatedFromMobile", Order = 6)]
	public bool qmlCreatedFromMobile { get; set; }

	[JsonProperty("qmlFirm", Order = 7)]
	public bool qmlFirm { get; set; }

	[JsonProperty("qmlMatrixCalculated", Order = 8)]
	public bool qmlMatrixCalculated { get; set; }

	[JsonProperty("qmlPurchaseToOrder", Order = 9)]
	public bool qmlPurchaseToOrder { get; set; }

	[JsonProperty("qmlTransferredToOrder", Order = 10)]
	public bool qmlTransferredToOrder { get; set; }

	[JsonProperty("qmlLeadID", Order = 11)]
	[MaxLength(10)]
	public string qmlLeadID { get; set; }

	[JsonProperty("qmlLeadLineID", Order = 12)]
	public short qmlLeadLineID { get; set; }

	[JsonProperty("qmlNonTaxReasonID", Order = 13)]
	[MaxLength(5)]
	public string qmlNonTaxReasonID { get; set; }

	[JsonProperty("qmlOrgPartID", Order = 14)]
	[MaxLength(30)]
	public string qmlOrgPartID { get; set; }

	[JsonProperty("qmlOrgPartShortDescription", Order = 15)]
	[MaxLength(50)]
	public string qmlOrgPartShortDescription { get; set; }

	[JsonProperty("qmlPartGroupID", Order = 16)]
	[MaxLength(5)]
	public string qmlPartGroupID { get; set; }

	[JsonProperty("qmlPartID", Order = 17)]
	[Required(ErrorMessage = "qmlPartID is required.")]
	[MaxLength(30)]
	public string qmlPartID { get; set; }

	[JsonProperty("qmlPartLongDescriptionRtf", Order = 18)]
	public string qmlPartLongDescriptionRtf { get; set; }

	[JsonProperty("qmlPartLongDescriptionText", Order = 19)]
	public string qmlPartLongDescriptionText { get; set; }

	[JsonProperty("qmlPartRevisionID", Order = 20)]
	[MaxLength(15)]
	public string qmlPartRevisionID { get; set; }

	[JsonProperty("qmlPartShortDescription", Order = 21)]
	[Required(ErrorMessage = "qmlPartShortDescription is required.")]
	[MaxLength(50)]
	public string qmlPartShortDescription { get; set; }

	[JsonProperty("qmlProductionNotesRTF", Order = 22)]
	[MaxLength(50)]
	public string qmlProductionNotesRTF { get; set; }

	[JsonProperty("qmlProductionNotesText", Order = 23)]
	[MaxLength(50)]
	public string qmlProductionNotesText { get; set; }

	[JsonProperty("qmlProjectAreaID", Order = 24)]
	[MaxLength(15)]
	public string qmlProjectAreaID { get; set; }

	[JsonProperty("qmlProjectID", Order = 25)]
	[MaxLength(10)]
	public string qmlProjectID { get; set; }

	[JsonProperty("qmlPurchaseLocationID", Order = 26)]
	[MaxLength(5)]
	public string qmlPurchaseLocationID { get; set; }

	[JsonProperty("qmlPurchaseUnitCostBase", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmlPurchaseUnitCostBase { get; set; }

	[JsonProperty("qmlPurchaseUnitCostForeign", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmlPurchaseUnitCostForeign { get; set; }

	[JsonProperty("qmlQuantityToTotal", Order = 29)]
	public byte qmlQuantityToTotal { get; set; }

	[JsonProperty("qmlQuoteID", Order = 30)]
	[Required(ErrorMessage = "qmlQuoteID is required.")]
	[MaxLength(10)]
	public string qmlQuoteID { get; set; }

	[JsonProperty("qmlQuoteMarkupType", Order = 31)]
	[Required(ErrorMessage = "qmlQuoteMarkupType is required.")]
	public byte qmlQuoteMarkupType { get; set; }

	[JsonProperty("qmlResolutionReasonID", Order = 32)]
	[MaxLength(5)]
	public string qmlResolutionReasonID { get; set; }

	[JsonProperty("qmlRowVersion", Order = 33)]
	public byte[] qmlRowVersion { get; set; }

	[JsonProperty("qmlSecondTaxCodeID", Order = 34)]
	[MaxLength(5)]
	public string qmlSecondTaxCodeID { get; set; }

	[JsonProperty("qmlQuoteLineID", Order = 35)]
	[Required(ErrorMessage = "qmlQuoteLineID is required.")]
	public short qmlQuoteLineID { get; set; }

	[JsonProperty("qmlSourceMethodID", Order = 36)]
	[MaxLength(30)]
	public string qmlSourceMethodID { get; set; }

	[JsonProperty("qmlSourceRevisionID", Order = 37)]
	[MaxLength(15)]
	public string qmlSourceRevisionID { get; set; }

	[JsonProperty("qmlSupplierOrganizationID", Order = 38)]
	[MaxLength(10)]
	public string qmlSupplierOrganizationID { get; set; }

	[JsonProperty("qmlTaxCodeID", Order = 39)]
	[MaxLength(5)]
	public string qmlTaxCodeID { get; set; }

	[JsonProperty("qmlTaxDate", Order = 40)]
	public DateTime? qmlTaxDate { get; set; }

	[JsonProperty("qmlUnitOfMeasure", Order = 41)]
	[MaxLength(2)]
	public string qmlUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 42)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
