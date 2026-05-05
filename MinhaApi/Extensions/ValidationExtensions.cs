using FluentValidation;
using FluentValidation.Results;

public static class ValidationExtensions
{
    public static IResult ValidateRequest<T>(this T model, IValidator<T> validator)
    {
        ValidationResult result = validator.Validate(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errors });
        }

        return null;
    }
}
