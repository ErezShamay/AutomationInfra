using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Backend.Services.AdminPortal.BaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminPortal.MerchantKey.Functionality;
using Splitit.Automation.NG.Backend.Services.IdServer.Functionality;
using Splitit.Automation.NG.Backend.Services.IdServer.requests;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using PostGenerateBaseObjects = Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects.PostGenerateBaseObjects;

namespace Splitit.Automation.NG.Backend.Tests.KeyEncryptionTests.KeyEncryptionTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("KeyEncryptionTests")]
[AllureDisplayIgnored]
public class KeyEncryptionTests
{
    private RequestHeader? _requestHeader;
    private readonly GetGenerateAsymmetricFunctionality _getGenerateAsymmetricFunctionality;
    private readonly PostKeyFunctionality _postKeyFunctionality;
    private readonly PostKeyBaseObjects.Root _postOwnerBaseObjects;
    private readonly PostGenerateFunctionality _postGenerateFunctionality;
    private readonly PostGenerateBaseObjects.Root _postGenerateBaseObjects;
    private readonly Services.MerchantPortal.Credentials.Functionality.PostGenerateFunctionality _postGenerateFunctionalityMerchantPortal;
    private readonly Services.MerchantPortal.Credentials.BaseObjects.PostGenerateBaseObjects.Root _postGenerateBaseObjectsMerchantPortal;
    private readonly PostPgpDecryptFunctionality _postPgpDecryptFunctionality;
    private readonly PostPgpDecryptBaseObjects.Root _postPgpDecryptBaseObjects;
    private readonly DecodeBase64Pgp _decodeBase64Pgp;
    private readonly PostMerchantIdKeysFunctionality _postMerchantIdKeysFunctionality;
    private readonly PostMerchantIdKeysBaseObjects.Root _postMerchantIdKeysBaseObjects;
    private readonly PostConnectTokenFunctionality _postConnectTokenFunctionality;
    private readonly PostConnectTokenBaseObjects.Root _postConnectTokenBaseObjects;
    private readonly InstallmentPlans _installmentPlans;
    private readonly PostRsaSignPayloadFunctionality _postRsaSignPayloadFunctionality;
    private readonly PostRsaSignPayloadBaseObjects.Root _postRsaSignPayloadBaseObjects;
    private readonly StatusToFlagInit _statusToFlagInit;
    private readonly SringifiedJsonConvertor _sringifiedJsonConvertor;
    private readonly PostPreparePayloadFunctionality _postPreparePayloadFunctionality;
    private readonly PostPreparePayloadBaseObjects.Root _postPreparePayloadBaseObjects;
    private readonly PostPlanCreateKeyEncryptServerFunctionality _postPlanCreateKeyEncryptServerFunctionality;
    private readonly CreatePlanV3Kes _createPlanV3Kes;
    private readonly ExtractHeadersController _extractHeadersController;
    private readonly PostVerifyResponseSignature _postVerifyResponseSignature;
    private readonly PostJweDecryptPayloadFunctionality _postJweDecryptPayloadFunctionality;
    private readonly PostJweDecryptPayloadBaseObjects.Root _postJweDecryptPayloadBaseObjects;


    public KeyEncryptionTests()
    {
        Console.WriteLine("\nStaring KeyEncryptionTests Setup");
        _getGenerateAsymmetricFunctionality = new GetGenerateAsymmetricFunctionality();
        _postKeyFunctionality = new PostKeyFunctionality();
        _postOwnerBaseObjects = new PostKeyBaseObjects.Root();
        _postGenerateFunctionality = new PostGenerateFunctionality();
        _postGenerateBaseObjects = new PostGenerateBaseObjects.Root();
        _postGenerateFunctionalityMerchantPortal = new Services.MerchantPortal.Credentials.Functionality.PostGenerateFunctionality();
        _postGenerateBaseObjectsMerchantPortal = new Services.MerchantPortal.Credentials.BaseObjects.PostGenerateBaseObjects.Root();
        _postPgpDecryptFunctionality = new PostPgpDecryptFunctionality();
        _postPgpDecryptBaseObjects = new PostPgpDecryptBaseObjects.Root();
        _decodeBase64Pgp = new DecodeBase64Pgp();
        _postMerchantIdKeysFunctionality = new PostMerchantIdKeysFunctionality();
        _postMerchantIdKeysBaseObjects = new PostMerchantIdKeysBaseObjects.Root();
        _postConnectTokenFunctionality = new PostConnectTokenFunctionality();
        _postConnectTokenBaseObjects = new PostConnectTokenBaseObjects.Root();
        _installmentPlans = new InstallmentPlans();
        _postRsaSignPayloadFunctionality = new PostRsaSignPayloadFunctionality();
        _postRsaSignPayloadBaseObjects = new PostRsaSignPayloadBaseObjects.Root();
        _statusToFlagInit = new StatusToFlagInit();
        _sringifiedJsonConvertor = new SringifiedJsonConvertor();
        _postPreparePayloadFunctionality = new PostPreparePayloadFunctionality();
        _postPreparePayloadBaseObjects = new PostPreparePayloadBaseObjects.Root();
        _postPlanCreateKeyEncryptServerFunctionality = new PostPlanCreateKeyEncryptServerFunctionality();
        _createPlanV3Kes = new CreatePlanV3Kes();
        _extractHeadersController = new ExtractHeadersController();
        _postVerifyResponseSignature = new PostVerifyResponseSignature();
        _postJweDecryptPayloadFunctionality = new PostJweDecryptPayloadFunctionality();
        _postJweDecryptPayloadBaseObjects = new PostJweDecryptPayloadBaseObjects.Root();
        Console.WriteLine("KeyEncryptionTests Setup Succeeded\n");
    }

