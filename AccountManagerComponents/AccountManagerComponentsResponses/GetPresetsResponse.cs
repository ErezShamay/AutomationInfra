namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;

public class GetPresetsResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Preset
    {
        public bool IsEnable { get; set; }
        public bool PrePopulate { get; set; }
        public bool IsMandatory { get; set; }
        public string Category { get; set; }
        public string AccountFieldName { get; set; }
    }

    public class PresetData
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public List<Preset> Presets { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<PresetData> PresetDatas { get; set; }
        public int TotalResult { get; set; }
    }
}