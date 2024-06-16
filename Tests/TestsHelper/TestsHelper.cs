using LumenWorks.Framework.IO.Csv;
using MongoDB.Driver;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Tests.TestsHelper;

public class TestsHelper
{
    private readonly InstallmentPlans _installmentPlans = new();
    private readonly AuditLogController _auditLogController = new();
    private readonly MongoHandler _mongoHandler = new();
    
    public class SettlementReportValues
    {
        public string Merchant { get; set; } = null!;
        public string PlanNumber { get; set; } = null!;
        public string NumberOfInstallments { get; set; } = null!;
        public string PaymentType { get; set; } = null!;
        public string OriginalPlanAmount { get; set; } = null!;
        public string TransactionAmount { get; set; } = null!;
        public string ExchangeRate { get; set; } = null!;
        public string GrossSettlementAmount { get; set; } = null!;
        public string VariableFee { get; set; } = null!;
        public string FixedFee { get; set; } = null!;
        public string NetSettlementAmount { get; set; } = null!;
    }

    public class SettlementReportSummeryValues
    {
        public string Merchant { get; set; } = null!;
        public string SettlementDate { get; set; } = null!;
        public string TotalLineItems { get; set; } = null!;
        public string GrossSettlementAmount { get; set; } = null!;
        public string TotalFees { get; set; } = null!;
        public string Tax { get; set; } = null!;
        public string NetSettlementAmount { get; set; } = null!;
        public string InvoiceId { get; set; } = null!;
    }

    public Dictionary<string, string> ReturnTestsCardsDict()
    {
        var cardsDict = new Dictionary<string, string>
        {
            ["4539097887163333"] = "4539097887163333",
            ["5325191087030619"] = "5325191087030619",
            ["4012888888881881"] = "4212345678910006",
            ["4025000000001005"] = "4025000000001005",
            ["5412000000001002"] = "5412000000001102",
            ["5412000000001003"] = "5412000000001103"
        };
        return cardsDict;
    }
    
    public async Task<bool?> ValidateAllInstalmentsAreBiggerThenZero(List<ResponseFullPlanInfoIpn.Installment> instalments)
    {
        try
        {
            Console.WriteLine("trying to validate all installments are bigger then 0");
            foreach (var instalment in instalments)
            {
                Assert.That(instalment.Amount.Value > 0.0);
            }
            Console.WriteLine("All installments are greater then 0");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateAllInstalmentsAreBiggerThenZero" + exception);
            throw;
        }
    }

    public async Task<List<string>> ReturnSettlementReportColumns(string reportType)
    {
        switch (reportType)
        {
            case "SettlementReport":
                var reportListColumnsSetRep = new List<string>
                {
                    "merchant", "business_unit", "business_unit_id", "plan_number",
                    "order_number", "number_of_installments", "plan_created_date", "activity_date",
                    "ref_transaction_id", "payment_type",
                    "installment_number", "plan_currency", "original_plan_amount", "transaction_amount",
                    "exchange_rate", "exchange_rate_date",
                    "settlement_currency", "gross_settlement_amount", "variable_fee", "fixed_fee",
                    "net_settlement_amount", "settlement_date", "invoice_id", "shopper_name", "shopper_email",
                    "card_brand", "card_type"
                };
                return reportListColumnsSetRep;
            case "SettlementReportSummery":
                var reportListColumnsSetRepSum = new List<string>
                {
                    "merchant","settlement_date","total_line_items","gross_settlement_amount",
                    "total_fees","tax","net_settlement_amount","invoice_id"
                };
                return reportListColumnsSetRepSum;
            case "SettlementReportGrossDebit":
                var reportListColumnsDebitGross = new List<string>
                {
                    "merchant", "business_unit", "business_unit_id", "plan_number",
                    "order_number", "number_of_installments", "plan_created_date", "activity_date",
                    "ref_transaction_id", "payment_type",
                    "installment_number", "plan_currency", "original_plan_amount", "transaction_amount",
                    "exchange_rate", "exchange_rate_date",
                    "settlement_currency", "gross_settlement_amount", "variable_fee", "fixed_fee",
                    "net_settlement_amount", "settlement_date", "invoice_id", "shopper_name", "shopper_email",
                    "card_brand", "card_type"
                };
                return reportListColumnsDebitGross;
        }
        return null!;
    }

