using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPlantDepartmentInformationDto
{
	public string xavApApGlAccountID { get; set; }

	public string xavApBankAccountID { get; set; }

	public string xavApCashGlAccountID { get; set; }

	public string xavApDiscountGlAccountID { get; set; }

	public string xavApFreightGlAccountID { get; set; }

	public string xavArArGlAccountID { get; set; }

	public string xavArBankAccountID { get; set; }

	public string xavArCashGlAccountID { get; set; }

	public string xavArDepositGlAccountID { get; set; }

	public string xavArDiscountGlAccountID { get; set; }

	public string xavArFreightGlAccountID { get; set; }

	public string xavArSalesGlAccountID { get; set; }

	public string xavPlantDepartmentID { get; set; }

	public string xavCreatedBy { get; set; }

	public DateTime? xavCreatedDate { get; set; }

	public Guid xavUniqueID { get; set; }

	public DateTime? xavEstablishedDate { get; set; }

	public DateTime? xavInactiveDate { get; set; }

	public bool xavInactive { get; set; }

	public bool xavUseProperties { get; set; }

	public string xavName { get; set; }

	public string xavPlantID { get; set; }

	public byte[] xavRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
