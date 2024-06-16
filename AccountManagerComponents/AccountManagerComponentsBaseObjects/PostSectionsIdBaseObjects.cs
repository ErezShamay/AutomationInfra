namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;

public class PostSectionsIdBaseObjects
{
    public class Preset
    {
        public bool IsEnable { get; set; }
        public bool PrePopulate { get; set; }
        public bool IsMandatory { get; set; }
        public string Category { get; set; }
        public string AccountFieldName { get; set; }
    }

    public class Root
    {
        public string Name { get; set; }
        public List<Preset> Presets { get; set; }
    }
}