    public Dictionary<string, string> ReturnSplititActivityMerchantFacingMapping()
    {
        var dictMapping = new Dictionary<string, string>
        {
            { "FND", "New Plan" },
            { "COL", "Installment Collection" },
            { "REF", "Refund" },
            { "RED", "Refund" },
            { "CHB", "Dispute" },
            { "RPR", "DisputeReverse" },
            { "AIP", "Collection Adjustment" },
            { "ADP", "Credit Adjustment" },
            { "WRF", "Write-off" },
            { "WRR", "Write-off Reversal" }
        };
        return dictMapping;
    }

    public async Task<List<SettlementReportValues>> ConvertCsvToListDict(string pathToReport, string ipn, 
        string invoiceId = default!)
    {
        List<SettlementReportValues> settlementReportValuesList = new();
        List<SettlementReportSummeryValues> settlementReportSummeryValuesList = new();
        
        try
        {
            Console.WriteLine("Starting ExtractRelevantLinesFromReport");
            var csv = new CachedCsvReader(new StreamReader(pathToReport), true);
            foreach (var line in csv.Where(line => !line[0].Contains("Merchant")))
            {
                if (line[3].Equals(ipn))
                {
                    PopulateSettlementReportValuesObject(line, settlementReportValuesList);
                }
            }
            Console.WriteLine("Done with ExtractRelevantLinesFromReport");
            return settlementReportValuesList;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ExtractRelevantLinesFromReport" + e);
            throw;
        }
    }
    
    public async Task<List<SettlementReportSummeryValues>> ConvertCsvToListDictSummery(string pathToReport, string invoiceId)
    {
        List<SettlementReportSummeryValues> settlementReportSummeryValuesList = new();
        
        try
        {
            Console.WriteLine("Starting ExtractRelevantLinesFromReport");
            var csv = new CachedCsvReader(new StreamReader(pathToReport), true);
            foreach (var line in csv.Where(line => !line[0].Contains("Merchant")))
            {
                if (line[7].Equals(invoiceId))
                {
                    PopulateSettlementReportSummeryValuesObject(line, settlementReportSummeryValuesList);
                }
            }
            Console.WriteLine("Done with ExtractRelevantLinesFromReport");
            return settlementReportSummeryValuesList;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ExtractRelevantLinesFromReport" + e);
            throw;
        }
    }

    private async Task PopulateSettlementReportSummeryValuesObject(string[] line, List<SettlementReportSummeryValues>
        settlementReportSummeryValuesList)
    {
        var settlementReportSummeryValues = new SettlementReportSummeryValues();
        try
        {
            Console.WriteLine("Starting PopulateSettlementReportSummeryValuesObject");
            settlementReportSummeryValues.Merchant = line[0];
            settlementReportSummeryValues.SettlementDate = line[1];
            settlementReportSummeryValues.TotalLineItems = line[2];
            settlementReportSummeryValues.GrossSettlementAmount = line[3];
            settlementReportSummeryValues.TotalFees = line[4];
            settlementReportSummeryValues.Tax = line[5];
            settlementReportSummeryValues.NetSettlementAmount = line[6];
            settlementReportSummeryValues.InvoiceId = line[7];
            settlementReportSummeryValuesList.Add(settlementReportSummeryValues);
            Console.WriteLine("Done with PopulateSettlementReportSummeryValuesObject");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in PopulateSettlementReportSummeryValuesObject" + e);
            throw;
        }
    }

    private async Task PopulateSettlementReportValuesObject(string[] line, List<SettlementReportValues> settlementReportValuesList)
    {
        var settlementReportValues = new SettlementReportValues();
        
        try
        {
            Console.WriteLine("Starting PopulateSettlementReportValuesObject");
            settlementReportValues.Merchant = line[0];
            settlementReportValues.PlanNumber = line[3];
            settlementReportValues.NumberOfInstallments = line[5];
            settlementReportValues.PaymentType = line[9];
            settlementReportValues.OriginalPlanAmount = line[12];
            settlementReportValues.TransactionAmount = line[13];
            settlementReportValues.ExchangeRate = line[14];
            settlementReportValues.GrossSettlementAmount = line[17];
            settlementReportValues.VariableFee = line[18];
            settlementReportValues.FixedFee = line[19];
            settlementReportValues.NetSettlementAmount = line[20];
            settlementReportValuesList.Add(settlementReportValues);
            Console.WriteLine("Done with PopulateSettlementReportValuesObject");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in PopulateSettlementReportValuesObject" + e);
            throw;
        }
    }

