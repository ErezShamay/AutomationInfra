namespace Splitit.Automation.NG.Backend.Services.WebApi.InstallmentPlan.Responses;

public class PostTermsAndConditionsResponse
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
        public TermsAndConditions TermsAndConditions { get; set; }
    }

    public class TermsAndConditions
    {
        public string Agreement { get; set; }
        public string ImportantNote { get; set; }
        public string FullContent { get; set; }
        public string PrivacyPolicy { get; set; }
        public string WhatYouNeedToKnow_AuthHoldDefine { get; set; }
        public string WhatYouNeedToKnow_BudgetManagement { get; set; }
        public string WhatYouNeedToKnow_CardTypeDetails { get; set; }
    }
}