using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRFQLineDto
{
	[JsonProperty("rqlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string rqlCreatedBy { get; set; }

	[JsonProperty("rqlCreatedDate", Order = 2)]
	public DateTime? rqlCreatedDate { get; set; }

	[JsonProperty("rqlDocuments", Order = 3)]
	[MaxLength(50)]
	public string rqlDocuments { get; set; }

	[JsonProperty("rqlUniqueID", Order = 4)]
	public Guid rqlUniqueID { get; set; }

	[JsonProperty("rqlInventoryUnitOfMeasure", Order = 5)]
	[MaxLength(2)]
	public string rqlInventoryUnitOfMeasure { get; set; }

	[JsonProperty("rqlAlternatePart", Order = 6)]
	public bool rqlAlternatePart { get; set; }

	[JsonProperty("rqlClosed", Order = 7)]
	public bool rqlClosed { get; set; }

	[JsonProperty("rqlJobAssemblyID", Order = 8)]
	public int rqlJobAssemblyID { get; set; }

	[JsonProperty("rqlJobEstimatedQty", Order = 9)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rqlJobEstimatedQty { get; set; }

	[JsonProperty("rqlJobID", Order = 10)]
	[MaxLength(20)]
	public string rqlJobID { get; set; }

	[JsonProperty("rqlJobMaterialID", Order = 11)]
	public int rqlJobMaterialID { get; set; }

	[JsonProperty("rqlJobOperationID", Order = 12)]
	public int rqlJobOperationID { get; set; }

	[JsonProperty("rqlPartID", Order = 13)]
	[Required(ErrorMessage = "rqlPartID is required.")]
	[MaxLength(30)]
	public string rqlPartID { get; set; }

	[JsonProperty("rqlPartLongDescriptionRtf", Order = 14)]
	public string rqlPartLongDescriptionRtf { get; set; }

	[JsonProperty("rqlPartLongDescriptionText", Order = 15)]
	public string rqlPartLongDescriptionText { get; set; }

	[JsonProperty("rqlPartRevisionID", Order = 16)]
	[MaxLength(15)]
	public string rqlPartRevisionID { get; set; }

	[JsonProperty("rqlPartShortDescription", Order = 17)]
	[Required(ErrorMessage = "rqlPartShortDescription is required.")]
	[MaxLength(50)]
	public string rqlPartShortDescription { get; set; }

	[JsonProperty("rqlProjectAreaID", Order = 18)]
	[MaxLength(15)]
	public string rqlProjectAreaID { get; set; }

	[JsonProperty("rqlProjectID", Order = 19)]
	[MaxLength(10)]
	public string rqlProjectID { get; set; }

	[JsonProperty("rqlPurchaseUnitOfMeasure", Order = 20)]
	[Required(ErrorMessage = "rqlPurchaseUnitOfMeasure is required.")]
	[MaxLength(2)]
	public string rqlPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("rqlQuoteAssemblyID", Order = 21)]
	public int rqlQuoteAssemblyID { get; set; }

	[JsonProperty("rqlQuoteID", Order = 22)]
	[MaxLength(10)]
	public string rqlQuoteID { get; set; }

	[JsonProperty("rqlQuoteLineID", Order = 23)]
	public short rqlQuoteLineID { get; set; }

	[JsonProperty("rqlQuoteMaterialID", Order = 24)]
	public int rqlQuoteMaterialID { get; set; }

	[JsonProperty("rqlQuoteOperationID", Order = 25)]
	public int rqlQuoteOperationID { get; set; }

	[JsonProperty("rqlRfqID", Order = 26)]
	[Required(ErrorMessage = "rqlRfqID is required.")]
	[MaxLength(10)]
	public string rqlRfqID { get; set; }

	[JsonProperty("rqlRfqType", Order = 27)]
	public byte rqlRfqType { get; set; }

	[JsonProperty("rqlRowVersion", Order = 28)]
	public byte[] rqlRowVersion { get; set; }

	[JsonProperty("rqlRfqLineID", Order = 29)]
	[Required(ErrorMessage = "rqlRfqLineID is required.")]
	public short rqlRfqLineID { get; set; }

	[JsonProperty("customFields", Order = 30)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
