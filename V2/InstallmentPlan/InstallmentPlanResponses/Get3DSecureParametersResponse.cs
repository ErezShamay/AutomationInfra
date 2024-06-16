namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class Get3DSecureParametersResponse
{
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
        public string IssuerRedirectUrl { get; set; }
        public string PaReq { get; set; }
        public string Md { get; set; }
        public string TermUrl { get; set; }
        public ThreeDSecureParams ThreeDSecureParams { get; set; }
    }

    public class ThreeDSecureParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }
}