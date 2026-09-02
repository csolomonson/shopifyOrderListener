using System;

namespace M1.Core.Integrations;

public class IntegrationTransactionQueueSummary
{
	public string Status { get; set; }

	public int Count { get; set; }

	public DateTime? MaxDate { get; set; }
}
