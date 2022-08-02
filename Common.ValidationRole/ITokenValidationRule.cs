namespace San.CoreCommon.ValidationRule
{
  public interface ITokenValidationRule
  {
    IValidationResult IsSatisfied(long valueToValidate);
  }
}
