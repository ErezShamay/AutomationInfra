namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;

public class ResponseAuditLog
{
    public class AuditLog
    {
        public string ActivityExecutionDate { get; set; }
        public string Activity { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public bool Result { get; set; }
        public string ResultMessage { get; set; }
        public object AdditionalInfo { get; set; }
        public object Error { get; set; }
        public TouchPoint TouchPoint { get; set; }
        public string TraceId { get; set; }
        public DateTime ActivityTimestamp { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<object> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public string PlanNumber { get; set; }
        public string MerchantName { get; set; }
        public string CreationDate { get; set; }
        public List<AuditLog> AuditLogs { get; set; }
        public ResponseHeader ResponseHeader { get; set; }
    }

    public class TouchPoint
    {
        public int? LegacyID { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string SubVersion { get; set; }
        public string Name { get; set; }
        public string CodeWithVersion { get; set; }
    }
}