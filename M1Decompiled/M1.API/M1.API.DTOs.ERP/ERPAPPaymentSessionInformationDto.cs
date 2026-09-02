using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPPaymentSessionInformationDto
{
	public string apsApGlAccountID { get; set; }

	public string apsArGlAccountID { get; set; }

	public string apsBankAccountID { get; set; }

	public string apsCashGlAccountID { get; set; }

	public DateTime? apsCompletedDate { get; set; }

	public string apsCreatedBy { get; set; }

	public DateTime? apsCreatedDate { get; set; }

	public string apsCurrencyRateID { get; set; }

	public string apsEftDescription { get; set; }

	public string apsEftReferenceNumber { get; set; }

	public DateTime? apsEftSettlementDate { get; set; }

	public Guid apsUniqueID { get; set; }

	public decimal apsExchangeRate { get; set; }

	public short apsGlFiscalYearID { get; set; }

	public byte apsGlFiscalYearPeriodID { get; set; }

	public bool apsCompleted { get; set; }

	public bool apsCustomRate { get; set; }

	public bool apsOpenPaymentLoad { get; set; }

	public bool apsPaymentsPrinted { get; set; }

	public bool apsPostedToGl { get; set; }

	public decimal apsPaymentAmount { get; set; }

	public decimal apsPaymentAmountForeign { get; set; }

	public DateTime? apsPaymentDate { get; set; }

	public string apsPlantDepartmentID { get; set; }

	public string apsPlantID { get; set; }

	public DateTime? apsPostedDate { get; set; }

	public byte[] apsRowVersion { get; set; }

	public int apsApPaymentSessionID { get; set; }

	public byte apsSessionType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
