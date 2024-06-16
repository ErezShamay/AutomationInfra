namespace Splitit.Automation.NG.Backend.Services.Chargebacks.Responses;

public class DeleteChargebacksIdDeleteCommentCommentIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
        public ExtraData ExtraData { get; set; }
    }

    public class ExtraData
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Root
    {
        public string TraceId { get; set; }
        public Error Error { get; set; }
    }
}