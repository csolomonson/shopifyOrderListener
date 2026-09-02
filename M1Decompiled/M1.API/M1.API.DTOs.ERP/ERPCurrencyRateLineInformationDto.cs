using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCurrencyRateLineInformationDto
{
	public string mclCreatedBy { get; set; }

	public DateTime? mclCreatedDate { get; set; }

	public string mclCurrencyRateID { get; set; }

	public DateTime? mclEffectiveDate { get; set; }

	public Guid mclUniqueID { get; set; }

	public decimal mclExchangeRate { get; set; }

	public string mclReference { get; set; }

	public byte[] mclRowVersion { get; set; }

	public int mclCurrencyRateLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
