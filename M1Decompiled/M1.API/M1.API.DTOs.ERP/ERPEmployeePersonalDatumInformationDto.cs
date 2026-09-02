using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeePersonalDatumInformationDto
{
	public string lmdAddressLine1 { get; set; }

	public string lmdAddressLine2 { get; set; }

	public string lmdAddressLine3 { get; set; }

	public string lmdBasisOfPayment { get; set; }

	public DateTime? lmdBirthDate { get; set; }

	public string lmdCity { get; set; }

	public string lmdContact1HomePhoneNumber { get; set; }

	public string lmdContact1MobilePhoneNumber { get; set; }

	public string lmdContact1Name { get; set; }

	public string lmdContact1Relationship { get; set; }

	public string lmdContact1WorkPhoneNumber { get; set; }

	public string lmdContact2HomePhoneNumber { get; set; }

	public string lmdContact2MobilePhoneNumber { get; set; }

	public string lmdContact2Name { get; set; }

	public string lmdContact2Relationship { get; set; }

	public string lmdContact2WorkPhoneNumber { get; set; }

	public string lmdCountry { get; set; }

	public string lmdCreatedBy { get; set; }

	public DateTime? lmdCreatedDate { get; set; }

	public string lmdEmployeeFirstName { get; set; }

	public string lmdEmployeeID { get; set; }

	public string lmdEmployeeLastName { get; set; }

	public string lmdEmployeeMiddleName { get; set; }

	public DateTime? lmdEmploymentDeclarationDate { get; set; }

	public string lmdEmploymentStatus { get; set; }

	public Guid lmdUniqueID { get; set; }

	public string lmdFaxNumber { get; set; }

	public string lmdGender { get; set; }

	public string lmdHomeCountry { get; set; }

	public bool lmdEmploymentDeclarationOnFile { get; set; }

	public bool lmdPayrollEmployee { get; set; }

	public bool lmdStdntFinSupplSchemeLoan { get; set; }

	public bool lmdStudyTrainLoanRepayment { get; set; }

	public bool lmdTaxFreeThresholdClaimed { get; set; }

	public bool lmdWorkingHolidayMaker { get; set; }

	public decimal lmdLaborRate { get; set; }

	public string lmdMaritalStatus { get; set; }

	public string lmdMobileNumber { get; set; }

	public string lmdNZTaxCode { get; set; }

	public string lmdPAYGSummaryType { get; set; }

	public string lmdPayrollDefinitionID { get; set; }

	public string lmdPayrollExportEmployeeID { get; set; }

	public string lmdPersonalEmailAddress { get; set; }

	public string lmdPhoneNumber { get; set; }

	public string lmdPostCode { get; set; }

	public string lmdResidencyStatus { get; set; }

	public byte[] lmdRowVersion { get; set; }

	public string lmdState { get; set; }

	public string lmdStateAus { get; set; }

	public string lmdTaxFileNumber { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
