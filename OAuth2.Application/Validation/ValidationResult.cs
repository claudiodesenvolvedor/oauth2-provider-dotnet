namespace OAuth2.Application.Validation;

public sealed record ValidationResult(
    bool IsValid,
    IReadOnlyDictionary<string, string[]> Errors)
{
    public static ValidationResult Success()
    {
        return new ValidationResult(true, new Dictionary<string, string[]>());
    }
}
