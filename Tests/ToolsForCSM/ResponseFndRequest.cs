namespace Splitit.Automation.NG.Backend.Tests.ToolsForCSM;

public class ResponseFndRequest
{
    public class Actions
    {
        public string href { get; set; }
    }

    public class Address
    {
        public string address_line1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class Balances
    {
        public int total_authorized { get; set; }
        public int total_voided { get; set; }
        public int available_to_void { get; set; }
        public int total_captured { get; set; }
        public int available_to_capture { get; set; }
        public int total_refunded { get; set; }
        public int available_to_refund { get; set; }
    }

    public class BillingAddress
    {
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public Phone phone { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Actions actions { get; set; }
        public Refund refund { get; set; }
    }

    public class Metadata
    {
        public string splitittransactionidentifier { get; set; }
        public string cardOnFile { get; set; }
        public string externalOrderId { get; set; }
        public string paymentRequestId { get; set; }
    }

    public class Phone
    {
        public string number { get; set; }
    }

    public class Processing
    {
        public string acquirer_transaction_id { get; set; }
        public string retrieval_reference_number { get; set; }
        public string merchant_category_code { get; set; }
        public string scheme_merchant_id { get; set; }
        public string scheme { get; set; }
        public bool aft { get; set; }
    }

    public class Refund
    {
        public string href { get; set; }
    }

    public class Risk
    {
        public bool flagged { get; set; }
        public double score { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public DateTime requested_on { get; set; }
        public Source source { get; set; }
        public DateTime expires_on { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string payment_type { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public bool approved { get; set; }
        public Balances balances { get; set; }
        public Risk risk { get; set; }
        public Customer customer { get; set; }
        public Shipping shipping { get; set; }
        public string payment_ip { get; set; }
        public Metadata metadata { get; set; }
        public Processing processing { get; set; }
        public bool merchant_initiated { get; set; }
        public string eci { get; set; }
        public string scheme_id { get; set; }
        public Links _links { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Shipping
    {
        public Address address { get; set; }
    }

    public class Source
    {
        public string id { get; set; }
        public string type { get; set; }
        public BillingAddress billing_address { get; set; }
        public int expiry_month { get; set; }
        public int expiry_year { get; set; }
        public string scheme { get; set; }
        public string last4 { get; set; }
        public string fingerprint { get; set; }
        public string bin { get; set; }
        public string card_type { get; set; }
        public string card_category { get; set; }
        public string issuer { get; set; }
        public string issuer_country { get; set; }
        public string product_id { get; set; }
        public string product_type { get; set; }
        public string avs_check { get; set; }
        public string cvv_check { get; set; }
        public string payment_account_reference { get; set; }
    }
}