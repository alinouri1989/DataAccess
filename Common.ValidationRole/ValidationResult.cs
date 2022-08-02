using San.CoreCommon.Attribute;
using San.CoreCommon.Constants;

namespace San.CoreCommon.ValidationRule
{
    [ScopedService]
    public class ValidationResult : IValidationResult
    {
        private string mDescription;

        public ValidationResult() => this.Status = (short)0;

        public short Status { get; set; }

        public string Description
        {
            get
            {
                if (this.mDescription == null)
                    this.mDescription = PaymentStatus.GetDescription(this.Status);
                return this.mDescription;
            }
            set => this.mDescription = value;
        }

        public object StateObject { get; set; }

        public bool IsSatisfied => this.Status == (short)0;

        public short Succeed => 0;
    }
}
