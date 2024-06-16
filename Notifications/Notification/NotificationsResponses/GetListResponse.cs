namespace Splitit.Automation.NG.Backend.Services.Notifications.Notification.NotificationsResponses;

public class GetListResponse
{
    public class InstallmentPlanSlim
    {
        public string installmentPlanNumber { get; set; }
    }

    public class PagingResponseHeader
    {
        public int totalNumber { get; set; }
    }

    public class Root
    {
        public List<Webhook> webhooks { get; set; }
        public PagingResponseHeader pagingResponseHeader { get; set; }
        public object errors { get; set; }
        public int statusCode { get; set; }
        public string traceId { get; set; }
        public bool isSuccess { get; set; }
    }

    public class Webhook
    {
        public string id { get; set; }
        public DateTime createdDate { get; set; }
        public bool success { get; set; }
        public string traceId { get; set; }
        public string payload { get; set; }
        public int processorId { get; set; }
        public string processorName { get; set; }
        public string gatewayEventType { get; set; }
        public string gatewayTransactionId { get; set; }
        public string merchantTransactionId { get; set; }
        public string splititRefTransactionId { get; set; }
        public string splititEventType { get; set; }
        public InstallmentPlanSlim installmentPlanSlim { get; set; }
    }
}