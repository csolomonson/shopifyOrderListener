using System.ComponentModel.DataAnnotations;

namespace M1.API.Attributes;

public class ValidateNullableDecimalAttribute : ValidationAttribute
{
	public override bool IsValid(object value)
	{
		return IsValid((value != null) ? value : ((object)0));
	}

	protected override ValidationResult IsValid(object value, ValidationContext context)
	{
		if ((value as decimal?).HasValue)
		{
			return ValidationResult.Success;
		}
		return new ValidationResult("xx");
	}
}
