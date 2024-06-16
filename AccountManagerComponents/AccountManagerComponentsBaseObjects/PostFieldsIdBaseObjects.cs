namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;

public class PostFieldsIdBaseObjects
{
    public class FileFormat
    {
        public string Type { get; set; }
    }

    public class Root
    {
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

    public class SelectOption
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}