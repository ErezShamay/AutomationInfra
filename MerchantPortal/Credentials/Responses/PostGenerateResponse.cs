namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Credentials.Responses;

public class PostGenerateResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public string Secret { get; set; }
        public string RecipientEmail { get; set; }
        public bool IsPgpEncrypted { get; set; }
        public string UniqueId { get; set; }
        public string First4 { get; set; }
    }
}