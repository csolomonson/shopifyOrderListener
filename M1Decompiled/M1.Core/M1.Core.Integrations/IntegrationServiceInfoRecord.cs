namespace M1.Core.Integrations;

public class IntegrationServiceInfoRecord
{
	private const int _defaultPollingFrequencyValue = 60;

	public IntegrationServiceInfoRecordType IntegrationType { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public string DatabaseId { get; set; }

	public int PollingFrequency { get; set; } = 60;

	public bool Inactive { get; set; }

	public bool Added { get; set; }

	public bool IsSynced { get; set; }

	public string TenantId { get; set; }
}
