namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class GetSchedulesResponse
{
    public class Element
    {
        public int InstallmentNumber { get; set; }
        public DateTime ChargeDate { get; set; }
        public int ChargeAmount { get; set; }
        public int RequiredCredit { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<Schedule> Schedules { get; set; }
        public string InstallmentsPicker { get; set; }
        public string Headline { get; set; }
    }

    public class Schedule
    {
        public int NumberOfInstallments { get; set; }
        public bool Deposit { get; set; }
        public List<Element> Elements { get; set; }
    }
}