using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFreightPackageRateDto
{
	[JsonProperty("fprCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string fprCreatedBy { get; set; }

	[JsonProperty("fprCreatedDate", Order = 2)]
	public DateTime? fprCreatedDate { get; set; }

	[JsonProperty("fprUniqueID", Order = 3)]
	public Guid fprUniqueID { get; set; }

	[JsonProperty("fprFdxBaseCharge", Order = 4)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxBaseCharge { get; set; }

	[JsonProperty("fprFdxCurrency", Order = 5)]
	[MaxLength(3)]
	public string fprFdxCurrency { get; set; }

	[JsonProperty("fprFdxDeliveryDate", Order = 6)]
	public DateTime? fprFdxDeliveryDate { get; set; }

	[JsonProperty("fprFdxDeliveryDay", Order = 7)]
	[MaxLength(3)]
	public string fprFdxDeliveryDay { get; set; }

	[JsonProperty("fprFdxDestinationStationID", Order = 8)]
	[MaxLength(5)]
	public string fprFdxDestinationStationID { get; set; }

	[JsonProperty("fprFdxPackageBaseCharge", Order = 9)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageBaseCharge { get; set; }

	[JsonProperty("fprFdxPackageBillingWeight", Order = 10)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageBillingWeight { get; set; }

	[JsonProperty("fprFdxPackageDimWeight", Order = 11)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageDimWeight { get; set; }

	[JsonProperty("fprFdxPackageFreightDiscount", Order = 12)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageFreightDiscount { get; set; }

	[JsonProperty("fprFdxPackageNetCharge", Order = 13)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageNetCharge { get; set; }

	[JsonProperty("fprFdxPackageNetFreight", Order = 14)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageNetFreight { get; set; }

	[JsonProperty("fprFdxPackageSurcharges", Order = 15)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxPackageSurcharges { get; set; }

	[JsonProperty("fprFdxPackaging", Order = 16)]
	[MaxLength(35)]
	public string fprFdxPackaging { get; set; }

	[JsonProperty("fprFdxService", Order = 17)]
	[MaxLength(35)]
	public string fprFdxService { get; set; }

	[JsonProperty("fprFdxTimeInTransit", Order = 18)]
	public short fprFdxTimeInTransit { get; set; }

	[JsonProperty("fprFdxTotalBillingWeight", Order = 19)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalBillingWeight { get; set; }

	[JsonProperty("fprFdxTotalCustomerCharge", Order = 20)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalCustomerCharge { get; set; }

	[JsonProperty("fprFdxTotalDimWeight", Order = 21)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalDimWeight { get; set; }

	[JsonProperty("fprFdxTotalFreightDiscount", Order = 22)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalFreightDiscount { get; set; }

	[JsonProperty("fprFdxTotalNetCharge", Order = 23)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalNetCharge { get; set; }

	[JsonProperty("fprFdxTotalNetFreightCharge", Order = 24)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalNetFreightCharge { get; set; }

	[JsonProperty("fprFdxTotalSurcharges", Order = 25)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxTotalSurcharges { get; set; }

	[JsonProperty("fprFdxUnits", Order = 26)]
	[MaxLength(3)]
	public string fprFdxUnits { get; set; }

	[JsonProperty("fprFdxVariableHandlingCharge", Order = 27)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fprFdxVariableHandlingCharge { get; set; }

	[JsonProperty("fprFreightPackageID", Order = 28)]
	public short fprFreightPackageID { get; set; }

	[JsonProperty("fprFreightShipmentID", Order = 29)]
	[MaxLength(10)]
	public string fprFreightShipmentID { get; set; }

	[JsonProperty("fprRCTI", Order = 30)]
	[MaxLength(40)]
	public string fprRCTI { get; set; }

	[JsonProperty("fprRowVersion", Order = 31)]
	public byte[] fprRowVersion { get; set; }

	[JsonProperty("fprFreightPackageRateID", Order = 32)]
	public short fprFreightPackageRateID { get; set; }

	[JsonProperty("customFields", Order = 33)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
