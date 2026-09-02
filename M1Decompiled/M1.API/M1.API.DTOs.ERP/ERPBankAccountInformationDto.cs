using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPBankAccountInformationDto
{
	public string glnBankAccountName { get; set; }

	public string glnBankAccountNumber { get; set; }

	public string glnBankInitials { get; set; }

	public string glnBankName { get; set; }

	public string glnBic { get; set; }

	public string glnBsbNumber { get; set; }

	public string glnCanadianEftType { get; set; }

	public string glnCashGlAccountID { get; set; }

	public string glnBankAccountID { get; set; }

	public string glnCreatedBy { get; set; }

	public DateTime? glnCreatedDate { get; set; }

	public string glnCurrencyRateID { get; set; }

	public decimal glnDataCenterCode { get; set; }

	public string glnDescription { get; set; }

	public string glnDirectEntryUserID { get; set; }

	public string glnDirectEntryUserName { get; set; }

	public string glnEftApDescription { get; set; }

	public string glnEftCompanyID { get; set; }

	public string glnEftCompanyName { get; set; }

	public string glnEftDiscretionaryData { get; set; }

	public string glnEftFileID { get; set; }

	public string glnEftFileIDModifier { get; set; }

	public string glnEftFileLocation { get; set; }

	public string glnEftPayrollDescription { get; set; }

	public string glnEftReferenceCode { get; set; }

	public Guid glnUniqueID { get; set; }

	public short glnFileCreationNumber { get; set; }

	public string glnIban { get; set; }

	public DateTime? glnInactiveDate { get; set; }

	public bool glnAChFormat { get; set; }

	public bool glnInactive { get; set; }

	public bool glnEftCreateOffsettingDebit { get; set; }

	public bool glnPayrollOnly { get; set; }

	public string glnLanguageCode { get; set; }

	public int glnNextEftNumber { get; set; }

	public int glnNextPaymentNumber { get; set; }

	public string glnNZEftType { get; set; }

	public string glnOrganizationID { get; set; }

	public byte[] glnRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
