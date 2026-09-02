using System;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class AdditionalFilterParameterDateRange : AdditionalFilterParameter
{
	private readonly bool _ignoreTime = true;

	private DateTime? _valueStart;

	private DateTime? _valueEnd;

	public string ValueField = string.Empty;

	public DateTime? ValueStart
	{
		get
		{
			return _valueStart;
		}
		set
		{
			if (_ignoreTime)
			{
				value = value?.Date;
			}
			if (_valueStart != value)
			{
				_valueStart = value;
				OnFilterChanged();
			}
		}
	}

	public DateTime? ValueEnd
	{
		get
		{
			return _valueEnd;
		}
		set
		{
			if (_ignoreTime)
			{
				value = value?.Date;
			}
			if (_valueEnd != value)
			{
				_valueEnd = value;
				OnFilterChanged();
			}
		}
	}

	public AdditionalFilterParameterDateRange(string caption)
		: base(caption)
	{
	}

	public AdditionalFilterParameterDateRange(string caption, bool ignoreTime)
		: base(caption)
	{
		_ignoreTime = ignoreTime;
	}

	protected override string ProcessFilterExpression(bool sql)
	{
		if (sql)
		{
			if (SqlOnly)
			{
				if (_valueStart.HasValue || _valueEnd.HasValue)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (ValueField.Length != 0)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(" And ");
						}
						if (_valueStart.HasValue)
						{
							stringBuilder.Append(ValueField + " >= " + _valueStart.Value.ToLinq());
							if (_valueEnd.HasValue)
							{
								stringBuilder.Append(" And ");
							}
						}
						if (_valueEnd.HasValue)
						{
							if (_ignoreTime)
							{
								stringBuilder.Append(ValueField + " < " + _valueEnd.Value.ToLinq());
							}
							else
							{
								stringBuilder.Append(ValueField + " <= " + _valueEnd.Value.ToLinq());
							}
						}
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Insert(0, '(');
						stringBuilder.Append(')');
					}
					return stringBuilder.ToString();
				}
				if (!IgnoreWhenEmpty)
				{
					return "0=1";
				}
			}
		}
		else if (!SqlOnly && (_valueStart.HasValue || _valueEnd.HasValue))
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			if (ValueField.Length != 0)
			{
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(" And ");
				}
				if (_valueStart.HasValue)
				{
					stringBuilder2.Append(ValueField + " >= " + _valueStart.Value.ToShortDateString().ToLinq());
					if (_valueEnd.HasValue)
					{
						stringBuilder2.Append(" And ");
					}
				}
				if (_valueEnd.HasValue)
				{
					stringBuilder2.Append(ValueField + " <= " + _valueEnd.Value.ToShortDateString().ToLinq());
				}
			}
			if (stringBuilder2.Length != 0)
			{
				stringBuilder2.Insert(0, '(');
				stringBuilder2.Append(')');
			}
			return stringBuilder2.ToString();
		}
		return string.Empty;
	}
}
