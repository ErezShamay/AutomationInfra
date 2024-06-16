namespace Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.Responses;

public class ResponseProcessing
{
    public class PendingPayment
    {
        public string internal_id { get; set; }
        public DateTime created_at { get; set; }
        public string sku { get; set; }
        public string merchant_business_unit { get; set; }
        public int installment_number { get; set; }
        public int billing_amount { get; set; }
        public string billing_currency { get; set; }
        public int num_of_installments { get; set; }
        public string installment_plan_number { get; set; }
        public string payment_activity { get; set; }
    }

    public class Root
    {
        public List<PendingPayment> pending_payments { get; set; }
    }
}