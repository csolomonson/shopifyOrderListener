namespace M1.API.DTOs.Custom;

public class TaxInformationDto
{
	public string FirstTaxCodeID { get; set; }

	public string SecondTaxCodeID { get; set; }

	public decimal FirstTaxRate { get; set; }

	public decimal SecondTaxRate { get; set; }

	public string NonTaxReasonID { get; set; }
}
