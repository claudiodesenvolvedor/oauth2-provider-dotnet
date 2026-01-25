namespace OAuth2.Application.Validation;

public interface IRequestValidator<in TRequest>
{
    ValidationResult Validate(TRequest request);
}
