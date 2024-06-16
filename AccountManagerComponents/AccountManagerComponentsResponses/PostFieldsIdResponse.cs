namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;

public class PostFieldsIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Field
    {
        public string Id { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }
        public string AccountFieldName { get; set; }
        public string DateFormat { get; set; }
        public List<FileFormat> FileFormats { get; set; }
        public string EnumName { get; set; }
        public List<SelectOption> SelectOptions { get; set; }
        public bool IsMultiple { get; set; }
        public string CheckboxLabel { get; set; }
        public bool HasCheckbox { get; set; }
    }

    public class FileFormat
    {
        public string Type { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Field Field { get; set; }
    }

    public class SelectOption
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}