using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARPaymentSessionInformationDto
{
	public string arsApDiscountGlAccountID { get; set; }

	public string arsApGlAccountID { get; set; }

	public string arsArGlAccountID { get; set; }

	public string arsBankAccountID { get; set; }

	public string arsCashGlAccountID { get; set; }

	public string arsCreatedBy { get; set; }

	public DateTime? arsCreatedDate { get; set; }

	public string arsCurrencyRateID { get; set; }

	public decimal arsDepositAmount { get; set; }

	public decimal arsDepositAmountForeign { get; set; }

	public string arsDiscountGlAccountID { get; set; }

	public Guid arsUniqueID { get; set; }

	public decimal arsExchangeRate { get; set; }

	public short arsGlFiscalYearID { get; set; }

	public byte arsGlFiscalYearPeriodID { get; set; }

	public bool arsAvalaraTaxCalculated { get; set; }

	public bool arsCustomRate { get; set; }

	public bool arsGroupBySettlement { get; set; }

	public bool arsOpenPaymentLoad { get; set; }

	public bool arsPostedToGl { get; set; }

	public string arsPlantDepartmentID { get; set; }

	public string arsPlantID { get; set; }

	public DateTime? arsPostedDate { get; set; }

	public DateTime? arsReceiptDate { get; set; }

	public byte[] arsRowVersion { get; set; }

	public int arsArPaymentSessionID { get; set; }

	public DateTime? arsSettlementEndTime { get; set; }

	public DateTime? arsSettlementStartTime { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
