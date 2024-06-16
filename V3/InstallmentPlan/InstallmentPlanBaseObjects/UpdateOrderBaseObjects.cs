namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;

public class UpdateOrderBaseObjects
{
    public class UpdateOrder
    {
        public Identifier Identifier = new();
        public string RefOrderNumber { get; set; }
        public string TrackingNumber { get; set; }
        public bool Capture { get; set; }
        public string ShippingStatus { get; set; }
    }

    public class Identifier
    {
        public ExtendedParams ExtendedParams = new();
        public string RefOrderNumber { get; set; }
        public string InstallmentPlanNumber { get; set; }
    }

    public class ExtendedParams
    {
        public List<Dictionary<string, string>> extendedParams = new();
    }
}