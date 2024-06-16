namespace Splitit.Automation.NG.Backend.Services.Processors.ErrorClassifications.BaseObjects;

public class PostProcessorProcessorIdBaseObjects
{
    public class Root
    {
        public List<UpsertGatewayErrorClassification> UpsertGatewayErrorClassifications { get; set; }
        public int ProcessorId { get; set; }
    }

    public class UpsertGatewayErrorClassification
    {
        public int Id { get; set; }
        public string GatewayErrorCode { get; set; }
        public int PisException { get; set; }
        public bool AutoHandlingMode { get; set; }
        public bool CcNeedsUpdate { get; set; }
        public string GatewayErrorMessage { get; set; }
        public bool IsDeleted { get; set; }
    }
}