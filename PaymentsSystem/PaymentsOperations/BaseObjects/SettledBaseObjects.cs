namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;

public class SettledBaseObjects
{
    public class Result
    {
        public string installment_plan_number { get; set; }
        public string external_id { get; set; }
        public bool success { get; set; }
        public string internal_id { get; set; }
        public string date { get; set; }
        public string subscription { get; set; }
        public string original_txn_date { get; set; }
        public string installment_number { get; set; }
        public string original_amount { get; set; }
        public string translated_amount { get; set; }
        public string card_type { get; set; }
        public string error_type { get; set; }
        public string error_code { get; set; }
        public string error_details { get; set; }
        public string stack_trace { get; set; }
        public int processed_count { get; set; }
        public int error_count { get; set; }
    }

    public class Root
    {
        public int processed_count { get; set; }
        public int error_count { get; set; }
        public string batch_id { get; set; }
        public List<Result> results { get; set; }
    }
}