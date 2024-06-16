namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.BaseObjects;

public class InvoicedBaseObjects
{
    public class InvoiceFees
    {
        public double fixedFee { get; set; }
        public double variableFee { get; set; }
    }

    public class Payment
    {
        public string paymentRecordId { get; set; }
        public string inoviceId { get; set; }
        public InvoiceFees invoiceFees { get; set; }
    }

    public class Root
    {
        public List<Payment> payments { get; set; }
    }
}