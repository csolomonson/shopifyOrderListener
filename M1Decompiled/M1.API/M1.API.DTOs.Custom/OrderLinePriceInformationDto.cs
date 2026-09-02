namespace M1.API.DTOs.Custom;

public class OrderLinePriceInformationDto
{
	public decimal fullUnitPriceBase { get; set; }

	public decimal fullUnitPriceForeign { get; set; }

	public decimal unitPriceBase { get; set; }

	public decimal unitPriceForeign { get; set; }

	public decimal fullExtendedPriceBase { get; set; }

	public decimal fullExtendedPriceForeign { get; set; }

	public decimal extendedDiscountBase { get; set; }

	public decimal extendedDiscountForeign { get; set; }

	public decimal extendedPriceBase { get; set; }

	public decimal extendedPriceForeign { get; set; }

	public decimal taxAmountBase { get; set; }

	public decimal taxAmountForeign { get; set; }

	public decimal secondTaxAmountBase { get; set; }

	public decimal secondTaxAmountForeign { get; set; }

	public decimal discountPercent { get; set; }

	public decimal unitDiscountBase { get; set; }

	public decimal unitDiscountForeign { get; set; }
}
