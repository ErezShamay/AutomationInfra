namespace Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.Response;

public class GetImportResponse
{
    public class DynamicFee
    {
        public int TransactionFlatFee { get; set; }
        public int TransactionPercentFee { get; set; }
        public string CurrencyIsoCode { get; set; }
        public string ProductCode { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ProcessorConfigurations
    {
        public int Id { get; set; }
        public int ProcessorId { get; set; }
        public string ProcessorIntegration { get; set; }
        public string CertificationLevel { get; set; }
        public string Status { get; set; }
        public bool IsDeletedFromSOB { get; set; }
        public string Fee { get; set; }
        public string SpecificRegistrationLink { get; set; }
        public bool ShowNewAccounts { get; set; }
        public bool ShowExistingAccounts { get; set; }
        public bool None { get; set; }
        public bool IsNoneSecure { get; set; }
        public bool SpecialSettingsRequired { get; set; }
        public bool RequiresSplititSpecificAccount { get; set; }
        public bool RequiresMerchantsToChange { get; set; }
        public string Note { get; set; }
        public string Logo { get; set; }
        public string DisplayName { get; set; }
        public bool IsSupportAllCountries { get; set; }
        public string InstructionsUrl { get; set; }
        public List<string> Countries { get; set; }
        public List<string> PaymentMethods { get; set; }
        public List<DynamicFee> DynamicFees { get; set; }
        public bool IsMerchantOfRecord { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public ProcessorConfigurations ProcessorConfigurations { get; set; }
    }
}