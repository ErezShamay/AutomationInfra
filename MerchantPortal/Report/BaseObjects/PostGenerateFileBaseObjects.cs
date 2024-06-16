namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.BaseObjects;

public class PostGenerateFileBaseObjects
{
    public class Data
    {
        public ParamsValues ParamsValues { get; set; }
        public PagingRequest PagingRequest { get; set; }
    }

    public class PagingRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class ParamsValues
    {
        public List<string> additionalProp1 { get; set; }
        public List<string> additionalProp2 { get; set; }
        public List<string> additionalProp3 { get; set; }
    }

    public class Root
    {
        public string Code { get; set; }
        public string Extension { get; set; }
        public Data Data { get; set; }
    }
}