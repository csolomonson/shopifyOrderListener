using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteAssemblyDto
{
	[JsonProperty("qmaAssemblyOverlap", Order = 1)]
	public byte qmaAssemblyOverlap { get; set; }

	[JsonProperty("qmaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qmaCreatedBy { get; set; }

	[JsonProperty("qmaCreatedDate", Order = 3)]
	public DateTime? qmaCreatedDate { get; set; }

	[JsonProperty("qmaDocuments", Order = 4)]
	[MaxLength(50)]
	public string qmaDocuments { get; set; }

	[JsonProperty("qmaUniqueID", Order = 5)]
	public Guid qmaUniqueID { get; set; }

	[JsonProperty("qmaClosed", Order = 6)]
	public bool qmaClosed { get; set; }

	[JsonProperty("qmaPullAllFromStock", Order = 7)]
	public bool qmaPullAllFromStock { get; set; }

	[JsonProperty("qmaLevel", Order = 8)]
	[Required(ErrorMessage = "qmaLevel is required.")]
	public short qmaLevel { get; set; }

	[JsonProperty("qmaOverlapDestinationLink", Order = 9)]
	public byte qmaOverlapDestinationLink { get; set; }

	[JsonProperty("qmaOverlapOffsetTime", Order = 10)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmaOverlapOffsetTime { get; set; }

	[JsonProperty("qmaOverlapOperationID", Order = 11)]
	public int qmaOverlapOperationID { get; set; }

	[JsonProperty("qmaOverlapSourceLink", Order = 12)]
	public byte qmaOverlapSourceLink { get; set; }

	[JsonProperty("qmaOverlapSourceOperationID", Order = 13)]
	public int qmaOverlapSourceOperationID { get; set; }

	[JsonProperty("qmaOverlapType", Order = 14)]
	public byte qmaOverlapType { get; set; }

	[JsonProperty("qmaParentAssemblyID", Order = 15)]
	public int qmaParentAssemblyID { get; set; }

	[JsonProperty("qmaPartID", Order = 16)]
	[Required(ErrorMessage = "qmaPartID is required.")]
	[MaxLength(30)]
	public string qmaPartID { get; set; }

	[JsonProperty("qmaPartLongDescriptionRtf", Order = 17)]
	public string qmaPartLongDescriptionRtf { get; set; }

	[JsonProperty("qmaPartLongDescriptionText", Order = 18)]
	public string qmaPartLongDescriptionText { get; set; }

	[JsonProperty("qmaPartRevisionID", Order = 19)]
	[MaxLength(15)]
	public string qmaPartRevisionID { get; set; }

	[JsonProperty("qmaPartShortDescription", Order = 20)]
	[Required(ErrorMessage = "qmaPartShortDescription is required.")]
	[MaxLength(50)]
	public string qmaPartShortDescription { get; set; }

	[JsonProperty("qmaProductionNotesRTF", Order = 21)]
	[MaxLength(50)]
	public string qmaProductionNotesRTF { get; set; }

	[JsonProperty("qmaProductionNotesText", Order = 22)]
	[MaxLength(50)]
	public string qmaProductionNotesText { get; set; }

	[JsonProperty("qmaQuantityPerParent", Order = 23)]
	[Required(ErrorMessage = "qmaQuantityPerParent is required.")]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmaQuantityPerParent { get; set; }

	[JsonProperty("qmaQuoteID", Order = 24)]
	[Required(ErrorMessage = "qmaQuoteID is required.")]
	[MaxLength(10)]
	public string qmaQuoteID { get; set; }

	[JsonProperty("qmaQuoteLineID", Order = 25)]
	[Required(ErrorMessage = "qmaQuoteLineID is required.")]
	public short qmaQuoteLineID { get; set; }

	[JsonProperty("qmaRowVersion", Order = 26)]
	public byte[] qmaRowVersion { get; set; }

	[JsonProperty("qmaQuoteAssemblyID", Order = 27)]
	[Required(ErrorMessage = "qmaQuoteAssemblyID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int qmaQuoteAssemblyID { get; set; }

	[JsonProperty("qmaSourceMethodID", Order = 28)]
	[MaxLength(30)]
	public string qmaSourceMethodID { get; set; }

	[JsonProperty("qmaSourceRevisionID", Order = 29)]
	[MaxLength(15)]
	public string qmaSourceRevisionID { get; set; }

	[JsonProperty("qmaUnitOfMeasure", Order = 30)]
	[MaxLength(2)]
	public string qmaUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
