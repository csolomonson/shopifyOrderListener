using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDMRShipmentComponentDto
{
	[JsonProperty("dsoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoAdditionalQuantity { get; set; }

	[JsonProperty("dsoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string dsoCreatedBy { get; set; }

	[JsonProperty("dsoCreatedDate", Order = 3)]
	public DateTime? dsoCreatedDate { get; set; }

	[JsonProperty("dsoDescription", Order = 4)]
	[Required(ErrorMessage = "dsoDescription is required.")]
	[MaxLength(50)]
	public string dsoDescription { get; set; }

	[JsonProperty("dsoDmrClaimComponentID", Order = 5)]
	public int dsoDmrClaimComponentID { get; set; }

	[JsonProperty("dsoDmrClaimID", Order = 6)]
	[MaxLength(10)]
	public string dsoDmrClaimID { get; set; }

	[JsonProperty("dsoDmrClaimLineID", Order = 7)]
	public short dsoDmrClaimLineID { get; set; }

	[JsonProperty("dsoDmrShipmentID", Order = 8)]
	[Required(ErrorMessage = "dsoDmrShipmentID is required.")]
	[MaxLength(10)]
	public string dsoDmrShipmentID { get; set; }

	[JsonProperty("dsoDmrShipmentLineID", Order = 9)]
	[Required(ErrorMessage = "dsoDmrShipmentLineID is required.")]
	public short dsoDmrShipmentLineID { get; set; }

	[JsonProperty("dsoUniqueID", Order = 10)]
	public Guid dsoUniqueID { get; set; }

	[JsonProperty("dsoInspectionComponentID", Order = 11)]
	public int dsoInspectionComponentID { get; set; }

	[JsonProperty("dsoInspectionID", Order = 12)]
	[MaxLength(10)]
	public string dsoInspectionID { get; set; }

	[JsonProperty("dsoInspectionLineID", Order = 13)]
	public short dsoInspectionLineID { get; set; }

	[JsonProperty("dsoInvParentQuantity", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoInvParentQuantity { get; set; }

	[JsonProperty("dsoInvQuantityShipped", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoInvQuantityShipped { get; set; }

	[JsonProperty("dsoClosed", Order = 16)]
	public bool dsoClosed { get; set; }

	[JsonProperty("dsoPosted", Order = 17)]
	public bool dsoPosted { get; set; }

	[JsonProperty("dsoReversed", Order = 18)]
	public bool dsoReversed { get; set; }

	[JsonProperty("dsoShippedComplete", Order = 19)]
	public bool dsoShippedComplete { get; set; }

	[JsonProperty("dsoJobAssemblyID", Order = 20)]
	public int dsoJobAssemblyID { get; set; }

	[JsonProperty("dsoJobID", Order = 21)]
	[MaxLength(20)]
	public string dsoJobID { get; set; }

	[JsonProperty("dsoJobMaterialComponentID", Order = 22)]
	public int dsoJobMaterialComponentID { get; set; }

	[JsonProperty("dsoJobMaterialID", Order = 23)]
	public int dsoJobMaterialID { get; set; }

	[JsonProperty("dsoJobMatParentQuantity", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoJobMatParentQuantity { get; set; }

	[JsonProperty("dsoJobMatQuantityShipped", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoJobMatQuantityShipped { get; set; }

	[JsonProperty("dsoPartBinID", Order = 26)]
	[Required(ErrorMessage = "dsoPartBinID is required.")]
	[MaxLength(15)]
	public string dsoPartBinID { get; set; }

	[JsonProperty("dsoPartID", Order = 27)]
	[Required(ErrorMessage = "dsoPartID is required.")]
	[MaxLength(30)]
	public string dsoPartID { get; set; }

	[JsonProperty("dsoPartRevisionID", Order = 28)]
	[MaxLength(15)]
	public string dsoPartRevisionID { get; set; }

	[JsonProperty("dsoPartWarehouseLocationID", Order = 29)]
	[Required(ErrorMessage = "dsoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string dsoPartWarehouseLocationID { get; set; }

	[JsonProperty("dsoQuantityPerParent", Order = 30)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoQuantityPerParent { get; set; }

	[JsonProperty("dsoReturnParentQuantity", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoReturnParentQuantity { get; set; }

	[JsonProperty("dsoReturnQuantityShipped", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoReturnQuantityShipped { get; set; }

	[JsonProperty("dsoReverseDmrShipmentCompID", Order = 33)]
	public int dsoReverseDmrShipmentCompID { get; set; }

	[JsonProperty("dsoReverseDmrShipmentID", Order = 34)]
	[MaxLength(10)]
	public string dsoReverseDmrShipmentID { get; set; }

	[JsonProperty("dsoReverseDmrShipmentLineID", Order = 35)]
	public short dsoReverseDmrShipmentLineID { get; set; }

	[JsonProperty("dsoRowVersion", Order = 36)]
	public byte[] dsoRowVersion { get; set; }

	[JsonProperty("dsoDmrShipmentComponentID", Order = 37)]
	[Required(ErrorMessage = "dsoDmrShipmentComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int dsoDmrShipmentComponentID { get; set; }

	[JsonProperty("dsoUnitOfMeasure", Order = 38)]
	[MaxLength(2)]
	public string dsoUnitOfMeasure { get; set; }

	[JsonProperty("dsoWeight", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dsoWeight { get; set; }

	[JsonProperty("customFields", Order = 40)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
