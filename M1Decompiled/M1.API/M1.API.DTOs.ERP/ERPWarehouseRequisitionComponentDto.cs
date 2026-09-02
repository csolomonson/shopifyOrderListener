using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseRequisitionComponentDto
{
	[JsonProperty("wqoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqoAdditionalQuantity { get; set; }

	[JsonProperty("wqoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string wqoCreatedBy { get; set; }

	[JsonProperty("wqoCreatedDate", Order = 3)]
	public DateTime? wqoCreatedDate { get; set; }

	[JsonProperty("wqoDescription", Order = 4)]
	[Required(ErrorMessage = "wqoDescription is required.")]
	[MaxLength(50)]
	public string wqoDescription { get; set; }

	[JsonProperty("wqoUniqueID", Order = 5)]
	public Guid wqoUniqueID { get; set; }

	[JsonProperty("wqoClosed", Order = 6)]
	public bool wqoClosed { get; set; }

	[JsonProperty("wqoTransferredComplete", Order = 7)]
	public bool wqoTransferredComplete { get; set; }

	[JsonProperty("wqoParentQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqoParentQuantity { get; set; }

	[JsonProperty("wqoPartID", Order = 9)]
	[Required(ErrorMessage = "wqoPartID is required.")]
	[MaxLength(30)]
	public string wqoPartID { get; set; }

	[JsonProperty("wqoPartRevisionID", Order = 10)]
	[MaxLength(15)]
	public string wqoPartRevisionID { get; set; }

	[JsonProperty("wqoQuantityPerParent", Order = 11)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqoQuantityPerParent { get; set; }

	[JsonProperty("wqoQuantityRequested", Order = 12)]
	[Required(ErrorMessage = "wqoQuantityRequested is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqoQuantityRequested { get; set; }

	[JsonProperty("wqoQuantityTransferred", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqoQuantityTransferred { get; set; }

	[JsonProperty("wqoRowVersion", Order = 14)]
	public byte[] wqoRowVersion { get; set; }

	[JsonProperty("wqoSourceWarehouseID", Order = 15)]
	[MaxLength(5)]
	public string wqoSourceWarehouseID { get; set; }

	[JsonProperty("wqoUnitOfMeasure", Order = 16)]
	[MaxLength(2)]
	public string wqoUnitOfMeasure { get; set; }

	[JsonProperty("wqoWarehouseReqComponentID", Order = 17)]
	[Required(ErrorMessage = "wqoWarehouseReqComponentID is required.")]
	public short wqoWarehouseReqComponentID { get; set; }

	[JsonProperty("wqoWarehouseRequisitionID", Order = 18)]
	[Required(ErrorMessage = "wqoWarehouseRequisitionID is required.")]
	[MaxLength(10)]
	public string wqoWarehouseRequisitionID { get; set; }

	[JsonProperty("wqoWarehouseRequisitionLineID", Order = 19)]
	[Required(ErrorMessage = "wqoWarehouseRequisitionLineID is required.")]
	public short wqoWarehouseRequisitionLineID { get; set; }

	[JsonProperty("wqoWeight", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqoWeight { get; set; }

	[JsonProperty("customFields", Order = 21)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
