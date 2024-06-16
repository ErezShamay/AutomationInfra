namespace Splitit.Automation.NG.SharedResources.Tests.TestsHelper;

public class SpreedlyCreateTokenResponse
{
    public class BinMetadata
    {
        public string message { get; set; }
    }

    public class Metadata
    {
        public string key { get; set; }
        public int another_key { get; set; }
        public bool final_key { get; set; }
    }

    public class PaymentMethod
    {
        public string token { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string email { get; set; }
        public object data { get; set; }
        public string storage_state { get; set; }
        public bool test { get; set; }
        public Metadata metadata { get; set; }
        public object callback_url { get; set; }
        public string last_four_digits { get; set; }
        public string first_six_digits { get; set; }
        public string card_type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone_number { get; set; }
        public string company { get; set; }
        public string full_name { get; set; }
        public bool eligible_for_card_updater { get; set; }
        public string shipping_address1 { get; set; }
        public string shipping_address2 { get; set; }
        public string shipping_city { get; set; }
        public string shipping_state { get; set; }
        public string shipping_zip { get; set; }
        public string shipping_country { get; set; }
        public string shipping_phone_number { get; set; }
        public string issuer_identification_number { get; set; }
        public bool click_to_pay { get; set; }
        public bool managed { get; set; }
        public string payment_method_type { get; set; }
        public List<object> errors { get; set; }
        public BinMetadata bin_metadata { get; set; }
        public string fingerprint { get; set; }
        public string verification_value { get; set; }
        public string number { get; set; }
    }

    public class Root
    {
        public Transaction transaction { get; set; }
    }

    public class Transaction
    {
        public string token { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool succeeded { get; set; }
        public string transaction_type { get; set; }
        public bool retained { get; set; }
        public string state { get; set; }
        public string message_key { get; set; }
        public string message { get; set; }
        public PaymentMethod payment_method { get; set; }
    }
}