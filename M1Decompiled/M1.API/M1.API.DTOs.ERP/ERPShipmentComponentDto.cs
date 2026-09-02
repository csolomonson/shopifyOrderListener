using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentComponentDto
{
	[JsonProperty("smoAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoAdditionalQuantity { get; set; }

	[JsonProperty("smoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string smoCreatedBy { get; set; }

	[JsonProperty("smoCreatedDate", Order = 3)]
	public DateTime? smoCreatedDate { get; set; }

	[JsonProperty("smoDescription", Order = 4)]
	[MaxLength(50)]
	public string smoDescription { get; set; }

	[JsonProperty("smoUniqueID", Order = 5)]
	public Guid smoUniqueID { get; set; }

	[JsonProperty("smoClosed", Order = 6)]
	public bool smoClosed { get; set; }

	[JsonProperty("smoPostedToGl", Order = 7)]
	public bool smoPostedToGl { get; set; }

	[JsonProperty("smoReversed", Order = 8)]
	public bool smoReversed { get; set; }

	[JsonProperty("smoShippedComplete", Order = 9)]
	public bool smoShippedComplete { get; set; }

	[JsonProperty("smoJobID", Order = 10)]
	[MaxLength(20)]
	public string smoJobID { get; set; }

	[JsonProperty("smoJobParentQuantity", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoJobParentQuantity { get; set; }

	[JsonProperty("smoJobQuantityShipped", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoJobQuantityShipped { get; set; }

	[JsonProperty("smoParentQuantity", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoParentQuantity { get; set; }

	[JsonProperty("smoPartBinID", Order = 14)]
	[Required(ErrorMessage = "smoPartBinID is required.")]
	[MaxLength(15)]
	public string smoPartBinID { get; set; }

	[JsonProperty("smoPartID", Order = 15)]
	[Required(ErrorMessage = "smoPartID is required.")]
	[MaxLength(30)]
	public string smoPartID { get; set; }

	[JsonProperty("smoPartRevisionID", Order = 16)]
	[MaxLength(15)]
	public string smoPartRevisionID { get; set; }

	[JsonProperty("smoPartWarehouseLocationID", Order = 17)]
	[Required(ErrorMessage = "smoPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string smoPartWarehouseLocationID { get; set; }

	[JsonProperty("smoQuantityPerParent", Order = 18)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoQuantityPerParent { get; set; }

	[JsonProperty("smoQuantityShipped", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoQuantityShipped { get; set; }

	[JsonProperty("smoReverseShipmentComponentID", Order = 20)]
	public short smoReverseShipmentComponentID { get; set; }

	[JsonProperty("smoReverseShipmentID", Order = 21)]
	[MaxLength(10)]
	public string smoReverseShipmentID { get; set; }

	[JsonProperty("smoReverseShipmentLineID", Order = 22)]
	public short smoReverseShipmentLineID { get; set; }

	[JsonProperty("smoRowVersion", Order = 23)]
	public byte[] smoRowVersion { get; set; }

	[JsonProperty("smoSalesOrderComponentID", Order = 24)]
	public short smoSalesOrderComponentID { get; set; }

	[JsonProperty("smoSalesOrderDeliveryID", Order = 25)]
	public short smoSalesOrderDeliveryID { get; set; }

	[JsonProperty("smoSalesOrderID", Order = 26)]
	[MaxLength(10)]
	public string smoSalesOrderID { get; set; }

	[JsonProperty("smoSalesOrderLineID", Order = 27)]
	public short smoSalesOrderLineID { get; set; }

	[JsonProperty("smoShipmentComponentID", Order = 28)]
	[Required(ErrorMessage = "smoShipmentComponentID is required.")]
	public short smoShipmentComponentID { get; set; }

	[JsonProperty("smoShipmentID", Order = 29)]
	[Required(ErrorMessage = "smoShipmentID is required.")]
	[MaxLength(10)]
	public string smoShipmentID { get; set; }

	[JsonProperty("smoShipmentLineID", Order = 30)]
	[Required(ErrorMessage = "smoShipmentLineID is required.")]
	public short smoShipmentLineID { get; set; }

	[JsonProperty("smoSourceTableName", Order = 31)]
	[MaxLength(30)]
	public string smoSourceTableName { get; set; }

	[JsonProperty("smoSourceTableUniqueID", Order = 32)]
	public Guid smoSourceTableUniqueID { get; set; }

	[JsonProperty("smoUnitOfMeasure", Order = 33)]
	[MaxLength(2)]
	public string smoUnitOfMeasure { get; set; }

	[JsonProperty("smoWeight", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smoWeight { get; set; }

	[JsonProperty("customFields", Order = 35)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
