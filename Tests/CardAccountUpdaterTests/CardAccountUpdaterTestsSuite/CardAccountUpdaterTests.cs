using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.AdminApi.JobsMng.Functionality;
using Splitit.Automation.NG.Backend.Services.CardAccountUpdater.BaseObjects;
using Splitit.Automation.NG.Backend.Services.CardAccountUpdater.Functionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Tests.CardAccountUpdaterTests.CardAccountUpdaterTestsSuite;

[TestFixture]
[AllureNUnit]
[AllureSuite("CardAccountUpdaterTests")]
[AllureDisplayIgnored]
[Parallelizable(ParallelScope.All)]
public class CardAccountUpdaterTests
{
    private RequestHeader? _requestHeader;
    private readonly InstallmentPlanNumberCancel _installmentPlanNumberCancel;
    private readonly TestsHelper.TestsHelper _testsHelper;
    private readonly ReAuthFunctionality _reAuthFunctionality;
    private readonly PostJobRunFunctionality _postJobRunFunctionality;
    private readonly DeleteCardsFunctionality _deleteCardsFunctionality;
    private readonly DeleteCardsBaseObjects.Root _deleteCardsBaseObjects;
    private readonly EnvConfig _envConfig;

    public CardAccountUpdaterTests()
    {
        Console.WriteLine("Starting Setup");
        _installmentPlanNumberCancel = new InstallmentPlanNumberCancel();
        _testsHelper = new TestsHelper.TestsHelper();
        _reAuthFunctionality = new ReAuthFunctionality();
        _postJobRunFunctionality = new PostJobRunFunctionality();
        _deleteCardsFunctionality = new DeleteCardsFunctionality();
        _deleteCardsBaseObjects = new DeleteCardsBaseObjects.Root();
        _envConfig = new EnvConfig();
        Console.WriteLine("Done with Setup\n");
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
    
    // [TestCase(Category = "CardAccountUpdaterTests")]
    // [Test(Description = "TestValidateCardAccountUpdaterBulk")]
    // public async Task TestValidateCardAccountUpdaterBulk()
    // {
    //     try
    //     {
    //         Console.WriteLine("\nStarting TestValidateCardAccountUpdaterBulk");
    //         var testsCardsDict = _testsHelper.ReturnTestsCardsDict();
    //         Console.WriteLine("Starting to create plans");
    //         var ipn1 = await _testsHelper.PlansCreator( _requestHeader!, "4539097887163333");
    //         var ipn2 = await _testsHelper.PlansCreator( _requestHeader!, "5325191087030619");
    //         var ipn3 = await _testsHelper.PlansCreator( _requestHeader!, "4012888888881881");
    //         var ipn5 = await _testsHelper.PlansCreator( _requestHeader!, "5412000000001002");
    //         var ipn6 = await _testsHelper.PlansCreator( _requestHeader!, "5412000000001003");
    //         Console.WriteLine("Done with create plans");
    //         Console.WriteLine("Starting job runner");
    //         var jResponseJobsRunner = await _postJobRunFunctionality.SendPostRequestForPostJobRunAsync(
    //              _requestHeader!, "CardAccountUpdaterJob", 
    //             ipn1 + "," + ipn2 + ","+ ipn3 + "," + ipn5 + "," + ipn6);
    //         Assert.That(jResponseJobsRunner.IsSuccess);
    //         Console.WriteLine("Done with job runner");
    //         Console.WriteLine("Starting audit log process");
    //         Assert.That(await _testsHelper.ValidateAuditLogLogs( _requestHeader!, ipn1));
    //         Assert.That(await _testsHelper.ValidateAuditLogLogs( _requestHeader!, ipn2));
    //         Assert.That(await _testsHelper.ValidateAuditLogLogs( _requestHeader!, ipn3));
    //         Assert.That(await _testsHelper.ValidateAuditLogLogs( _requestHeader!, ipn5));
    //         Assert.That(await _testsHelper.ValidateAuditLogLogs( _requestHeader!, ipn6));
    //         Console.WriteLine("Done with audit log process");
    //         Console.WriteLine("Starting Reauth for all the plans");
    //         var jResponseReAuth1 =
    //             await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, ipn1);
    //         Assert.That(jResponseReAuth1.ResponseHeader.Succeeded);
    //         Console.WriteLine("Validating bin ,last 4 digits and exp for ipn -> " + ipn1);
    //         var bin = testsCardsDict["4539097887163333"][..6];
    //         var last4Digits = testsCardsDict["4539097887163333"].Substring(12, 4);
    //         Assert.That(jResponseReAuth1.InstallmentPlan.ActiveCard.Bin == bin);
    //         Assert.That(jResponseReAuth1.InstallmentPlan.ActiveCard.CardNumber.Contains(last4Digits));
    //         Assert.That(jResponseReAuth1.InstallmentPlan.ActiveCard.CardExpMonth.Equals("12"));
    //         Assert.That(jResponseReAuth1.InstallmentPlan.ActiveCard.CardExpYear.Equals("2026"));
    //         Console.WriteLine("Validated bin ,last 4 digits and exp for ipn -> " + ipn1);
    //         var jResponseReAuth2 =
    //             await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, ipn2);
    //         Assert.That(jResponseReAuth2.ResponseHeader.Succeeded);
    //         Console.WriteLine("Validating bin ,last 4 digits and exp for ipn -> " + ipn2);
    //         bin = testsCardsDict["5325191087030619"][..6];
    //         last4Digits = testsCardsDict["5325191087030619"].Substring(12, 4);
    //         Assert.That(jResponseReAuth2.InstallmentPlan.ActiveCard.Bin == bin);
    //         Assert.That(jResponseReAuth2.InstallmentPlan.ActiveCard.CardNumber.Contains(last4Digits));
    //         Assert.That(jResponseReAuth2.InstallmentPlan.ActiveCard.CardExpMonth.Equals("12"));
    //         Assert.That(jResponseReAuth2.InstallmentPlan.ActiveCard.CardExpYear.Equals("2026"));
    //         Console.WriteLine("Validated bin ,last 4 digits and exp for ipn -> " + ipn2);
    //         var jResponseReAuth3 =
    //             await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, ipn3);
    //         Assert.That(jResponseReAuth3.ResponseHeader.Succeeded);
    //         Console.WriteLine("Validating bin ,last 4 digits and exp for ipn -> " + ipn3);
    //         bin = testsCardsDict["4012888888881881"][..6];
    //         last4Digits = testsCardsDict["4012888888881881"].Substring(12, 4);
    //         Assert.That(jResponseReAuth3.InstallmentPlan.ActiveCard.Bin == bin);
    //         Assert.That(jResponseReAuth3.InstallmentPlan.ActiveCard.CardNumber.Contains(last4Digits));
    //         Assert.That(jResponseReAuth3.InstallmentPlan.ActiveCard.CardExpMonth.Equals("12"));
    //         Assert.That(jResponseReAuth3.InstallmentPlan.ActiveCard.CardExpYear.Equals("2026"));
    //         Console.WriteLine("Validated bin ,last 4 digits and exp for ipn -> " + ipn3);
    //         var jResponseReAuth5 =
    //             await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, ipn5);
    //         Assert.That(jResponseReAuth5.ResponseHeader.Succeeded);
    //         Console.WriteLine("Validating bin ,last 4 digits and exp for ipn -> " + ipn5);
    //         bin = testsCardsDict["5412000000001002"][..6];
    //         last4Digits = testsCardsDict["5412000000001002"].Substring(12, 4);
    //         Assert.That(jResponseReAuth5.InstallmentPlan.ActiveCard.Bin == bin);
    //         Assert.That(jResponseReAuth5.InstallmentPlan.ActiveCard.CardNumber.Contains(last4Digits));
    //         Assert.That(jResponseReAuth5.InstallmentPlan.ActiveCard.CardExpMonth.Equals("3"));
    //         Assert.That(jResponseReAuth5.InstallmentPlan.ActiveCard.CardExpYear.Equals("2030"));
    //         Console.WriteLine("Validated bin ,last 4 digits and exp for ipn -> " + ipn5);
    //         var jResponseReAuth6 =
    //             await _reAuthFunctionality.SendPostRequestReAuthAsync( _requestHeader!, ipn6);
    //         Assert.That(jResponseReAuth6.ResponseHeader.Succeeded);
    //         Console.WriteLine("Validating bin ,last 4 digits and exp for ipn -> " + ipn6);
    //         bin = testsCardsDict["5412000000001003"][..6];
    //         last4Digits = testsCardsDict["5412000000001003"].Substring(12, 4);
    //         Assert.That(jResponseReAuth6.InstallmentPlan.ActiveCard.Bin == bin);
    //         Assert.That(jResponseReAuth6.InstallmentPlan.ActiveCard.CardNumber.Contains(last4Digits));
    //         Assert.That(jResponseReAuth6.InstallmentPlan.ActiveCard.CardExpMonth.Equals("12"));
    //         Assert.That(jResponseReAuth6.InstallmentPlan.ActiveCard.CardExpYear.Equals("2026"));
    //         Console.WriteLine("Validated bin ,last 4 digits and exp for ipn -> " + ipn6);
    //         Console.WriteLine("Done with Reauth for all the plans");
    //         Console.WriteLine("Starting to cancel all of the plans");
    //         var jResponseCancel1 = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
    //              _requestHeader!, ipn1, 0, "NoRefunds");
    //         Assert.That(jResponseCancel1.ResponseHeader.Succeeded);
    //         var jResponseCancel2 = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
    //              _requestHeader!, ipn2, 0, "NoRefunds");
    //         Assert.That(jResponseCancel2.ResponseHeader.Succeeded);
    //         var jResponseCancel3 = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
    //              _requestHeader!, ipn3, 0, "NoRefunds");
    //         Assert.That(jResponseCancel3.ResponseHeader.Succeeded);
    //         var jResponseCancel5 = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
    //              _requestHeader!, ipn5, 0, "NoRefunds");
    //         Assert.That(jResponseCancel5.ResponseHeader.Succeeded);
    //         var jResponseCancel6 = await _installmentPlanNumberCancel.SendCancelPlanRequestAsync(
    //              _requestHeader!, ipn6, 0, "NoRefunds");
    //         Assert.That(jResponseCancel6.ResponseHeader.Succeeded);
    //         Console.WriteLine("Done with canceling all of the plans");
    //         Console.WriteLine("Starting with Deleting the jobs");
    //         _deleteCardsBaseObjects.CreditCardNumbers = new List<string> { "4539097887163333", 
    //             "5325191087030619", "4012888888881881", "4025000000001005", "5412000000001002", "5412000000001003"};
    //         var response = await _deleteCardsFunctionality.SendDeleteRequestDeleteCards(
    //              _requestHeader!, _deleteCardsBaseObjects);
    //         Assert.NotNull(response);
    //         Console.WriteLine("Done with Deleting the jobs");
    //         Console.WriteLine("TestValidateCardAccountUpdaterBulk is Done\n");
    //     }
    //     catch (Exception exception)
    //     {
    //         Assert.Fail("Error in TestValidateCardAccountUpdaterBulk \n" + exception + "\n");
    //     }
    // }
}