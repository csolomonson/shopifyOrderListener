namespace M1.Core;

public class AdditionalFilterParameterBool : AdditionalFilterParameter
{
	private bool _Value;

	public bool Value
	{
		get
		{
			return _Value;
		}
		set
		{
			if (_Value != value)
			{
				_Value = value;
				OnFilterChanged();
			}
		}
	}

	public AdditionalFilterParameterBool(string caption)
		: base(caption)
	{
	}

	protected override string ProcessFilterExpression(bool sql)
	{
		string text = (sql ? SqlFilterExpression : AdoFilterExpression);
		if (IgnoreWhenEmpty && !Value)
		{
			return string.Empty;
		}
		return text.Replace("{%value%}", Value ? "1" : "0");
	}
}