    public async Task<bool?> ValidateActivityValues(List<SettlementReportValues> settlementReportValuesList, 
        Dictionary<string, string> splititActivityMerchantFacingList, List<string> activityList)
    {
        Console.WriteLine("Starting ValidateActivityValues");
        var counter = 0;
        foreach (var activity in activityList)
        {
            var activityMerchantFacing = splititActivityMerchantFacingList[activity];
            foreach (var obj in settlementReportValuesList)
            {
                if (obj.PaymentType.Equals(activityMerchantFacing))
                {
                    counter++;
                }
            }   
        }

        Console.WriteLine("Done with ValidateActivityValues");
        return counter == activityList.Count;
    }

    public async Task<bool?> ValidateAmount(double totalAmount, List<SettlementReportValues> settlementReportValuesList)
    {
        Console.WriteLine("Validating report total amount");
        return settlementReportValuesList.All(obj => !(Math.Abs(double.Parse(obj.OriginalPlanAmount) - totalAmount) > 0.1));
    }

    public async Task<bool?> ValidateGrossSettlementAmount(List<SettlementReportValues> settlementReportValuesList,
        Dictionary<string, string> splititActivityMerchantFacingList, Dictionary<string, string> signDict)
    {
        Console.WriteLine("Starting ValidateGrossSettlementAmount");
        foreach (var activity in signDict.Keys)
        {
            var activityFacing = splititActivityMerchantFacingList[activity];
            foreach (var obj in settlementReportValuesList)
            {
                if (obj.PaymentType.Equals(activityFacing))
                {
                    if (signDict[activity].Equals("NEGATIVE"))
                    {
                        if (double.Parse(obj.GrossSettlementAmount) > 0)
                        {
                            Console.WriteLine("GrossSettlementAmount is not NEGATIVE as it should be");
                            return false;
                        }
                    }
                    if (signDict[activity].Equals("POSITIVE"))
                    {
                        if (double.Parse(obj.GrossSettlementAmount) < 0)
                        {
                            Console.WriteLine("GrossSettlementAmount is not POSITIVE as it should be");
                            return false;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Done with ValidateGrossSettlementAmount");
        return true;
    }

    public async Task<bool?> ValidateRefundAmountInReport(List<SettlementReportValues> settlementReportValuesList, int refundAmount,
        Dictionary<string, string> splititActivityMerchantFacingList, Dictionary<string, string> signDict)
    {
        Console.WriteLine("Starting ValidateRefundAmountInReport");
        foreach (var activity in signDict.Keys)
        {
            var activityFacing = splititActivityMerchantFacingList[activity];
            foreach (var obj in settlementReportValuesList)
            {
                if (obj.PaymentType.Equals(activityFacing))
                {
                    if (Math.Abs(double.Parse(obj.TransactionAmount) - refundAmount) < 0.1)
                    {
                        Console.WriteLine("Done with ValidateRefundAmountInReport with TRUE");
                        return true;
                    }

                    Console.WriteLine("Done with ValidateRefundAmountInReport with FALSE");
                    return false;
                }
            }
        }
        Console.WriteLine("Done with ValidateRefundAmountInReport with FALSE");
        return false;
    }

    public async Task<bool?> ValidateFeesCalculationAmounts(List<SettlementReportValues> settlementReportValuesList)
    {
        try
        {
            Console.WriteLine("Starting ValidateFeesCalculationAmounts");
            foreach (var line in settlementReportValuesList)
            {
                var feesSum = double.Parse(line.FixedFee) + double.Parse(line.VariableFee);
                var expectedAmount = double.Parse(line.GrossSettlementAmount) - feesSum;
                Assert.That(Math.Abs(expectedAmount - double.Parse(line.NetSettlementAmount)) < 0.1);
            }
            Console.WriteLine("Done with ValidateFeesCalculationAmounts");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateFeesCalculationAmounts" + e);
            throw;
        }
    }

    public bool ValidateSettlementAmounts(List<SettlementReportValues> settlementReportValuesList, 
        List<SettlementReportSummeryValues> settlementReportSumValuesList)
    {
        var sumGross = 0.0;
        var sumNet = 0.0;
        var fees = 0.0;
        var sumGrossSummery = 0.0;
        var sumNetSummery = 0.0;
        
        try
        {
            Console.WriteLine("Starting ValidateSettlementAmounts");
            foreach (var item in settlementReportValuesList)
            {
                sumGross += double.Parse(item.GrossSettlementAmount);
                sumNet += double.Parse(item.NetSettlementAmount);
                fees += double.Parse(item.FixedFee);
                fees += double.Parse(item.VariableFee);
            }

            foreach (var item in settlementReportSumValuesList)
            {
                sumGrossSummery += double.Parse(item.GrossSettlementAmount);
                sumNetSummery += double.Parse(item.NetSettlementAmount);
            }

            sumNetSummery -= fees;
            Console.WriteLine("Validating GrossSettlementAmount");
            Assert.That(Math.Abs(sumGross - sumGrossSummery) < 0.1);
            Console.WriteLine("GrossSettlementAmount Validated!");
            Console.WriteLine("Validating NetSettlementAmount");
            Assert.That(Math.Abs(sumNet - sumNetSummery) < 0.1);
            Console.WriteLine("NetSettlementAmount Validated!");
            Console.WriteLine("Done with ValidateSettlementAmounts");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateSettlementAmounts" + e);
            throw;
        }
    }

    public async Task<string> PlansCreator(RequestHeader requestHeader,
        string creditCardNumber)
    {
        try
        {
            Console.WriteLine("Starting PlansCreator");
            var createPlanDefaultValues = new CreatePlanDefaultValues();
            var is3DsVarsCreation = new Is3DsVarsCreation();
            createPlanDefaultValues = is3DsVarsCreation.Create3DsVars(createPlanDefaultValues, "no");
            createPlanDefaultValues.paymentMethod.card = new CardDefaultValues
            {
                cardNumber = creditCardNumber
            };
            var json = await _installmentPlans.CreatePlanAsync(
                Environment.GetEnvironmentVariable("ApiV3")!, requestHeader,
                "Active", new Random().Next(2, 6),
                Environment.GetEnvironmentVariable("CAU_Automation_Merchant_Terminal")!,
                createPlanDefaultValues, null!, null!, null!,
                null!, null!, "yes",
                "None");
            // var startInstallmentsResponse =
            //     await _startInstallmentsFunctionality.SendStartInstallmentsRequestAsync(
            //         requestHeader, json.InstallmentPlanNumber);
            // Assert.That(startInstallmentsResponse.ResponseHeader.Succeeded);
            Console.WriteLine("Done with PlansCreator");
            return json.InstallmentPlanNumber;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in PlansCreator" + e);
            throw;
        }
    }

    public async Task<bool> ValidateAuditLogLogs(RequestHeader requestHeader,
        string ipn)
    {
        try
        {
            Console.WriteLine("Starting ValidateAuditLogLogs");
            Console.WriteLine("Starting audit log process for ipn -> " + ipn);
            var activityLog1 = await _auditLogController.WaitForActivityToBeLogged(
                requestHeader, ipn, new[] { "Update Payment Data" });
            Console.WriteLine("Validating if got the log");
            Assert.That(activityLog1);
            Console.WriteLine("log was received for ipn -> " + ipn);
            Console.WriteLine("Done with ValidateAuditLogLogs");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateAuditLogLogs" + e);
            throw;
        }
    }

    public async Task<bool> CheckWebhookInCollection(string pgtlCaptureTransactionId, List<string> gatewayEventTypeList)
    {
        try
        {
            Console.WriteLine("Starting CheckWebhookInCollection");
            var counter = 0;
            var db = _mongoHandler.MongoConnect(Environment.GetEnvironmentVariable("MongoConnection")!, "Splitit_WebhooksDB");
            var returnDoc = await _mongoHandler.QueryMongoAndReturnDoc(db!, "Webhooks_Raw",
                "GatewayTransactionId", pgtlCaptureTransactionId);
            foreach (var item in returnDoc)
            {
                Console.WriteLine("Validating value --> " +item[7] + " in ExpectedList");
                Assert.That(gatewayEventTypeList.Contains(item[7].ToString()!));
                Console.WriteLine("Value --> " +item[7] + "was found in ExpectedList");
                counter++;
            }

            if (counter != gatewayEventTypeList.Count)
            {
                Console.WriteLine("CheckWebhookInCollection Done With ERROR");
                return false;
            }
            Console.WriteLine("Done with CheckWebhookInCollection");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in CheckWebhookInCollection" + e);
            return false;
        }
    }
}