using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPlantInformationDto
{
	public string xauAccruedCreditorsGlAccountID { get; set; }

	public string xauAddressLine1 { get; set; }

	public string xauAddressLine2 { get; set; }

	public string xauAddressLine3 { get; set; }

	public string xauApApGlAccountID { get; set; }

	public string xauApBankAccountID { get; set; }

	public string xauApCashGlAccountID { get; set; }

	public string xauApDiscountGlAccountID { get; set; }

	public string xauApFreightGlAccountID { get; set; }

	public string xauArArGlAccountID { get; set; }

	public string xauArBankAccountID { get; set; }

	public string xauArCashGlAccountID { get; set; }

	public string xauArDepositGlAccountID { get; set; }

	public string xauArDiscountGlAccountID { get; set; }

	public string xauArFreightGlAccountID { get; set; }

	public string xauArSalesGlAccountID { get; set; }

	public string xauCity { get; set; }

	public string xauPlantID { get; set; }

	public string xauCountry { get; set; }

	public string xauCountryCode { get; set; }

	public string xauCreatedBy { get; set; }

	public DateTime? xauCreatedDate { get; set; }

	public decimal xauDayStartTimeFri { get; set; }

	public decimal xauDayStartTimeMon { get; set; }

	public decimal xauDayStartTimeSat { get; set; }

	public decimal xauDayStartTimeSun { get; set; }

	public decimal xauDayStartTimeThu { get; set; }

	public decimal xauDayStartTimeTue { get; set; }

	public decimal xauDayStartTimeWed { get; set; }

	public string xauEmailAddress { get; set; }

	public Guid xauUniqueID { get; set; }

	public DateTime? xauEstablishedDate { get; set; }

	public string xauFaxNumber { get; set; }

	public string xauFederalID { get; set; }

	public decimal xauHoursFri { get; set; }

	public decimal xauHoursMon { get; set; }

	public decimal xauHoursSat { get; set; }

	public decimal xauHoursSun { get; set; }

	public decimal xauHoursThu { get; set; }

	public decimal xauHoursTue { get; set; }

	public decimal xauHoursWed { get; set; }

	public DateTime? xauInactiveDate { get; set; }

	public bool xauInactive { get; set; }

	public bool xauAvalaraAddressValidated { get; set; }

	public bool xauUseProperties { get; set; }

	public string xauLaborClearingGlAccountID { get; set; }

	public string xauName { get; set; }

	public string xauOverheadClearingGlAccountID { get; set; }

	public string xauPhoneNumber { get; set; }

	public string xauPostCode { get; set; }

	public string xauPurchaseVarianceGlAccountID { get; set; }

	public byte[] xauRowVersion { get; set; }

	public string xauShipAwaitInvoiceGlAccountID { get; set; }

	public string xauState { get; set; }

	public string xauStockInTransitGlAccountID { get; set; }

	public string xauStockRevaluationGlAccountID { get; set; }

	public string xauSVarLaborGlAccountID { get; set; }

	public string xauSVarMaterialGlAccountID { get; set; }

	public string xauSVarOverheadGlAccountID { get; set; }

	public string xauSVarSubcontractGlAccountID { get; set; }

	public string xauWipLaborGlAccountID { get; set; }

	public string xauWipMaterialGlAccountID { get; set; }

	public string xauWipoverheadGlAccountID { get; set; }

	public string xauWipSubcontractGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
