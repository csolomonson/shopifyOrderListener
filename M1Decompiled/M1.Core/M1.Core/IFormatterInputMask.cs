namespace M1.Core;

public interface IFormatterInputMask
{
	bool ReadOnly { get; }

	string GenerateMask(FieldDefinition field, bool grid);

	string SetTextFromValue(object value);

	string SetValueFromText(string text, object prevValue);
}
