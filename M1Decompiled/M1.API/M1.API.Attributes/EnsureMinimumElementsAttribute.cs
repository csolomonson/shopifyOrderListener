using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace M1.API.Attributes;

public class EnsureMinimumElementsAttribute : ValidationAttribute
{
	private readonly int _minElements;

	public EnsureMinimumElementsAttribute(int minElements)
	{
		_minElements = minElements;
	}

	public override bool IsValid(object value)
	{
		return (value as IList)?.Count >= _minElements;
	}
}
