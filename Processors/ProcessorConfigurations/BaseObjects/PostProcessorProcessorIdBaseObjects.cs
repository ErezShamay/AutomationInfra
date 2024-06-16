namespace Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.BaseObjects;

public class PostProcessorProcessorIdBaseObjects
{
    public class Root
    {
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
        public int ProcessorId { get; set; }
        public int Id { get; set; }
        public bool IsMerchantOfRecord { get; set; }
    }
}