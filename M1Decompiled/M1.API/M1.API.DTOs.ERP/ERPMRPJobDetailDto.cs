using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMRPJobDetailDto
{
	[JsonProperty("mrjCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string mrjCreatedBy { get; set; }

	[JsonProperty("mrjCreatedDate", Order = 2)]
	public DateTime? mrjCreatedDate { get; set; }

	[JsonProperty("mrjCustomerOrganizationID", Order = 3)]
	[MaxLength(10)]
	public string mrjCustomerOrganizationID { get; set; }

	[JsonProperty("mrjUniqueID", Order = 4)]
	public Guid mrjUniqueID { get; set; }

	[JsonProperty("mrjInventoryQuantity", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrjInventoryQuantity { get; set; }

	[JsonProperty("mrjCompleted", Order = 6)]
	public bool mrjCompleted { get; set; }

	[JsonProperty("mrjConsolidated", Order = 7)]
	public bool mrjConsolidated { get; set; }

	[JsonProperty("mrjDataMissing", Order = 8)]
	public bool mrjDataMissing { get; set; }

	[JsonProperty("mrjDirectLink", Order = 9)]
	public bool mrjDirectLink { get; set; }

	[JsonProperty("mrjExistingJob", Order = 10)]
	public bool mrjExistingJob { get; set; }

	[JsonProperty("mrjFirm", Order = 11)]
	public bool mrjFirm { get; set; }

	[JsonProperty("mrjGetPartMethod", Order = 12)]
	public bool mrjGetPartMethod { get; set; }

	[JsonProperty("mrjIndirectLink", Order = 13)]
	public bool mrjIndirectLink { get; set; }

	[JsonProperty("mrjJobAssemblyID", Order = 14)]
	public int mrjJobAssemblyID { get; set; }

	[JsonProperty("mrjJobDetailID", Order = 15)]
	[Required(ErrorMessage = "mrjJobDetailID is required.")]
	public int mrjJobDetailID { get; set; }

	[JsonProperty("mrjJobID", Order = 16)]
	[MaxLength(20)]
	public string mrjJobID { get; set; }

	[JsonProperty("mrjLineID", Order = 17)]
	[Required(ErrorMessage = "mrjLineID is required.")]
	public int mrjLineID { get; set; }

	[JsonProperty("mrjOrderQuantity", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrjOrderQuantity { get; set; }

	[JsonProperty("mrjPartBinID", Order = 19)]
	[Required(ErrorMessage = "mrjPartBinID is required.")]
	[MaxLength(15)]
	public string mrjPartBinID { get; set; }

	[JsonProperty("mrjPartID", Order = 20)]
	[Required(ErrorMessage = "mrjPartID is required.")]
	[MaxLength(30)]
	public string mrjPartID { get; set; }

	[JsonProperty("mrjPartPlantID", Order = 21)]
	[MaxLength(5)]
	public string mrjPartPlantID { get; set; }

	[JsonProperty("mrjPartRevisionID", Order = 22)]
	[MaxLength(15)]
	public string mrjPartRevisionID { get; set; }

	[JsonProperty("mrjPartWarehouseLocationID", Order = 23)]
	[Required(ErrorMessage = "mrjPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string mrjPartWarehouseLocationID { get; set; }

	[JsonProperty("mrjProductionDueDate", Order = 24)]
	[Required(ErrorMessage = "mrjProductionDueDate is required.")]
	public DateTime? mrjProductionDueDate { get; set; }

	[JsonProperty("mrjRowVersion", Order = 25)]
	public byte[] mrjRowVersion { get; set; }

	[JsonProperty("mrjSalesOrderDeliveryID", Order = 26)]
	public short mrjSalesOrderDeliveryID { get; set; }

	[JsonProperty("mrjSalesOrderID", Order = 27)]
	[MaxLength(10)]
	public string mrjSalesOrderID { get; set; }

	[JsonProperty("mrjSalesOrderLineID", Order = 28)]
	public short mrjSalesOrderLineID { get; set; }

	[JsonProperty("mrjSessionID", Order = 29)]
	[Required(ErrorMessage = "mrjSessionID is required.")]
	[MaxLength(10)]
	public string mrjSessionID { get; set; }

	[JsonProperty("mrjShipLocationID", Order = 30)]
	[MaxLength(5)]
	public string mrjShipLocationID { get; set; }

	[JsonProperty("mrjShipOrganizationID", Order = 31)]
	[MaxLength(10)]
	public string mrjShipOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 32)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
