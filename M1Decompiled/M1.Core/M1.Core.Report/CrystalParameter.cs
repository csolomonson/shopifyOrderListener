using System.Diagnostics;
using System.Runtime.InteropServices;

namespace M1.Core.Report;

[DebuggerDisplay("{Name} - {Text}, Multiple = {EnableMultipleValues}, IsRange = {IsRange}")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
public class CrystalParameter
{
	private ReportParameterData _Data;

	public string Name { get; set; }

	public string Text { get; set; }

	public int MaximumValue { get; set; }

	public int ValueType { get; set; }

	public bool EnableMultipleValues { get; set; }

	public bool IsRange { get; set; }

	public bool InUse { get; set; }

	public ReportParameterData Data
	{
		get
		{
			return _Data;
		}
		set
		{
			_Data = value;
		}
	}

	public CrystalParameter(string name, string text, int valueType, bool enableMultiple, bool isRange, int maximum, bool inUse)
	{
		Name = name;
		if (text == null)
		{
			Text = string.Empty;
		}
		else
		{
			Text = text;
		}
		ValueType = valueType;
		EnableMultipleValues = enableMultiple;
		IsRange = isRange;
		MaximumValue = maximum;
		InUse = inUse;
	}
}
