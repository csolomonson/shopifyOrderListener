using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLeadLineInformationDto
{
	public string lolCreatedBy { get; set; }

	public DateTime? lolCreatedDate { get; set; }

	public string lolCurrencyRateID { get; set; }

	public string lolDescription { get; set; }

	public decimal lolDiscountAmount { get; set; }

	public decimal lolDiscountAmountForeign { get; set; }

	public decimal lolDiscountPercent { get; set; }

	public Guid lolUniqueID { get; set; }

	public decimal lolExchangeRate { get; set; }

	public DateTime? lolForecastDate { get; set; }

	public decimal lolGrossAmount { get; set; }

	public decimal lolGrossAmountForeign { get; set; }

	public bool lolCreatedFromMobile { get; set; }

	public bool lolCustomRate { get; set; }

	public bool lolTransferredToQuote { get; set; }

	public DateTime? lolLeadDate { get; set; }

	public string lolLeadID { get; set; }

	public string lolOrgPartID { get; set; }

	public string lolOrgPartShortDescription { get; set; }

	public string lolPartGroupID { get; set; }

	public string lolPartID { get; set; }

	public int lolPartPriceID { get; set; }

	public string lolPartRevisionID { get; set; }

	public decimal lolQuantity { get; set; }

	public string lolResolutionReasonID { get; set; }

	public decimal lolRevenueForecast { get; set; }

	public decimal lolRevenueForecastForeign { get; set; }

	public byte[] lolRowVersion { get; set; }

	public short lolLeadLineID { get; set; }

	public string lolUnitOfMeasure { get; set; }

	public decimal lolUnitSalePriceBase { get; set; }

	public decimal lolUnitSalePriceForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
