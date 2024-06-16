namespace Splitit.Automation.NG.Backend.Services.V2.Terminal.TerminalResponses;

public class PoliciesResponse
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<Terminal> Terminals { get; set; }
    }
    
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
    }

    public class DefaultCulture
    {
        public string ParentName { get; set; }
        public string CultureName { get; set; }
        public string DisplayName { get; set; }
        public string IsoCountryName { get; set; }
        public string IsoLanguageName { get; set; }
        public int LCID { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Parameters
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Terminal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public TerminalPolicies TerminalPolicies { get; set; }
        public string LogoURL { get; set; }
        public DefaultCulture DefaultCulture { get; set; }
        public Parameters Parameters { get; set; }
        public bool IsCopyLinkEnabledInVpos3 { get; set; }
        public bool IsPhoneOrderEnabledInVpos3 { get; set; }
    }

    public class TerminalPolicies
    {
        public List<int> AllowedNumberOfInstallments { get; set; }
        public List<Currency> Currencies { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public bool TryFundPlanByDefault { get; set; }
        public bool HasAvailableCredit { get; set; }
        public string DefaultPlanStrategy { get; set; }
        public bool DelayedChargeVPOS { get; set; }
        public List<TransactionLimit> TransactionLimits { get; set; }
        public bool SetOrderIdMandatory { get; set; }
    }

    public class TransactionLimit
    {
        public string Type { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string CurrencyCode { get; set; }
    }
}