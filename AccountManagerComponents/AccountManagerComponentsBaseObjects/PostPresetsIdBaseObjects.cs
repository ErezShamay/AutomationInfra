namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;

public class PostPresetsIdBaseObjects
{
    public class Field
    {
        public string Id { get; set; }
        public int Order { get; set; }
    }

    public class Root
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Footer { get; set; }
        public string Arrangement { get; set; }
        public string AddItemText { get; set; }
        public string RemoveItemText { get; set; }
        public int TotalNumberOfForms { get; set; }
    }
}