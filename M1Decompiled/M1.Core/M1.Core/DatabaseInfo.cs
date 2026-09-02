namespace M1.Core;

public class DatabaseInfo
{
	public string Name = string.Empty;

	public string Description = string.Empty;

	public string Version = string.Empty;

	public string ExtensionVersions = string.Empty;

	public bool DataDictionary;

	public bool SingleUser;

	public string CollationName = string.Empty;

	public string TenantId = string.Empty;

	private string _CompatabilityText = string.Empty;

	private byte _CompatibilityLevel;

	public string CompatabilityText => _CompatabilityText;

	public byte CompatibilityLevel
	{
		get
		{
			return _CompatibilityLevel;
		}
		set
		{
			_CompatibilityLevel = value;
			switch (_CompatibilityLevel)
			{
			case 70:
				_CompatabilityText = "7";
				break;
			case 80:
				_CompatabilityText = "2000";
				break;
			case 90:
				_CompatabilityText = "2005";
				break;
			case 100:
				_CompatabilityText = "2008";
				break;
			case 110:
				_CompatabilityText = "2012";
				break;
			case 120:
				_CompatabilityText = "2014";
				break;
			default:
				_CompatabilityText = string.Empty;
				break;
			}
		}
	}

	public DatabaseInfo(bool dataDictionary)
	{
		DataDictionary = dataDictionary;
	}

	public override string ToString()
	{
		return ToString(includeVersion: true);
	}

	public string ToString(bool includeVersion)
	{
		if (DataDictionary)
		{
			return Name.ToUpper() + (includeVersion ? (" - " + Version) : string.Empty) + (SingleUser ? " (Single User)" : string.Empty) + ((CompatibilityLevel <= 80) ? ("(Compatibility set to " + CompatabilityText + ")") : string.Empty);
		}
		if (Name.Length >= 4)
		{
			return Name.Substring(3).ToUpper() + " - " + Description + (includeVersion ? (" - " + Version) : string.Empty) + (SingleUser ? " (Single User)" : string.Empty) + ((CompatibilityLevel <= 80) ? ("(Compatibility set to " + CompatabilityText + ")") : string.Empty);
		}
		return string.Empty;
	}
}
