using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeePersonalDatumDto
{
	[JsonProperty("lmdAddressLine1", Order = 1)]
	[MaxLength(50)]
	public string lmdAddressLine1 { get; set; }

	[JsonProperty("lmdAddressLine2", Order = 2)]
	[MaxLength(50)]
	public string lmdAddressLine2 { get; set; }

	[JsonProperty("lmdAddressLine3", Order = 3)]
	[MaxLength(50)]
	public string lmdAddressLine3 { get; set; }

	[JsonProperty("lmdBasisOfPayment", Order = 4)]
	[MaxLength(1)]
	public string lmdBasisOfPayment { get; set; }

	[JsonProperty("lmdBirthDate", Order = 5)]
	public DateTime? lmdBirthDate { get; set; }

	[JsonProperty("lmdCity", Order = 6)]
	[MaxLength(30)]
	public string lmdCity { get; set; }

	[JsonProperty("lmdContact1HomePhoneNumber", Order = 7)]
	[MaxLength(20)]
	public string lmdContact1HomePhoneNumber { get; set; }

	[JsonProperty("lmdContact1MobilePhoneNumber", Order = 8)]
	[MaxLength(20)]
	public string lmdContact1MobilePhoneNumber { get; set; }

	[JsonProperty("lmdContact1Name", Order = 9)]
	[MaxLength(50)]
	public string lmdContact1Name { get; set; }

	[JsonProperty("lmdContact1Relationship", Order = 10)]
	[MaxLength(10)]
	public string lmdContact1Relationship { get; set; }

	[JsonProperty("lmdContact1WorkPhoneNumber", Order = 11)]
	[MaxLength(20)]
	public string lmdContact1WorkPhoneNumber { get; set; }

	[JsonProperty("lmdContact2HomePhoneNumber", Order = 12)]
	[MaxLength(20)]
	public string lmdContact2HomePhoneNumber { get; set; }

	[JsonProperty("lmdContact2MobilePhoneNumber", Order = 13)]
	[MaxLength(20)]
	public string lmdContact2MobilePhoneNumber { get; set; }

	[JsonProperty("lmdContact2Name", Order = 14)]
	[MaxLength(50)]
	public string lmdContact2Name { get; set; }

	[JsonProperty("lmdContact2Relationship", Order = 15)]
	[MaxLength(10)]
	public string lmdContact2Relationship { get; set; }

	[JsonProperty("lmdContact2WorkPhoneNumber", Order = 16)]
	[MaxLength(20)]
	public string lmdContact2WorkPhoneNumber { get; set; }

	[JsonProperty("lmdCountry", Order = 17)]
	[MaxLength(20)]
	public string lmdCountry { get; set; }

	[JsonProperty("lmdCreatedBy", Order = 18)]
	[MaxLength(20)]
	public string lmdCreatedBy { get; set; }

	[JsonProperty("lmdCreatedDate", Order = 19)]
	public DateTime? lmdCreatedDate { get; set; }

	[JsonProperty("lmdEmployeeFirstName", Order = 20)]
	[MaxLength(20)]
	public string lmdEmployeeFirstName { get; set; }

	[JsonProperty("lmdEmployeeID", Order = 21)]
	[Required(ErrorMessage = "lmdEmployeeID is required.")]
	[MaxLength(10)]
	public string lmdEmployeeID { get; set; }

	[JsonProperty("lmdEmployeeLastName", Order = 22)]
	[MaxLength(20)]
	public string lmdEmployeeLastName { get; set; }

	[JsonProperty("lmdEmployeeMiddleName", Order = 23)]
	[MaxLength(20)]
	public string lmdEmployeeMiddleName { get; set; }

	[JsonProperty("lmdEmploymentDeclarationDate", Order = 24)]
	public DateTime? lmdEmploymentDeclarationDate { get; set; }

	[JsonProperty("lmdEmploymentStatus", Order = 25)]
	[MaxLength(10)]
	public string lmdEmploymentStatus { get; set; }

	[JsonProperty("lmdUniqueID", Order = 26)]
	public Guid lmdUniqueID { get; set; }

	[JsonProperty("lmdFaxNumber", Order = 27)]
	[MaxLength(20)]
	public string lmdFaxNumber { get; set; }

	[JsonProperty("lmdGender", Order = 28)]
	[MaxLength(1)]
	public string lmdGender { get; set; }

	[JsonProperty("lmdHomeCountry", Order = 29)]
	[MaxLength(2)]
	public string lmdHomeCountry { get; set; }

	[JsonProperty("lmdEmploymentDeclarationOnFile", Order = 30)]
	public bool lmdEmploymentDeclarationOnFile { get; set; }

	[JsonProperty("lmdPayrollEmployee", Order = 31)]
	public bool lmdPayrollEmployee { get; set; }

	[JsonProperty("lmdStdntFinSupplSchemeLoan", Order = 32)]
	public bool lmdStdntFinSupplSchemeLoan { get; set; }

	[JsonProperty("lmdStudyTrainLoanRepayment", Order = 33)]
	public bool lmdStudyTrainLoanRepayment { get; set; }

	[JsonProperty("lmdTaxFreeThresholdClaimed", Order = 34)]
	public bool lmdTaxFreeThresholdClaimed { get; set; }

	[JsonProperty("lmdWorkingHolidayMaker", Order = 35)]
	public bool lmdWorkingHolidayMaker { get; set; }

	[JsonProperty("lmdLaborRate", Order = 36)]
	[Range(0.0, 9999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmdLaborRate { get; set; }

	[JsonProperty("lmdMaritalStatus", Order = 37)]
	[MaxLength(1)]
	public string lmdMaritalStatus { get; set; }

	[JsonProperty("lmdMobileNumber", Order = 38)]
	[MaxLength(20)]
	public string lmdMobileNumber { get; set; }

	[JsonProperty("lmdNZTaxCode", Order = 39)]
	[MaxLength(5)]
	public string lmdNZTaxCode { get; set; }

	[JsonProperty("lmdPAYGSummaryType", Order = 40)]
	[MaxLength(1)]
	public string lmdPAYGSummaryType { get; set; }

	[JsonProperty("lmdPayrollDefinitionID", Order = 41)]
	[MaxLength(5)]
	public string lmdPayrollDefinitionID { get; set; }

	[JsonProperty("lmdPayrollExportEmployeeID", Order = 42)]
	[MaxLength(8)]
	public string lmdPayrollExportEmployeeID { get; set; }

	[JsonProperty("lmdPersonalEmailAddress", Order = 43)]
	[MaxLength(50)]
	public string lmdPersonalEmailAddress { get; set; }

	[JsonProperty("lmdPhoneNumber", Order = 44)]
	[MaxLength(20)]
	public string lmdPhoneNumber { get; set; }

	[JsonProperty("lmdPostCode", Order = 45)]
	[MaxLength(10)]
	public string lmdPostCode { get; set; }

	[JsonProperty("lmdResidencyStatus", Order = 46)]
	[MaxLength(25)]
	public string lmdResidencyStatus { get; set; }

	[JsonProperty("lmdRowVersion", Order = 47)]
	public byte[] lmdRowVersion { get; set; }

	[JsonProperty("lmdState", Order = 48)]
	[MaxLength(3)]
	public string lmdState { get; set; }

	[JsonProperty("lmdStateAus", Order = 49)]
	[MaxLength(3)]
	public string lmdStateAus { get; set; }

	[JsonProperty("lmdTaxFileNumber", Order = 50)]
	[MaxLength(11)]
	public string lmdTaxFileNumber { get; set; }

	[JsonProperty("customFields", Order = 51)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
