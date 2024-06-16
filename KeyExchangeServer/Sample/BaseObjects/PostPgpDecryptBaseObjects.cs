namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;

public class PostPgpDecryptBaseObjects
{
    public class Root
    {
        public string SenderPublicKey { get; set; }
        public string RecipientPrivateKey { get; set; }
        public string RecipientPassphrase { get; set; }
        public string EncryptedContent { get; set; }
    }
}