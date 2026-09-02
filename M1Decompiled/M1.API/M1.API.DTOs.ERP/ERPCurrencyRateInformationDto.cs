using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCurrencyRateInformationDto
{
	public string mcpApGlAccountID { get; set; }

	public string mcpArGlAccountID { get; set; }

	public string mcpCurrencyRateID { get; set; }

	public string mcpCreatedBy { get; set; }

	public DateTime? mcpCreatedDate { get; set; }

	public string mcpDescription { get; set; }

	public Guid mcpUniqueID { get; set; }

	public string mcpExchangeGainGlAccountID { get; set; }

	public string mcpExchangeLossGlAccountID { get; set; }

	public byte[] mcpRowVersion { get; set; }

	public string mcpSymbol { get; set; }

	public string mcpUnrealisedExGainGlAccountID { get; set; }

	public string mcpUnrealisedExLossGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
