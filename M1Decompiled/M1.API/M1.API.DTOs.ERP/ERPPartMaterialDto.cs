using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartMaterialDto
{
	[JsonProperty("immCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string immCreatedBy { get; set; }

	[JsonProperty("immCreatedDate", Order = 2)]
	public DateTime? immCreatedDate { get; set; }

	[JsonProperty("immDocuments", Order = 3)]
	[MaxLength(50)]
	public string immDocuments { get; set; }

	[JsonProperty("immUniqueID", Order = 4)]
	public Guid immUniqueID { get; set; }

	[JsonProperty("immEstimatedUnitCost", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal immEstimatedUnitCost { get; set; }

	[JsonProperty("immBackflush", Order = 6)]
	public bool immBackflush { get; set; }

	[JsonProperty("immManualPart", Order = 7)]
	public bool immManualPart { get; set; }

	[JsonProperty("immUseDefaultWarehouseAndBin", Order = 8)]
	public bool immUseDefaultWarehouseAndBin { get; set; }

	[JsonProperty("immLeadTime", Order = 9)]
	public short immLeadTime { get; set; }

	[JsonProperty("immMethodAssemblyID", Order = 10)]
	public int immMethodAssemblyID { get; set; }

	[JsonProperty("immMethodID", Order = 11)]
	[Required(ErrorMessage = "immMethodID is required.")]
	[MaxLength(30)]
	public string immMethodID { get; set; }

	[JsonProperty("immMethodMaterialID", Order = 12)]
	[Required(ErrorMessage = "immMethodMaterialID is required.")]
	public int immMethodMaterialID { get; set; }

	[JsonProperty("immMethodRevisionID", Order = 13)]
	[MaxLength(15)]
	public string immMethodRevisionID { get; set; }

	[JsonProperty("immMinimumCharge", Order = 14)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal immMinimumCharge { get; set; }

	[JsonProperty("immPartBinID", Order = 15)]
	[Required(ErrorMessage = "immPartBinID is required.")]
	[MaxLength(15)]
	public string immPartBinID { get; set; }

	[JsonProperty("immPartID", Order = 16)]
	[Required(ErrorMessage = "immPartID is required.")]
	[MaxLength(30)]
	public string immPartID { get; set; }

	[JsonProperty("immPartLongDescriptionRtf", Order = 17)]
	public string immPartLongDescriptionRtf { get; set; }

	[JsonProperty("immPartLongDescriptionText", Order = 18)]
	public string immPartLongDescriptionText { get; set; }

	[JsonProperty("immPartRevisionID", Order = 19)]
	[MaxLength(15)]
	public string immPartRevisionID { get; set; }

	[JsonProperty("immPartShortDescription", Order = 20)]
	[Required(ErrorMessage = "immPartShortDescription is required.")]
	[MaxLength(50)]
	public string immPartShortDescription { get; set; }

	[JsonProperty("immPartWarehouseLocationID", Order = 21)]
	[Required(ErrorMessage = "immPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string immPartWarehouseLocationID { get; set; }

	[JsonProperty("immPurchaseLocationID", Order = 22)]
	[MaxLength(5)]
	public string immPurchaseLocationID { get; set; }

	[JsonProperty("immQuantityPerAssembly", Order = 23)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal immQuantityPerAssembly { get; set; }

	[JsonProperty("immRelatedPartOperationID", Order = 24)]
	public int immRelatedPartOperationID { get; set; }

	[JsonProperty("immRowVersion", Order = 25)]
	public byte[] immRowVersion { get; set; }

	[JsonProperty("immScrapPercent", Order = 26)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal immScrapPercent { get; set; }

	[JsonProperty("immScrapQuantity", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal immScrapQuantity { get; set; }

	[JsonProperty("immSupplierOrganizationID", Order = 28)]
	[MaxLength(10)]
	public string immSupplierOrganizationID { get; set; }

	[JsonProperty("immUnitOfMeasure", Order = 29)]
	[MaxLength(2)]
	public string immUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 30)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
