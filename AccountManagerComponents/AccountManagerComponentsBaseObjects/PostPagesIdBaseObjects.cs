namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;

public class PostPagesIdBaseObjects
{
    public class Root
    {
        public List<SectionId> SectionIds { get; set; }
        public string Description { get; set; }
        public bool HasStatus { get; set; }
        public string Name { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }

    public class SectionId
    {
        public string Id { get; set; }
        public int Order { get; set; }
    }
}