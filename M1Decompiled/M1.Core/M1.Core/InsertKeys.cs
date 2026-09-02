namespace M1.Core;

public class InsertKeys
{
	public string Field;

	public object SourceValue;

	public object DestinationValue;

	public InsertKeys(string field, object sourceValue, object destinationValue)
	{
		Field = field;
		SourceValue = sourceValue;
		DestinationValue = destinationValue;
	}
}
