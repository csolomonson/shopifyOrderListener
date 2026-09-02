namespace M1.Core;

public class DmoIndex
{
	public string IndexName = string.Empty;

	public string Fields = string.Empty;

	public bool Unique;

	public DmoIndex(string fields, bool unique)
	{
		IndexName = fields.Replace(',', '_').Replace(" ", string.Empty);
		Fields = fields;
		Unique = unique;
	}
}