    [OneTimeSetUp]
    public async Task InitSetUp()
    {
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
        var sendAdminLoginRequest = new SendAdminLoginRequest();
        _requestHeader = await sendAdminLoginRequest.DoAdminLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("ClientSecret")!,
            Environment.GetEnvironmentVariable("SplititMockTerminal")!,
            Environment.GetEnvironmentVariable("clientId")!);
    }

    [Category("KeyEncryptionTests")]
    [Test(Description = "TestValidateCredentialsAreDownloaded with generated merchant"), CancelAfter(120 * 1000), Order(1)]
    public async Task TestValidateCredentialsAreDownloaded()
    {
        try
        {
            const string terminalApiKey = "45027e82-9660-4907-aa86-689c50dae472";
            const string merchantId = "146874";
            const string merchantName = "Automation-fd8f2946-3ce6-4706-88de-bfbc27ebf8ba";
            const string clientId = "API000146874.1746672f97184e13b4deb9185562d5da";
            Console.WriteLine("\nStarting TestValidateCredentialsAreDownloaded");
            var jResponseGetGenerateAsymmetric =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                     _requestHeader!, "Pgp", "4096");
            _postOwnerBaseObjects.OwnerCode = merchantName;
            _postOwnerBaseObjects.Type = "Pgp";
            _postOwnerBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            _postOwnerBaseObjects.PublicKey = jResponseGetGenerateAsymmetric.PublicKey;
            _postOwnerBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            var jResponsePostKey =
                await _postKeyFunctionality.SendPostRequestPostKeyAsync( _requestHeader!,
                    _postOwnerBaseObjects);
            Assert.That(jResponsePostKey.UniqueId, Is.Not.Null);
            Assert.That(!jResponsePostKey.UniqueId.Equals(""));
            _postGenerateBaseObjects.OwnerCode = merchantName;
            _postGenerateBaseObjects.Type = "Pgp";
            _postGenerateBaseObjects.Length = 4096;
            _postGenerateBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postGenerateBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            var jResponsePostGenerate = await _postGenerateFunctionality.SendPostRequestPostGenerateAsync(
                 _requestHeader!, _postGenerateBaseObjects);
            Assert.That(jResponsePostGenerate.Key, Is.Not.Null);
            var pgpKey = _decodeBase64Pgp.DecodeToPgp(jResponseGetGenerateAsymmetric.PublicKey);
            _postMerchantIdKeysBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postMerchantIdKeysBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            _postMerchantIdKeysBaseObjects.Type = "PGP";
            _postMerchantIdKeysBaseObjects.PublicKey = pgpKey;
            var jResponseMerchantIdKeys =
                await _postMerchantIdKeysFunctionality.SendPostRequestPostMerchantIdKeysAsync(
                    _requestHeader!, _postMerchantIdKeysBaseObjects,
                    merchantId);
            Assert.That(jResponseMerchantIdKeys.IsSuccess);
            _postGenerateBaseObjectsMerchantPortal.ClientId = clientId;
            var jResponsePostGenerateMerchantPortal =
                await _postGenerateFunctionalityMerchantPortal.SendPostRequestPostGenerateAsync(
                    _requestHeader!,
                    _postGenerateBaseObjectsMerchantPortal, "yes", int.Parse(merchantId));
            _postPgpDecryptBaseObjects.SenderPublicKey = Environment.GetEnvironmentVariable("SplititPublicKey")!;
            _postPgpDecryptBaseObjects.RecipientPrivateKey = jResponseGetGenerateAsymmetric.PrivateKey;
            _postPgpDecryptBaseObjects.RecipientPassphrase = "sample";
            _postPgpDecryptBaseObjects.EncryptedContent = jResponsePostGenerateMerchantPortal.Secret;
            var jResponsePostPgpDecrypt = await _postPgpDecryptFunctionality.SendPostRequestPostPgpDecryptAsync(
                 _requestHeader!, _postPgpDecryptBaseObjects);
            Assert.That(!jResponsePostPgpDecrypt.Equals(""));
            Assert.That(jResponsePostPgpDecrypt, Is.Not.Null);
            _postConnectTokenBaseObjects.client_id = clientId;
            _postConnectTokenBaseObjects.client_secret = jResponsePostPgpDecrypt;
            _postConnectTokenBaseObjects.grant_type = "client_credentials";
            _postConnectTokenBaseObjects.scope = "api.v1 api.v3";
            var jResponseToken = await _postConnectTokenFunctionality.SendPostRequestPostConnectTokenAsync(
                 _requestHeader!, _postConnectTokenBaseObjects);
            var generatedRequestHeader = new RequestHeader
            {
                apiKey = Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                sessionId = jResponseToken!.access_token
            };
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            await _installmentPlans.CreatePlanAsync(Environment.GetEnvironmentVariable("ApiV3")!,
                generatedRequestHeader,
                "Active", new Random().Next(2, 6), terminalApiKey,
                createPlanDefaultValues);
            Console.WriteLine("TestValidateCredentialsAreDownloaded is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateCredentialsAreDownloaded\n" + exception + "\n");
        }
    }

    [Category("KeyEncryptionTests")]
    [Test(Description = "TestValidateAuthentication with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidateAuthentication()
    {
        try
        {
            const string terminalApiKey = "653dbf96-cfb4-4b67-a462-783e4d2e9230";
            const string merchantName = "Automation-ef9d721d-c82d-4964-afe4-e3821f8cd702";
            const string clientId = "API000146875.e4a7d669b6314557a6b11a9737150194";
            Console.WriteLine("\nStarting TestValidateAuthentication");
            var jResponseGetGenerateAsymmetric =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                    _requestHeader!, "RSA", "4096");
            
            _postOwnerBaseObjects.OwnerCode = merchantName;
            _postOwnerBaseObjects.Type = "RSA";
            _postOwnerBaseObjects.Usage = new List<string> { "Authentication" };
            _postOwnerBaseObjects.PublicKey = jResponseGetGenerateAsymmetric.PublicKey;
            _postOwnerBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postOwnerBaseObjects.RelatedClientId = clientId;
            await Task.Delay(10 * 1000);
            var jResponsePostKey =
                await _postKeyFunctionality.SendPostRequestPostKeyAsync( _requestHeader!,
                    _postOwnerBaseObjects);
            Assert.That(jResponsePostKey.UniqueId, Is.Not.Null);
            Assert.That(!jResponsePostKey.UniqueId.Equals(""));
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues = _statusToFlagInit.StatusToFlagsInit("Active", createPlanDefaultValues);
            createPlanDefaultValues.planData.terminalId = terminalApiKey;
            createPlanDefaultValues.planData.numberOfInstallments = new Random().Next(2, 6);
            var payload = _sringifiedJsonConvertor.ConvertStringToStringified(createPlanDefaultValues);
            _postRsaSignPayloadBaseObjects.Algorithm = "SHA384_PSS";
            _postRsaSignPayloadBaseObjects.Payload = payload;
            _postRsaSignPayloadBaseObjects.PrivateKey = jResponseGetGenerateAsymmetric.PrivateKey;
            _postRsaSignPayloadBaseObjects.Method = "POST";
            _postRsaSignPayloadBaseObjects.Url = "web-api-v3.sandbox.splitit.com/api/installmentplans";
            var jResponseRsaSignPayload =
                await _postRsaSignPayloadFunctionality.SendPostRequestPostRsaSignPayloadAsync(
                    _requestHeader!, _postRsaSignPayloadBaseObjects);
            Assert.That(jResponseRsaSignPayload.Signature, Is.Not.Null);
            Assert.That(!jResponseRsaSignPayload.Signature.Equals(""));
            Console.WriteLine("TestValidateAuthentication is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateAuthentication\n" + exception + "\n");
        }
    }

    [Category("KeyEncryptionTests")]
    [Test(Description = "TestValidateMessageLevelEncryption with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidateMessageLevelEncryption()
    {
        try
        {
            const string terminalApiKey = "c008c6cb-9c5e-49ab-8770-49c4a7d1c4aa";
            const string merchantName = "Automation-d88ea842-064d-4814-914f-11f7608d3627";
            const string clientId = "146872";
            Console.WriteLine("\nStarting TestValidateMessageLevelEncryption");
            var jResponseGetGenerateAsymmetric =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                    _requestHeader!, "RSA", "4096");
            Assert.That(jResponseGetGenerateAsymmetric.PublicKey, Is.Not.Null);
            Assert.That(jResponseGetGenerateAsymmetric.PrivateKey, Is.Not.Null);
            _postGenerateBaseObjects.OwnerCode = merchantName;
            _postGenerateBaseObjects.Type = "RSA";
            _postGenerateBaseObjects.Length = 2048;
            _postGenerateBaseObjects.Usage = new List<string> { "MessageLevelEncryption" };
            _postGenerateBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postGenerateBaseObjects.PgpPassPhrase = "sample";
            var jResponsePostGenerate =
                await _postGenerateFunctionality.SendPostRequestPostGenerateAsync( _requestHeader!,
                    _postGenerateBaseObjects);
            Assert.That(jResponsePostGenerate.UniqueId, Is.Not.Null);
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues = _statusToFlagInit.StatusToFlagsInit("Active", createPlanDefaultValues);
            createPlanDefaultValues.planData.terminalId = terminalApiKey;
            createPlanDefaultValues.planData.numberOfInstallments = new Random().Next(2, 6);
            _postPreparePayloadBaseObjects.KeyUniqueId = jResponsePostGenerate.UniqueId;
            _postPreparePayloadBaseObjects.PlainTextPayload = new PostPreparePayloadBaseObjects.PlainTextPayload
            {
                AutoCapture = createPlanDefaultValues.autoCapture,
                AttemptAuthorize = createPlanDefaultValues.attemptAuthorize,
                Attempt3DSecure = createPlanDefaultValues.attempt3DSecure,
                TermsAndConditionsAccepted = createPlanDefaultValues.termsAndConditionsAccepted,
                Shopper = new PostPreparePayloadBaseObjects.Shopper
                {
                    FullName = createPlanDefaultValues.shopper.fullName,
                    Email = createPlanDefaultValues.shopper.email,
                    PhoneNumber = createPlanDefaultValues.shopper.phoneNumber,
                    Culture = createPlanDefaultValues.shopper.culture
                },
                PlanData = new PostPreparePayloadBaseObjects.PlanData
                {
                    TotalAmount = createPlanDefaultValues.planData.totalAmount,
                    Currency = createPlanDefaultValues.planData.currency,
                    NumberOfInstallments = createPlanDefaultValues.planData.numberOfInstallments,
                    TerminalId = createPlanDefaultValues.planData.terminalId,
                    PurchaseMethod = createPlanDefaultValues.planData.purchaseMethod,
                    RefOrderNumber = createPlanDefaultValues.planData.refOrderNumber,
                    _ExtendedParams = new PostPreparePayloadBaseObjects.ExtendedParams
                    {
                        ThreeDSExemption = "no_preference",
                        force3ds = "true"
                    },
                    _FirstInstallmentAmount = createPlanDefaultValues.planData.FirstInstallmentAmount,
                    FirstInstallmentDate = createPlanDefaultValues.planData.FirstInstallmentDate
                },
                BillingAddress = new PostPreparePayloadBaseObjects.BillingAddress
                {
                    AddressLine1 = createPlanDefaultValues.billingAddress.addressLine1,
                    _AddressLine2 = createPlanDefaultValues.billingAddress.addressLine2,
                    City = createPlanDefaultValues.billingAddress.city,
                    Country = createPlanDefaultValues.billingAddress.country,
                    State = createPlanDefaultValues.billingAddress.state,
                    Zip = createPlanDefaultValues.billingAddress.zip
                },
                PaymentMethod = new PostPreparePayloadBaseObjects.PaymentMethod
                {
                    Type = createPlanDefaultValues.paymentMethod.type,
                    Card = new PostPreparePayloadBaseObjects.Card
                    {
                        CardHolderFullName = createPlanDefaultValues.paymentMethod.card.cardHolderFullName,
                        CardNumber = createPlanDefaultValues.paymentMethod.card.cardNumber,
                        CardExpYear = createPlanDefaultValues.paymentMethod.card.cardExpYear,
                        CardExpMonth = createPlanDefaultValues.paymentMethod.card.cardExpMonth,
                        CardCvv = createPlanDefaultValues.paymentMethod.card.cardCvv
                    }
                },
                RedirectUrls = new PostPreparePayloadBaseObjects.RedirectUrls
                {
                    AuthorizeFailed = createPlanDefaultValues.redirectUrls.authorizeFailed,
                    AuthorizeSucceeded = createPlanDefaultValues.redirectUrls.authorizeSucceeded
                }
            };
            var jResponsePostPreparePayloadBaseObjects =
                await _postPreparePayloadFunctionality.SendPostRequestPostPreparePayloadBaseObjectsAsync(
                    _requestHeader!, _postPreparePayloadBaseObjects);
            Assert.That(jResponsePostPreparePayloadBaseObjects.ExactPayload, Is.Not.Null);
            var jResponse = await _postPlanCreateKeyEncryptServerFunctionality.SendPostPlanCreateKeyEncryptServerAsync(
                 _requestHeader!, jResponsePostPreparePayloadBaseObjects.ExactPayload);
            Assert.That(jResponse!.InstallmentPlanNumber, Is.Not.Null);
            Console.WriteLine("TestValidateMessageLevelEncryption is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMessageLevelEncryption\n" + exception + "\n");
        }
    }

    [Category("KeyEncryptionTests")]
    [Test(Description = "TestValidateMessageLevelEncryptionPlusSignature with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidateMessageLevelEncryptionPlusSignature()
    {
        try
        {
            Console.WriteLine("\nStarting TestValidateMessageLevelEncryption");
            const string terminalApiKey = "cc8f0843-bd53-4571-97df-614a5257aad2";
            const string merchantName = "Automation-f7e2979d-7744-4693-bc6b-b77748a0c075";
            const string clientId = "API000146871.75b1a8e6d9224d12bd2d2a99901fed4c";
            var jResponseGetGenerateAsymmetric =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                    _requestHeader!, "RSA", "4096");
            Assert.That(jResponseGetGenerateAsymmetric.PublicKey, Is.Not.Null);
            Assert.That(jResponseGetGenerateAsymmetric.PrivateKey, Is.Not.Null);
            _postGenerateBaseObjects.OwnerCode = merchantName;
            _postGenerateBaseObjects.Type = "RSA";
            _postGenerateBaseObjects.Length = 2048;
            _postGenerateBaseObjects.Usage = new List<string> { "MessageLevelEncryption" };
            _postGenerateBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postGenerateBaseObjects.PgpPassPhrase = "sample";
            var jResponsePostGenerate =
                await _postGenerateFunctionality.SendPostRequestPostGenerateAsync( _requestHeader!,
                    _postGenerateBaseObjects);
            Assert.That(jResponsePostGenerate.UniqueId, Is.Not.Null);
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues = _statusToFlagInit.StatusToFlagsInit("Active", createPlanDefaultValues);
            createPlanDefaultValues.planData.terminalId = terminalApiKey;
            createPlanDefaultValues.planData.numberOfInstallments = new Random().Next(2, 6);
            _postPreparePayloadBaseObjects.KeyUniqueId = jResponsePostGenerate.UniqueId;
            _postPreparePayloadBaseObjects.PlainTextPayload = new PostPreparePayloadBaseObjects.PlainTextPayload
            {
                AutoCapture = createPlanDefaultValues.autoCapture,
                AttemptAuthorize = createPlanDefaultValues.attemptAuthorize,
                Attempt3DSecure = createPlanDefaultValues.attempt3DSecure,
                TermsAndConditionsAccepted = createPlanDefaultValues.termsAndConditionsAccepted,
                Shopper = new PostPreparePayloadBaseObjects.Shopper
                {
                    FullName = createPlanDefaultValues.shopper.fullName,
                    Email = createPlanDefaultValues.shopper.email,
                    PhoneNumber = createPlanDefaultValues.shopper.phoneNumber,
                    Culture = createPlanDefaultValues.shopper.culture
                },
                PlanData = new PostPreparePayloadBaseObjects.PlanData
                {
                    TotalAmount = createPlanDefaultValues.planData.totalAmount,
                    Currency = createPlanDefaultValues.planData.currency,
                    NumberOfInstallments = createPlanDefaultValues.planData.numberOfInstallments,
                    TerminalId = createPlanDefaultValues.planData.terminalId,
                    PurchaseMethod = createPlanDefaultValues.planData.purchaseMethod,
                    RefOrderNumber = createPlanDefaultValues.planData.refOrderNumber,
                    _ExtendedParams = new PostPreparePayloadBaseObjects.ExtendedParams
                    {
                        ThreeDSExemption = "no_preference",
                        force3ds = "true"
                    },
                    _FirstInstallmentAmount = createPlanDefaultValues.planData.FirstInstallmentAmount,
                    FirstInstallmentDate = createPlanDefaultValues.planData.FirstInstallmentDate
                },
                BillingAddress = new PostPreparePayloadBaseObjects.BillingAddress
                {
                    AddressLine1 = createPlanDefaultValues.billingAddress.addressLine1,
                    _AddressLine2 = createPlanDefaultValues.billingAddress.addressLine2,
                    City = createPlanDefaultValues.billingAddress.city,
                    Country = createPlanDefaultValues.billingAddress.country,
                    State = createPlanDefaultValues.billingAddress.state,
                    Zip = createPlanDefaultValues.billingAddress.zip
                },
                PaymentMethod = new PostPreparePayloadBaseObjects.PaymentMethod
                {
                    Type = createPlanDefaultValues.paymentMethod.type,
                    Card = new PostPreparePayloadBaseObjects.Card
                    {
                        CardHolderFullName = createPlanDefaultValues.paymentMethod.card.cardHolderFullName,
                        CardNumber = createPlanDefaultValues.paymentMethod.card.cardNumber,
                        CardExpYear = createPlanDefaultValues.paymentMethod.card.cardExpYear,
                        CardExpMonth = createPlanDefaultValues.paymentMethod.card.cardExpMonth,
                        CardCvv = createPlanDefaultValues.paymentMethod.card.cardCvv
                    }
                },
                RedirectUrls = new PostPreparePayloadBaseObjects.RedirectUrls
                {
                    AuthorizeFailed = createPlanDefaultValues.redirectUrls.authorizeFailed,
                    AuthorizeSucceeded = createPlanDefaultValues.redirectUrls.authorizeSucceeded
                }
            };
            var jResponsePostPreparePayloadBaseObjects =
                await _postPreparePayloadFunctionality.SendPostRequestPostPreparePayloadBaseObjectsAsync(
                    _requestHeader!, _postPreparePayloadBaseObjects);
            Assert.That(jResponsePostPreparePayloadBaseObjects.ExactPayload, Is.Not.Null);
            _postOwnerBaseObjects.OwnerCode = merchantName;
            _postOwnerBaseObjects.Type = "RSA";
            _postOwnerBaseObjects.Usage = new List<string> { "Authentication" };
            _postOwnerBaseObjects.PublicKey = jResponseGetGenerateAsymmetric.PublicKey;
            _postOwnerBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postOwnerBaseObjects.RelatedClientId = clientId;
            await Task.Delay(10 * 1000);
            var jResponsePostKey =
                await _postKeyFunctionality.SendPostRequestPostKeyAsync( _requestHeader!,
                    _postOwnerBaseObjects);
            Assert.That(jResponsePostKey.UniqueId, Is.Not.Null);
            Assert.That(!jResponsePostKey.UniqueId.Equals(""));
            _postRsaSignPayloadBaseObjects.Algorithm = "SHA384_PSS";
            _postRsaSignPayloadBaseObjects.Payload = jResponsePostPreparePayloadBaseObjects.ExactPayload;
            _postRsaSignPayloadBaseObjects.PrivateKey = jResponseGetGenerateAsymmetric.PrivateKey;
            _postRsaSignPayloadBaseObjects.Method = "POST";
            _postRsaSignPayloadBaseObjects.Url = "web-api-v3.sandbox.splitit.com/api/installmentplans";
            var jResponseRsaSignPayload =
                await _postRsaSignPayloadFunctionality.SendPostRequestPostRsaSignPayloadAsync(
                    _requestHeader!,
                    _postRsaSignPayloadBaseObjects);
            Assert.That(jResponseRsaSignPayload.Signature, Is.Not.Null);
            Console.WriteLine("TestValidateMessageLevelEncryptionPlusSignature is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateMessageLevelEncryptionPlusSignature\n" + exception + "\n");
        }
    }

    [Category("KeyEncryptionTests")]
    [Test(Description = "TestValidateResponseSignature with generated merchant"), CancelAfter(120 * 1000)]
    public async Task TestValidateResponseSignature()
    {
        try
        {
            const string terminalApiKey = "eb905e7c-febe-42a6-bd65-a87e051c5f94";
            const string merchantId = "146876";
            const string merchantName = "Automation-187e288b-a1be-4a69-ac75-b61e5b27c645";
            const string clientId = "API000146876.98dd852cc0c5465a974b8b69b4f71f2e";
            Console.WriteLine("\nStarting TestValidateResponseSignature");
            var jResponseGetGenerateAsymmetric =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                     _requestHeader!, "Pgp", "4096");
            _postOwnerBaseObjects.OwnerCode = merchantName;
            _postOwnerBaseObjects.Type = "RSA";
            _postOwnerBaseObjects.Usage = new List<string> { "ResponseSignature" };
            _postOwnerBaseObjects.PublicKey = jResponseGetGenerateAsymmetric.PublicKey;
            _postOwnerBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postGenerateBaseObjects.OwnerCode = merchantName;
            _postGenerateBaseObjects.Type = "RSA";
            _postGenerateBaseObjects.Length = 2048;
            _postGenerateBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postGenerateBaseObjects.Usage = new List<string> { "ResponseSignature" };
            _postGenerateBaseObjects.RelatedClientId = clientId;
            var jResponsePostGenerate = await _postGenerateFunctionality.SendPostRequestPostGenerateAsync(
                 _requestHeader!, _postGenerateBaseObjects);
            Assert.That(jResponsePostGenerate.Key, Is.Not.Null);
            var pgpKey = _decodeBase64Pgp.DecodeToPgp(jResponseGetGenerateAsymmetric.PublicKey);
            _postMerchantIdKeysBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postMerchantIdKeysBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            _postMerchantIdKeysBaseObjects.Type = "Pgp";
            _postMerchantIdKeysBaseObjects.PublicKey = pgpKey;
            var jResponseMerchantIdKeys =
                await _postMerchantIdKeysFunctionality.SendPostRequestPostMerchantIdKeysAsync(
                    _requestHeader!, _postMerchantIdKeysBaseObjects,
                    merchantId);
            Assert.That(jResponseMerchantIdKeys.IsSuccess);
            _postGenerateBaseObjectsMerchantPortal.ClientId = clientId;
            var jResponsePostGenerateMerchantPortal =
                await _postGenerateFunctionalityMerchantPortal.SendPostRequestPostGenerateAsync(
                    _requestHeader!,
                    _postGenerateBaseObjectsMerchantPortal, "yes", int.Parse(merchantId));
            _postPgpDecryptBaseObjects.SenderPublicKey = Environment.GetEnvironmentVariable("SplititPublicKey")!;
            _postPgpDecryptBaseObjects.RecipientPrivateKey = jResponseGetGenerateAsymmetric.PrivateKey;
            _postPgpDecryptBaseObjects.RecipientPassphrase = "sample";
            _postPgpDecryptBaseObjects.EncryptedContent = jResponsePostGenerateMerchantPortal.Secret;
            var jResponsePostPgpDecrypt = await _postPgpDecryptFunctionality.SendPostRequestPostPgpDecryptAsync(
                 _requestHeader!, _postPgpDecryptBaseObjects);
            Assert.That(!jResponsePostPgpDecrypt.Equals(""));
            Assert.That(jResponsePostPgpDecrypt, Is.Not.Null);
            _postConnectTokenBaseObjects.client_id = clientId;
            _postConnectTokenBaseObjects.client_secret = jResponsePostPgpDecrypt;
            _postConnectTokenBaseObjects.grant_type = "client_credentials";
            _postConnectTokenBaseObjects.scope = "api.v1 api.v3 keyexchange.api";
            var jResponseToken = await _postConnectTokenFunctionality.SendPostRequestPostConnectTokenAsync(
                 _requestHeader!, _postConnectTokenBaseObjects);
            var generatedRequestHeader = new RequestHeader
            {
                apiKey = Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                sessionId = jResponseToken!.access_token
            };
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var (stringBodyResponse, response) = await _createPlanV3Kes.CreatePlanKesAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, generatedRequestHeader,
                "Active", createPlanDefaultValues.planData.numberOfInstallments, terminalApiKey,
                createPlanDefaultValues);
            var (responseSignature, responseSignatureKeyId, responseSignatureToken) =
                _extractHeadersController.ExtractHeaders(response);
            Assert.That(responseSignatureKeyId, Is.EqualTo(jResponsePostGenerate.UniqueId));
            var verifyResponse = _postVerifyResponseSignature.VerifyResponseSignature(stringBodyResponse,
                responseSignature!, jResponsePostGenerate.Key, responseSignatureToken!);
            Assert.That(verifyResponse);
            Console.WriteLine("TestValidateResponseSignature is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in TestValidateResponseSignature\n" + exception + "\n");
        }
    }

    [Category("KeyEncryptionTests")]
    [Test(Description = "TestValidateResponseEncryption with generated merchant"), CancelAfter(120 * 1000), Order(2)]
    public async Task TestValidateResponseEncryption()
    {
        try
        {
            Console.WriteLine("Starting TestValidateResponseEncryption");
            var terminalApiKey = "2fd8a17b-ca31-4517-9db1-033dbc1448af";
            var merchantId = "146877";
            var merchantName = "Automation-342d19cf-4460-41d9-b4da-488ccd9234a2";
            var clientId = "API000146877.28dd063a3cd6480b85b964606c050d6f";
            Console.WriteLine("\nStarting TestValidateMessageLevelEncryption");
            var jResponseGetGenerateAsymmetric =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                     _requestHeader!, "RSA", "2048");
            Assert.That(jResponseGetGenerateAsymmetric.PublicKey, Is.Not.Null);
            Assert.That(jResponseGetGenerateAsymmetric.PrivateKey, Is.Not.Null);
            _postOwnerBaseObjects.OwnerCode = merchantName;
            _postOwnerBaseObjects.Type = "RSA";
            _postOwnerBaseObjects.Usage = new List<string> { "ResponseEncryption" };
            _postOwnerBaseObjects.PublicKey = jResponseGetGenerateAsymmetric.PublicKey;
            _postOwnerBaseObjects.RelatedClientId = clientId;
            _postOwnerBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            var jResponsePostKey =
                await _postKeyFunctionality.SendPostRequestPostKeyAsync( _requestHeader!,
                    _postOwnerBaseObjects);
            Assert.That(jResponsePostKey.UniqueId, Is.Not.Null);
            Assert.That(!jResponsePostKey.UniqueId.Equals(""));
            var jResponseGetGenerateAsymmetricPgp =
                await _getGenerateAsymmetricFunctionality.SendGetRequestGetGenerateAsymmetricAsync(
                    _requestHeader!, "Pgp", "4096");
            _postOwnerBaseObjects.OwnerCode = merchantName;
            _postOwnerBaseObjects.Type = "Pgp";
            _postOwnerBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            _postOwnerBaseObjects.PublicKey = jResponseGetGenerateAsymmetricPgp.PublicKey;
            _postOwnerBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            var jResponsePostKeyNew =
                await _postKeyFunctionality.SendPostRequestPostKeyAsync( _requestHeader!,
                    _postOwnerBaseObjects);
            Assert.That(jResponsePostKeyNew.UniqueId, Is.Not.Null);
            Assert.That(!jResponsePostKeyNew.UniqueId.Equals(""));
            _postGenerateBaseObjects.OwnerCode = merchantName;
            _postGenerateBaseObjects.Type = "Pgp";
            _postGenerateBaseObjects.Length = 4096;
            _postGenerateBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postGenerateBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            var jResponsePostGenerate = await _postGenerateFunctionality.SendPostRequestPostGenerateAsync(
                 _requestHeader!, _postGenerateBaseObjects);
            Assert.That(jResponsePostGenerate.Key, Is.Not.Null);
            var pgpKey = _decodeBase64Pgp.DecodeToPgp(jResponseGetGenerateAsymmetricPgp.PublicKey);
            _postMerchantIdKeysBaseObjects.ExpirationUtc = DateTime.Now.AddMonths(1);
            _postMerchantIdKeysBaseObjects.Usage = new List<string> { "CredentialsDownload" };
            _postMerchantIdKeysBaseObjects.Type = "PGP";
            _postMerchantIdKeysBaseObjects.PublicKey = pgpKey;
            var jResponseMerchantIdKeys =
                await _postMerchantIdKeysFunctionality.SendPostRequestPostMerchantIdKeysAsync(
                    _requestHeader!, _postMerchantIdKeysBaseObjects,
                    merchantId);
            Assert.That(jResponseMerchantIdKeys.IsSuccess);
            _postGenerateBaseObjectsMerchantPortal.ClientId = clientId;
            var jResponsePostGenerateMerchantPortal =
                await _postGenerateFunctionalityMerchantPortal.SendPostRequestPostGenerateAsync(
                    _requestHeader!,
                    _postGenerateBaseObjectsMerchantPortal, "yes", int.Parse(merchantId));
            _postPgpDecryptBaseObjects.SenderPublicKey = Environment.GetEnvironmentVariable("SplititPublicKey")!;
            _postPgpDecryptBaseObjects.RecipientPrivateKey = jResponseGetGenerateAsymmetricPgp.PrivateKey;
            _postPgpDecryptBaseObjects.RecipientPassphrase = "sample";
            _postPgpDecryptBaseObjects.EncryptedContent = jResponsePostGenerateMerchantPortal.Secret;
            var jResponsePostPgpDecrypt = await _postPgpDecryptFunctionality.SendPostRequestPostPgpDecryptAsync(
                 _requestHeader!, _postPgpDecryptBaseObjects);
            Assert.That(!jResponsePostPgpDecrypt.Equals(""));
            Assert.That(jResponsePostPgpDecrypt, Is.Not.Null);
            _postConnectTokenBaseObjects.client_id = clientId;
            _postConnectTokenBaseObjects.client_secret = jResponsePostPgpDecrypt;
            _postConnectTokenBaseObjects.grant_type = "client_credentials";
            _postConnectTokenBaseObjects.scope = "api.v1 api.v3";
            var jResponseToken = await _postConnectTokenFunctionality.SendPostRequestPostConnectTokenAsync(
                 _requestHeader!, _postConnectTokenBaseObjects);
            var generatedRequestHeader = new RequestHeader
            {
                apiKey = Environment.GetEnvironmentVariable("SplititMockTerminal")!,
                sessionId = jResponseToken!.access_token
            };
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            var json = await _createPlanV3Kes.CreatePlanKesAsyncReturnString(Environment.GetEnvironmentVariable("ApiV3")!,
                generatedRequestHeader,
                "Active", new Random().Next(2, 6), terminalApiKey,
                createPlanDefaultValues);
            Assert.That(json, Is.Not.Null);
            _postJweDecryptPayloadBaseObjects.PrivateKey = jResponseGetGenerateAsymmetric.PrivateKey;
            _postJweDecryptPayloadBaseObjects.JwePayload = json;
            var jResponse = await _postJweDecryptPayloadFunctionality.SendPostRequestPostJweDecryptPayloadAsync(
                 _requestHeader!, _postJweDecryptPayloadBaseObjects);
            Assert.That(jResponse, Is.Not.Null);
            Console.WriteLine("Done with TestValidateResponseEncryption");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in TestValidateResponseEncryption" + e);
            throw;
        }
    }
}