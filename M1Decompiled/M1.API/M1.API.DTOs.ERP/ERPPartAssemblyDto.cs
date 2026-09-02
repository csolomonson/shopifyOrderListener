using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartAssemblyDto
{
	[JsonProperty("imaAssemblyOverlap", Order = 1)]
	public byte imaAssemblyOverlap { get; set; }

	[JsonProperty("imaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imaCreatedBy { get; set; }

	[JsonProperty("imaCreatedDate", Order = 3)]
	public DateTime? imaCreatedDate { get; set; }

	[JsonProperty("imaDocuments", Order = 4)]
	[MaxLength(50)]
	public string imaDocuments { get; set; }

	[JsonProperty("imaUniqueID", Order = 5)]
	public Guid imaUniqueID { get; set; }

	[JsonProperty("imaPullAllFromStock", Order = 6)]
	public bool imaPullAllFromStock { get; set; }

	[JsonProperty("imaUseMethod", Order = 7)]
	public bool imaUseMethod { get; set; }

	[JsonProperty("imaLevel", Order = 8)]
	[Required(ErrorMessage = "imaLevel is required.")]
	public short imaLevel { get; set; }

	[JsonProperty("imaMethodAssemblyID", Order = 9)]
	public int imaMethodAssemblyID { get; set; }

	[JsonProperty("imaMethodID", Order = 10)]
	[Required(ErrorMessage = "imaMethodID is required.")]
	[MaxLength(30)]
	public string imaMethodID { get; set; }

	[JsonProperty("imaMethodRevisionID", Order = 11)]
	[MaxLength(15)]
	public string imaMethodRevisionID { get; set; }

	[JsonProperty("imaOverlapDestinationLink", Order = 12)]
	public byte imaOverlapDestinationLink { get; set; }

	[JsonProperty("imaOverlapOffsetTime", Order = 13)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imaOverlapOffsetTime { get; set; }

	[JsonProperty("imaOverlapOperationID", Order = 14)]
	public int imaOverlapOperationID { get; set; }

	[JsonProperty("imaOverlapSourceLink", Order = 15)]
	public byte imaOverlapSourceLink { get; set; }

	[JsonProperty("imaOverlapSourceOperationID", Order = 16)]
	public int imaOverlapSourceOperationID { get; set; }

	[JsonProperty("imaOverlapType", Order = 17)]
	public byte imaOverlapType { get; set; }

	[JsonProperty("imaParentAssemblyID", Order = 18)]
	public int imaParentAssemblyID { get; set; }

	[JsonProperty("imaPartID", Order = 19)]
	[Required(ErrorMessage = "imaPartID is required.")]
	[MaxLength(30)]
	public string imaPartID { get; set; }

	[JsonProperty("imaPartLongDescriptionRtf", Order = 20)]
	public string imaPartLongDescriptionRtf { get; set; }

	[JsonProperty("imaPartLongDescriptionText", Order = 21)]
	public string imaPartLongDescriptionText { get; set; }

	[JsonProperty("imaPartRevisionID", Order = 22)]
	[MaxLength(15)]
	public string imaPartRevisionID { get; set; }

	[JsonProperty("imaPartShortDescription", Order = 23)]
	[Required(ErrorMessage = "imaPartShortDescription is required.")]
	[MaxLength(50)]
	public string imaPartShortDescription { get; set; }

	[JsonProperty("imaProductionNotesRTF", Order = 24)]
	[MaxLength(50)]
	public string imaProductionNotesRTF { get; set; }

	[JsonProperty("imaProductionNotesText", Order = 25)]
	[MaxLength(50)]
	public string imaProductionNotesText { get; set; }

	[JsonProperty("imaQuantityPerParent", Order = 26)]
	[Required(ErrorMessage = "imaQuantityPerParent is required.")]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imaQuantityPerParent { get; set; }

	[JsonProperty("imaRowVersion", Order = 27)]
	public byte[] imaRowVersion { get; set; }

	[JsonProperty("imaSourceMethodID", Order = 28)]
	[MaxLength(30)]
	public string imaSourceMethodID { get; set; }

	[JsonProperty("imaSourceRevisionID", Order = 29)]
	[MaxLength(15)]
	public string imaSourceRevisionID { get; set; }

	[JsonProperty("imaUnitOfMeasure", Order = 30)]
	[MaxLength(2)]
	public string imaUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
