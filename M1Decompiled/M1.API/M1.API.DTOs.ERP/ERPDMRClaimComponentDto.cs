using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDMRClaimComponentDto
{
	[JsonProperty("dmoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmoAdditionalQuantity { get; set; }

	[JsonProperty("dmoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string dmoCreatedBy { get; set; }

	[JsonProperty("dmoCreatedDate", Order = 3)]
	public DateTime? dmoCreatedDate { get; set; }

	[JsonProperty("dmoDescription", Order = 4)]
	[Required(ErrorMessage = "dmoDescription is required.")]
	[MaxLength(50)]
	public string dmoDescription { get; set; }

	[JsonProperty("dmoDmrClaimID", Order = 5)]
	[Required(ErrorMessage = "dmoDmrClaimID is required.")]
	[MaxLength(10)]
	public string dmoDmrClaimID { get; set; }

	[JsonProperty("dmoDmrClaimLineID", Order = 6)]
	[Required(ErrorMessage = "dmoDmrClaimLineID is required.")]
	public short dmoDmrClaimLineID { get; set; }

	[JsonProperty("dmoUniqueID", Order = 7)]
	public Guid dmoUniqueID { get; set; }

	[JsonProperty("dmoInspectionComponentID", Order = 8)]
	public int dmoInspectionComponentID { get; set; }

	[JsonProperty("dmoInspectionID", Order = 9)]
	[MaxLength(10)]
	public string dmoInspectionID { get; set; }

	[JsonProperty("dmoInspectionLineID", Order = 10)]
	public short dmoInspectionLineID { get; set; }

	[JsonProperty("dmoShippedComplete", Order = 11)]
	public bool dmoShippedComplete { get; set; }

	[JsonProperty("dmoJobAssemblyID", Order = 12)]
	public int dmoJobAssemblyID { get; set; }

	[JsonProperty("dmoJobID", Order = 13)]
	[MaxLength(20)]
	public string dmoJobID { get; set; }

	[JsonProperty("dmoJobMaterialComponentID", Order = 14)]
	public int dmoJobMaterialComponentID { get; set; }

	[JsonProperty("dmoJobMaterialID", Order = 15)]
	public int dmoJobMaterialID { get; set; }

	[JsonProperty("dmoParentQuantity", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmoParentQuantity { get; set; }

	[JsonProperty("dmoPartBinID", Order = 17)]
	[Required(ErrorMessage = "dmoPartBinID is required.")]
	[MaxLength(15)]
	public string dmoPartBinID { get; set; }

	[JsonProperty("dmoPartID", Order = 18)]
	[Required(ErrorMessage = "dmoPartID is required.")]
	[MaxLength(30)]
	public string dmoPartID { get; set; }

	[JsonProperty("dmoPartRevisionID", Order = 19)]
	[MaxLength(15)]
	public string dmoPartRevisionID { get; set; }

	[JsonProperty("dmoPartWarehouseLocationID", Order = 20)]
	[Required(ErrorMessage = "dmoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string dmoPartWarehouseLocationID { get; set; }

	[JsonProperty("dmoQuantity", Order = 21)]
	[Required(ErrorMessage = "dmoQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmoQuantity { get; set; }

	[JsonProperty("dmoQuantityPerParent", Order = 22)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmoQuantityPerParent { get; set; }

	[JsonProperty("dmoQuantityShipped", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmoQuantityShipped { get; set; }

	[JsonProperty("dmoRowVersion", Order = 24)]
	public byte[] dmoRowVersion { get; set; }

	[JsonProperty("dmoDmrClaimComponentID", Order = 25)]
	[Required(ErrorMessage = "dmoDmrClaimComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int dmoDmrClaimComponentID { get; set; }

	[JsonProperty("dmoUnitOfMeasure", Order = 26)]
	[MaxLength(2)]
	public string dmoUnitOfMeasure { get; set; }

	[JsonProperty("dmoWeight", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmoWeight { get; set; }

	[JsonProperty("customFields", Order = 28)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
