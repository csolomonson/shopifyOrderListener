using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseRequisitionLineDto
{
	[JsonProperty("wqlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string wqlCreatedBy { get; set; }

	[JsonProperty("wqlCreatedDate", Order = 2)]
	public DateTime? wqlCreatedDate { get; set; }

	[JsonProperty("wqlUniqueID", Order = 3)]
	public Guid wqlUniqueID { get; set; }

	[JsonProperty("wqlClosed", Order = 4)]
	public bool wqlClosed { get; set; }

	[JsonProperty("wqlKitPart", Order = 5)]
	public bool wqlKitPart { get; set; }

	[JsonProperty("wqlTransferredComplete", Order = 6)]
	public bool wqlTransferredComplete { get; set; }

	[JsonProperty("wqlPartDescription", Order = 7)]
	[Required(ErrorMessage = "wqlPartDescription is required.")]
	[MaxLength(50)]
	public string wqlPartDescription { get; set; }

	[JsonProperty("wqlPartID", Order = 8)]
	[Required(ErrorMessage = "wqlPartID is required.")]
	[MaxLength(30)]
	public string wqlPartID { get; set; }

	[JsonProperty("wqlPartRevisionID", Order = 9)]
	[MaxLength(15)]
	public string wqlPartRevisionID { get; set; }

	[JsonProperty("wqlQuantityTransferred", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqlQuantityTransferred { get; set; }

	[JsonProperty("wqlRequestedQuantity", Order = 11)]
	[Required(ErrorMessage = "wqlRequestedQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wqlRequestedQuantity { get; set; }

	[JsonProperty("wqlRowVersion", Order = 12)]
	public byte[] wqlRowVersion { get; set; }

	[JsonProperty("wqlWarehouseRequisitionLineID", Order = 13)]
	[Required(ErrorMessage = "wqlWarehouseRequisitionLineID is required.")]
	public short wqlWarehouseRequisitionLineID { get; set; }

	[JsonProperty("wqlSourceWarehouseID", Order = 14)]
	[MaxLength(5)]
	public string wqlSourceWarehouseID { get; set; }

	[JsonProperty("wqlUnitOfMeasure", Order = 15)]
	[MaxLength(2)]
	public string wqlUnitOfMeasure { get; set; }

	[JsonProperty("wqlWarehouseRequisitionID", Order = 16)]
	[Required(ErrorMessage = "wqlWarehouseRequisitionID is required.")]
	[MaxLength(10)]
	public string wqlWarehouseRequisitionID { get; set; }

	[JsonProperty("customFields", Order = 17)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
