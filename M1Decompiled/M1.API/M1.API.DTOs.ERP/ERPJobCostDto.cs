using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobCostDto
{
	[JsonProperty("jmcApInvoiceID", Order = 1)]
	[Required(ErrorMessage = "jmcApInvoiceID is required.")]
	[MaxLength(10)]
	public string jmcApInvoiceID { get; set; }

	[JsonProperty("jmcApInvoiceLineID", Order = 2)]
	public short jmcApInvoiceLineID { get; set; }

	[JsonProperty("jmcCostSequence", Order = 3)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int jmcCostSequence { get; set; }

	[JsonProperty("jmcCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string jmcCreatedBy { get; set; }

	[JsonProperty("jmcCreatedDate", Order = 5)]
	public DateTime? jmcCreatedDate { get; set; }

	[JsonProperty("jmcUniqueID", Order = 6)]
	public Guid jmcUniqueID { get; set; }

	[JsonProperty("jmcHeatLot", Order = 7)]
	[MaxLength(50)]
	public string jmcHeatLot { get; set; }

	[JsonProperty("jmcJobAssemblyID", Order = 8)]
	public int jmcJobAssemblyID { get; set; }

	[JsonProperty("jmcJobID", Order = 9)]
	[Required(ErrorMessage = "jmcJobID is required.")]
	[MaxLength(20)]
	public string jmcJobID { get; set; }

	[JsonProperty("jmcJobMaterialComponentID", Order = 10)]
	public int jmcJobMaterialComponentID { get; set; }

	[JsonProperty("jmcJobMaterialID", Order = 11)]
	public int jmcJobMaterialID { get; set; }

	[JsonProperty("jmcJobOperationID", Order = 12)]
	public int jmcJobOperationID { get; set; }

	[JsonProperty("jmcJobSequence", Order = 13)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int jmcJobSequence { get; set; }

	[JsonProperty("jmcJobType", Order = 14)]
	public byte jmcJobType { get; set; }

	[JsonProperty("jmcPartDescription", Order = 15)]
	[MaxLength(50)]
	public string jmcPartDescription { get; set; }

	[JsonProperty("jmcPartID", Order = 16)]
	[Required(ErrorMessage = "jmcPartID is required.")]
	[MaxLength(30)]
	public string jmcPartID { get; set; }

	[JsonProperty("jmcPartRevisionID", Order = 17)]
	[MaxLength(15)]
	public string jmcPartRevisionID { get; set; }

	[JsonProperty("jmcQuantityReceived", Order = 18)]
	[Required(ErrorMessage = "jmcQuantityReceived is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmcQuantityReceived { get; set; }

	[JsonProperty("jmcReceiptComponentID", Order = 19)]
	public short jmcReceiptComponentID { get; set; }

	[JsonProperty("jmcReceiptID", Order = 20)]
	[MaxLength(10)]
	public string jmcReceiptID { get; set; }

	[JsonProperty("jmcReceiptLineID", Order = 21)]
	public short jmcReceiptLineID { get; set; }

	[JsonProperty("jmcReceivedUnitOfMeasure", Order = 22)]
	[MaxLength(2)]
	public string jmcReceivedUnitOfMeasure { get; set; }

	[JsonProperty("jmcReference", Order = 23)]
	[MaxLength(30)]
	public string jmcReference { get; set; }

	[JsonProperty("jmcRowVersion", Order = 24)]
	public byte[] jmcRowVersion { get; set; }

	[JsonProperty("jmcSource", Order = 25)]
	public byte jmcSource { get; set; }

	[JsonProperty("jmcSupplierOrganizationID", Order = 26)]
	[MaxLength(10)]
	public string jmcSupplierOrganizationID { get; set; }

	[JsonProperty("jmcTotalCogsCost", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmcTotalCogsCost { get; set; }

	[JsonProperty("jmcTotalCost", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmcTotalCost { get; set; }

	[JsonProperty("jmcTransactionDate", Order = 29)]
	[Required(ErrorMessage = "jmcTransactionDate is required.")]
	public DateTime? jmcTransactionDate { get; set; }

	[JsonProperty("customFields", Order = 30)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
