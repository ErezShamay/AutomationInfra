namespace Splitit.Automation.NG.Backend.Services.Processors.AuthenticationParameters.BaseObjects;

public class PostProcessorProcessorIdBaseObjects
{
    public class Root
    {
        public List<UpsertAuthenticationParameter> UpsertAuthenticationParameters { get; set; }
        public int ProcessorId { get; set; }
    }

    public class UpsertAuthenticationParameter
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public int Order { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsDeleted { get; set; }
    }
}