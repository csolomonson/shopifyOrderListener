using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderLineInformationDto
{
	public string omlCreatedBy { get; set; }

	public DateTime? omlCreatedDate { get; set; }

	public decimal omlDeliveryQuantityTotal { get; set; }

	public decimal omlDepositAmountBase { get; set; }

	public decimal omlDepositAmountForeign { get; set; }

	public decimal omlDepositPercent { get; set; }

	public decimal omlDiscountPercent { get; set; }

	public string omlDocuments { get; set; }

	public Guid omlUniqueID { get; set; }

	public decimal omlExtendedDiscountBase { get; set; }

	public decimal omlExtendedDiscountForeign { get; set; }

	public decimal omlExtendedPriceBase { get; set; }

	public decimal omlExtendedPriceForeign { get; set; }

	public decimal omlExtendedWeight { get; set; }

	public decimal omlFreightAmountBase { get; set; }

	public decimal omlFreightAmountForeign { get; set; }

	public decimal omlFullExtendedPriceBase { get; set; }

	public decimal omlFullExtendedPriceForeign { get; set; }

	public decimal omlFullUnitPriceBase { get; set; }

	public decimal omlFullUnitPriceForeign { get; set; }

	public bool omlAvalaraIgnoreLine { get; set; }

	public bool omlClosed { get; set; }

	public bool omlConfigured { get; set; }

	public bool omlDeposit { get; set; }

	public bool omlDepositCreated { get; set; }

	public bool omlDepositCredited { get; set; }

	public bool omlPayCommission { get; set; }

	public bool omlPriceOverride { get; set; }

	public bool omlTimeAndMaterial { get; set; }

	public string omlLeadID { get; set; }

	public short omlLeadLineID { get; set; }

	public string omlNonTaxReasonID { get; set; }

	public decimal omlOrderQuantity { get; set; }

	public string omlOrgPartID { get; set; }

	public string omlOrgPartShortDescription { get; set; }

	public string omlPartGroupID { get; set; }

	public string omlPartID { get; set; }

	public string omlPartLongDescriptionRtf { get; set; }

	public string omlPartLongDescriptionText { get; set; }

	public string omlPartRevisionID { get; set; }

	public string omlPartShortDescription { get; set; }

	public string omlProjectAreaID { get; set; }

	public string omlProjectID { get; set; }

	public decimal omlQuantityShipped { get; set; }

	public string omlQuoteID { get; set; }

	public short omlQuoteLineID { get; set; }

	public byte omlQuoteQuantityID { get; set; }

	public string omlReleaseNumber { get; set; }

	public string omlRmaClaimID { get; set; }

	public short omlRmaClaimLineID { get; set; }

	public byte[] omlRowVersion { get; set; }

	public string omlSalesOrderID { get; set; }

	public decimal omlSecondTaxAmountBase { get; set; }

	public decimal omlSecondTaxAmountForeign { get; set; }

	public string omlSecondTaxCodeID { get; set; }

	public short omlSalesOrderLineID { get; set; }

	public decimal omlTaxAmountBase { get; set; }

	public decimal omlTaxAmountForeign { get; set; }

	public string omlTaxCodeID { get; set; }

	public decimal omlUnitDiscountBase { get; set; }

	public decimal omlUnitDiscountForeign { get; set; }

	public string omlUnitOfMeasure { get; set; }

	public decimal omlUnitPriceBase { get; set; }

	public decimal omlUnitPriceForeign { get; set; }

	public decimal omlWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
