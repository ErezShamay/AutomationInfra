namespace Splitit.Automation.NG.Backend.Services.Processors.DesputeConfiguration.BaseObjects;

public class PostProcessorProcessorIdBaseObjects
{
    public class Root
    {
        public List<UpsertDisputeConfiguration> UpsertDisputeConfiguration { get; set; }
    }

    public class UpsertDisputeConfiguration
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string ReasonCode { get; set; }
        public string DisputeLiability { get; set; }
    }
}