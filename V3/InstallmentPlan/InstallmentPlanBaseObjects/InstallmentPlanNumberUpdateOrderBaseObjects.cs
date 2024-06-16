namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;

public class InstallmentPlanUpdateOrderBaseObjects
{
    public class UpdateOrder
    {
        public string TrackingNumber { get; set; }
        public string RefOrderNumber { get; set; }
        public string ShippingStatus { get; set; }
        public bool Capture { get; set; }
    }
}