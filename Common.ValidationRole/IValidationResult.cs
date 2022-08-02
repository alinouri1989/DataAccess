namespace San.CoreCommon.ValidationRule
{
  public interface IValidationResult
  {
    short Status { get; set; }

    string Description { get; set; }

    object StateObject { get; set; }

    bool IsSatisfied { get; }
  }
}
