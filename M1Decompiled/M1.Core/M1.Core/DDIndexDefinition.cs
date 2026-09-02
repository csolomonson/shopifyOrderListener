namespace M1.Core;

public class DDIndexDefinition
{
	public string IndexName = string.Empty;

	public string Fields = string.Empty;

	public bool Unique;

	public DDIndexDefinition(string fields, bool unique)
	{
		IndexName = fields.Replace(',', '_').Replace(" ", string.Empty);
		Fields = fields;
		Unique = unique;
	}
}
