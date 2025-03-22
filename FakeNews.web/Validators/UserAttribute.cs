using System.ComponentModel.DataAnnotations;

namespace FakeNews.web.Validators
{


	public class RequireDigitAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is string password && password.Any(char.IsDigit))
			{
				return ValidationResult.Success;
			}
			return new ValidationResult("Password must contain at least one digit.");
		}
	}

	public class RequireLowercaseAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is string password && password.Any(char.IsLower))
			{
				return ValidationResult.Success;
			}
			return new ValidationResult("Password must contain at least one lowercase letter.");
		}
	}

	public class RequireUppercaseAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is string password && password.Any(char.IsUpper))
			{
				return ValidationResult.Success;
			}
			return new ValidationResult("Password must contain at least one uppercase letter.");
		}
	}

	public class RequireNonAlphanumericAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is string password && password.Any(ch => !char.IsLetterOrDigit(ch)))
			{
				return ValidationResult.Success;
			}
			return new ValidationResult("Password must contain at least one non-alphanumeric character.");
		}
	}

	public class RequireUniqueCharsAttribute : ValidationAttribute
	{
		private readonly int _requiredUniqueChars;

		public RequireUniqueCharsAttribute(int requiredUniqueChars)
		{
			_requiredUniqueChars = requiredUniqueChars;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is string password && password.Distinct().Count() >= _requiredUniqueChars)
			{
				return ValidationResult.Success;
			}
			return new ValidationResult($"Password must contain at least {_requiredUniqueChars} unique characters.");
		}
	}

}

