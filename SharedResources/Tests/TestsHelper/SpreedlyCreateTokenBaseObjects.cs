namespace Splitit.Automation.NG.SharedResources.Tests.TestsHelper;

public class SpreedlyCreateTokenBaseObjects
{
    public class CreditCard
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string number { get; set; }
        public string verification_value { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone_number { get; set; }
        public string shipping_address1 { get; set; }
        public string shipping_address2 { get; set; }
        public string shipping_city { get; set; }
        public string shipping_state { get; set; }
        public string shipping_zip { get; set; }
        public string shipping_country { get; set; }
        public string shipping_phone_number { get; set; }
    }

    public class Metadata
    {
        public string key { get; set; }
        public int another_key { get; set; }
        public bool final_key { get; set; }
    }

    public class PaymentMethod
    {
        public CreditCard credit_card { get; set; }
        public string email { get; set; }
        public Metadata metadata { get; set; }
        public bool retained { get; set; }
    }

    public class Root
    {
        public PaymentMethod payment_method { get; set; }
    }
}