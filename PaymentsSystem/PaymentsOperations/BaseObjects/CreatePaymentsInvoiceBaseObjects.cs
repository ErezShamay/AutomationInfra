namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;

public class CreatePaymentsInvoiceBaseObjects
{
    public class InvoiceDetail
    {
        public string externalInvoiceId { get; set; }
        public string externalRowData { get; set; }
        public DateTime externalCreateAt { get; set; }
        public bool externalProcessed { get; set; }
        public int externalTax { get; set; }
        public int externalSubTotal { get; set; }
        public int externalDiscountTotal { get; set; }
        public int externalAmountRemaining { get; set; }
        public int externalTotal { get; set; }
    }

    public class Root
    {
        public List<InvoiceDetail> invoiceDetails { get; set; }
    }
}