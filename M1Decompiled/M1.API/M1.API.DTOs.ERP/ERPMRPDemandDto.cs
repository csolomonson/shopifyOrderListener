using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMRPDemandDto
{
	[JsonProperty("mrrCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string mrrCreatedBy { get; set; }

	[JsonProperty("mrrCreatedDate", Order = 2)]
	public DateTime? mrrCreatedDate { get; set; }

	[JsonProperty("mrrCustomerOrganizationID", Order = 3)]
	[MaxLength(10)]
	public string mrrCustomerOrganizationID { get; set; }

	[JsonProperty("mrrDemandID", Order = 4)]
	[Required(ErrorMessage = "mrrDemandID is required.")]
	public int mrrDemandID { get; set; }

	[JsonProperty("mrrDemandQuantity", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrrDemandQuantity { get; set; }

	[JsonProperty("mrrDueDate", Order = 6)]
	[Required(ErrorMessage = "mrrDueDate is required.")]
	public DateTime? mrrDueDate { get; set; }

	[JsonProperty("mrrUniqueID", Order = 7)]
	public Guid mrrUniqueID { get; set; }

	[JsonProperty("mrrJobAssemblyID", Order = 8)]
	public int mrrJobAssemblyID { get; set; }

	[JsonProperty("mrrJobID", Order = 9)]
	[MaxLength(20)]
	public string mrrJobID { get; set; }

	[JsonProperty("mrrJobMaterialID", Order = 10)]
	public int mrrJobMaterialID { get; set; }

	[JsonProperty("mrrLineID", Order = 11)]
	[Required(ErrorMessage = "mrrLineID is required.")]
	public int mrrLineID { get; set; }

	[JsonProperty("mrrOriginalQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrrOriginalQuantity { get; set; }

	[JsonProperty("mrrPartBinID", Order = 13)]
	[Required(ErrorMessage = "mrrPartBinID is required.")]
	[MaxLength(15)]
	public string mrrPartBinID { get; set; }

	[JsonProperty("mrrPartID", Order = 14)]
	[Required(ErrorMessage = "mrrPartID is required.")]
	[MaxLength(30)]
	public string mrrPartID { get; set; }

	[JsonProperty("mrrPartPlantID", Order = 15)]
	[MaxLength(5)]
	public string mrrPartPlantID { get; set; }

	[JsonProperty("mrrPartRevisionID", Order = 16)]
	[MaxLength(15)]
	public string mrrPartRevisionID { get; set; }

	[JsonProperty("mrrPartWarehouseLocationID", Order = 17)]
	[Required(ErrorMessage = "mrrPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string mrrPartWarehouseLocationID { get; set; }

	[JsonProperty("mrrQuantityReceived", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrrQuantityReceived { get; set; }

	[JsonProperty("mrrQuantityShipped", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrrQuantityShipped { get; set; }

	[JsonProperty("mrrRowVersion", Order = 20)]
	public byte[] mrrRowVersion { get; set; }

	[JsonProperty("mrrSalesOrderDeliveryID", Order = 21)]
	public short mrrSalesOrderDeliveryID { get; set; }

	[JsonProperty("mrrSalesOrderID", Order = 22)]
	[MaxLength(10)]
	public string mrrSalesOrderID { get; set; }

	[JsonProperty("mrrSalesOrderLineID", Order = 23)]
	public short mrrSalesOrderLineID { get; set; }

	[JsonProperty("mrrSessionID", Order = 24)]
	[Required(ErrorMessage = "mrrSessionID is required.")]
	[MaxLength(10)]
	public string mrrSessionID { get; set; }

	[JsonProperty("mrrShipLocationID", Order = 25)]
	[MaxLength(5)]
	public string mrrShipLocationID { get; set; }

	[JsonProperty("mrrShipOrganizationID", Order = 26)]
	[MaxLength(10)]
	public string mrrShipOrganizationID { get; set; }

	[JsonProperty("mrrSource", Order = 27)]
	[MaxLength(20)]
	public string mrrSource { get; set; }

	[JsonProperty("mrrType", Order = 28)]
	[MaxLength(20)]
	public string mrrType { get; set; }

	[JsonProperty("customFields", Order = 29)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
