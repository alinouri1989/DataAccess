using San.CoreCommon.Attribute;
using San.CoreCommon.ValidationRule;

namespace San.CoreCommon.ValidationRule
{
  [ScopedService]
  public class TokenValidationRule : ITokenValidationRule
  {
    private readonly IValidationResult _validationResult;

    public TokenValidationRule()
    {
    }

    public TokenValidationRule(IValidationResult validationResult) => this._validationResult = validationResult;

    public IValidationResult IsSatisfied(long valueToValidate)
    {
      if (valueToValidate > 0L && valueToValidate.ToString().Length >= 5)
        return this._validationResult;
      this._validationResult.Status = (short) -131;
      return this._validationResult;
    }
  }
}
