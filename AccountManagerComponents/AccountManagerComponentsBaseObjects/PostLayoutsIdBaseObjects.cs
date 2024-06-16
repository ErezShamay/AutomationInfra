namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;

public class PostLayoutsIdBaseObjects
{
    public class PagesId
    {
        public string Id { get; set; }
        public int Order { get; set; }
    }

    public class Root
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PagesId> PagesIds { get; set; }
        public string Type { get; set; }
        public bool HasBackNavigation { get; set; }
        public bool AllowPageSkip { get; set; }
        public string NavigationView { get; set; }
        public bool HasStatusIndication { get; set; }
    }
}