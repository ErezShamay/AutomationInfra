namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class GetValidNumberOfInstallmentsResponse
{
    public class Amounts
    {
        public int additionalProp1 { get; set; }
        public int additionalProp2 { get; set; }
        public int additionalProp3 { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<ValidNumberOfInstallmentsItem> ValidNumberOfInstallmentsItems { get; set; }
    }

    public class ValidNumberOfInstallmentsItem
    {
        public int NumberOfInstallments { get; set; }
        public int FirstInstallmentAmount { get; set; }
        public int SubsequentInstallmentsAmount { get; set; }
        public Amounts Amounts { get; set; }
    }
}