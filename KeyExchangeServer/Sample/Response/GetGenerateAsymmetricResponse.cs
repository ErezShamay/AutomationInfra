namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;

public class GetGenerateAsymmetricResponse
{
    public class Root
    {
        public string Note { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string PgpPassPhrase { get; set; }
    }
}