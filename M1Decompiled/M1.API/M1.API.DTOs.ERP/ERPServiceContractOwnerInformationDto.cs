using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPServiceContractOwnerInformationDto
{
	public string kboAddressLine1 { get; set; }

	public string kboAddressLine2 { get; set; }

	public string kboAddressLine3 { get; set; }

	public string kboCity { get; set; }

	public string kboCountry { get; set; }

	public string kboCreatedBy { get; set; }

	public DateTime? kboCreatedDate { get; set; }

	public DateTime? kboDeliveryDate { get; set; }

	public string kboEmailAddress { get; set; }

	public Guid kboUniqueID { get; set; }

	public string kboFaxNumber { get; set; }

	public string kboFirstName { get; set; }

	public string kboHomePhoneNumber { get; set; }

	public bool kboCurrentOwner { get; set; }

	public bool kboSameAsAbove { get; set; }

	public bool kboTermsAccepted { get; set; }

	public string kboLastName { get; set; }

	public string kboMiddleName { get; set; }

	public string kboMobileNumber { get; set; }

	public string kboOrganizationID { get; set; }

	public string kboPhysicalAddressLine1 { get; set; }

	public string kboPhysicalAddressLine2 { get; set; }

	public string kboPhysicalAddressLine3 { get; set; }

	public string kboPhysicalCity { get; set; }

	public string kboPhysicalCountry { get; set; }

	public string kboPhysicalLocationCity { get; set; }

	public string kboPhysicalLocationState { get; set; }

	public string kboPhysicalPostCode { get; set; }

	public string kboPhysicalState { get; set; }

	public string kboPostCode { get; set; }

	public DateTime? kboRegisteredDate { get; set; }

	public byte[] kboRowVersion { get; set; }

	public short kboServiceContractOwnerID { get; set; }

	public string kboServiceContractID { get; set; }

	public DateTime? kboStartDate { get; set; }

	public string kboState { get; set; }

	public string kboWorkPhoneNumber { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
