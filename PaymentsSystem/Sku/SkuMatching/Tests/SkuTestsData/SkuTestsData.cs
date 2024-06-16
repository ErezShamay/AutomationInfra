using LumenWorks.Framework.IO.Csv;
using NUnit.Framework;

namespace Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;

public class SkuTestsData
{
    private readonly Dictionary<string, PaymentSettings> _skuDict = new();
    private readonly Dictionary<string, Dictionary<string, string>> _signDict = new();
    private readonly Dictionary<string, FeesSettings> _feesDict = new();

    public class PaymentSettings
    {
        public long CreditLine { get; set; }
        public string RiskRating { get; set; } = null!;
        public long ReservePool { get; set; }
        public string FundingTrigger { get; set; } = null!;
        public bool DebitOnHold { get; set; }
        public bool FundingOnHold { get; set; }
        public string FundingEndDate { get; set; } = null!;
        public string FundingStartDate { get; set; } = null!;
        public string MonetaryFlow { get; set; } = null!;
        public List<string> SetActivity { get; set; } = null!;
        public bool IsActive { get; set; }
        public string FundNonSecuredPlans { get; set; } = null!;
        public string SettlementType { get; set; } = null!;
    }

    public class FeesSettings
    {
        public string GrossSettlementAmount { get; set; } = null!;
        public string VariableFee { get; set; } = null!;
        public string FixedFee { get; set; } = null!;
    }

    public (Dictionary<string, PaymentSettings>, Dictionary<string, Dictionary<string, string>>, Dictionary<string, FeesSettings>) 
        BuildPaymentSettingsDictionary(CachedCsvReader csv, string isProcessing = default!, string isReports = default!)
    {
        try
        {
            Console.WriteLine("\nStarting BuildPaymentSettingsDictionary");
            if (_skuDict.Count > 0)
            {
                return (_skuDict, _signDict, _feesDict);
            }

            foreach (var line in csv.Where(line => !line[0].Contains("UNFUNDED SKUs") && !line[0].Contains("FUNDED SKUs") 
                         && !line[1].Contains("Merchant Setup") 
                         && !line[0].Contains("SKU") 
                         && !line[0].Contains("Terminal")))
            {
                if (isReports == null)
                {
                    ListToDict(line, isProcessing);
                }
                else
                {
                    ListToDictReports(line);
                    ListToDictListSign(line);
                    ListToDictListFees(line);
                }
            }
        
            Console.WriteLine("Done BuildPaymentSettingsDictionary\n");
            return (_skuDict, _signDict, _feesDict);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in BuildPaymentSettingsDictionary\n" + exception + "\n");
            throw;
        }
    }

    private void ListToDictListSign(string[] listSplitElements)
    {
        try
        {
            Console.WriteLine("Starting ListToDictListSign");
            var signDictionary = new Dictionary<string, string>();
            var temp1 = listSplitElements[5];
            if (temp1.Equals("")) return;
            var temp = listSplitElements[0];
            var trimmed = string.Concat(temp.Where(c => !char.IsWhiteSpace(c)));
            var underScore = trimmed.Replace("-", "_");
            var key = "TestValidate_" + underScore;
            var signList = listSplitElements[20].Split("+");
            if (signList.Length < 2)
            {
                if (signList[0].Contains("NEGATIVE"))
                {
                    signDictionary[signList[0]] = "NEGATIVE";
                }
                if (signList[0].Contains("POSITIVE"))
                {
                    signDictionary[signList[0]] = "POSITIVE";
                }
            }
            else
            {
                signDictionary = SplitToList(signList);
            }
            _signDict.TryAdd(key, signDictionary);
            Console.WriteLine("Done with ListToDictListSign");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ListToDictListSign" + e);
            throw;
        }
    }
    
    private void ListToDictListFees(string[] listSplitElements)
    {
        try
        {
            Console.WriteLine("Starting ListToDictListSign");
            var feesObj = new FeesSettings();
            var temp1 = listSplitElements[5];
            if (temp1.Equals("")) return;
            var temp = listSplitElements[0];
            var trimmed = string.Concat(temp.Where(c => !char.IsWhiteSpace(c)));
            var underScore = trimmed.Replace("-", "_");
            var key = "TestValidate_" + underScore;
            feesObj.GrossSettlementAmount = listSplitElements[22];
            feesObj.VariableFee = listSplitElements[23];
            feesObj.FixedFee = listSplitElements[24];
            _feesDict.TryAdd(key, feesObj);
            Console.WriteLine("Done with ListToDictListSign");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ListToDictListSign" + e);
            throw;
        }
    }

    private Dictionary<string, string> SplitToList(string[] signList)
    {
        var signDict = new Dictionary<string, string>();
        foreach (var item in signList)
        {
            var splitList = item.Split("=");
            signDict[splitList[0]] = splitList[1];
        }

        return signDict;
    }

    private void ListToDict(string[] listSplitElements, string isProcessing)
    {
        try
        {
            var temp1 = listSplitElements[5];
            if (temp1.Equals("")) return;
            var paymentSettings = new PaymentSettings();
            var temp = listSplitElements[0];
            var trimmed = string.Concat(temp.Where(c => !char.IsWhiteSpace(c)));
            var underScore = trimmed.Replace("-", "_");
            var key = "TestValidate_" + underScore;
            paymentSettings.MonetaryFlow = listSplitElements[1];
            var fElement = listSplitElements[2];
            paymentSettings.FundingOnHold = !fElement.ToLower().Equals("false");
            var dElement = listSplitElements[3];
            paymentSettings.DebitOnHold = !dElement.ToLower().Equals("false");
            paymentSettings.RiskRating = listSplitElements[4];
            var creditLine = listSplitElements[5];
            paymentSettings.CreditLine = long.Parse(creditLine);
            var reservePool = listSplitElements[6];
            paymentSettings.ReservePool = long.Parse(reservePool);
            var eDateElement = listSplitElements[7];
            paymentSettings.FundingEndDate = !eDateElement.Equals("null") ? DateTime.Now.ToString("MM/dd/yyyy") : null!;

            var sDateElement = listSplitElements[8];

            if (!sDateElement.Equals("null"))
            {
                paymentSettings.FundingStartDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else
            {
                paymentSettings.FundingEndDate = null!;
            }

            paymentSettings.SetActivity = new List<string>();
            paymentSettings.FundingTrigger = listSplitElements[9];
            var active = listSplitElements[10];
            if (active.Equals("TRUE") || active.Equals("true"))
            {
                paymentSettings.IsActive = true;
            }
            paymentSettings.FundNonSecuredPlans = listSplitElements[11];
            if (isProcessing != null!)
            {
                var activityStatus = listSplitElements[14];
                var listActivityStatus = new string[] { };
                
                if (activityStatus.Contains(">"))
                {
                    listActivityStatus = activityStatus.Split(">");
                }
                else if (activityStatus.Contains("+"))
                {
                    listActivityStatus = activityStatus.Split("+");
                }
                else if (listActivityStatus.Length == 0)
                {
                    paymentSettings.SetActivity.Add(activityStatus);
                }
                foreach (var item in listActivityStatus)
                {
                    paymentSettings.SetActivity.Add(item);
                }
            }
            else
            {
                paymentSettings.SetActivity.Add(listSplitElements[14]);
                paymentSettings.SetActivity.Add(listSplitElements[15]);
                paymentSettings.SetActivity.Add(listSplitElements[16]);
                paymentSettings.SetActivity.Add(listSplitElements[17]);
                paymentSettings.SetActivity.Add(listSplitElements[18]);
                paymentSettings.SetActivity.Add(listSplitElements[19]);
                paymentSettings.SetActivity.Add(listSplitElements[20]);
                paymentSettings.SetActivity.Add(listSplitElements[21]);
                paymentSettings.SetActivity.Add(listSplitElements[22]);
                paymentSettings.SetActivity.Add(listSplitElements[23]);
            }

            _skuDict.TryAdd(key, paymentSettings);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ListToDict\n" + exception);
            throw;
        }
    }
    
    private void ListToDictReports(string[] listSplitElements)
    {
        try
        {
            var temp1 = listSplitElements[5];
            if (temp1.Equals("")) return;
            var paymentSettings = new PaymentSettings();
            var temp = listSplitElements[0];
            var trimmed = string.Concat(temp.Where(c => !char.IsWhiteSpace(c)));
            var underScore = trimmed.Replace("-", "_");
            var key = "TestValidate_" + underScore;
            paymentSettings.MonetaryFlow = listSplitElements[1];
            paymentSettings.SettlementType = listSplitElements[2];
            var fElement = listSplitElements[3];
            paymentSettings.FundingOnHold = !fElement.ToLower().Equals("false");
            var dElement = listSplitElements[4];
            paymentSettings.DebitOnHold = !dElement.ToLower().Equals("false");
            paymentSettings.RiskRating = listSplitElements[5];
            var creditLine = listSplitElements[6];
            paymentSettings.CreditLine = long.Parse(creditLine);
            var reservePool = listSplitElements[7];
            paymentSettings.ReservePool = long.Parse(reservePool);
            var eDateElement = listSplitElements[8];
            paymentSettings.FundingEndDate = !eDateElement.Equals("null") ? DateTime.Now.ToString("MM/dd/yyyy") : null!;

            var sDateElement = listSplitElements[9];

            if (!sDateElement.Equals("null"))
            {
                paymentSettings.FundingStartDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else
            {
                paymentSettings.FundingEndDate = null!;
            }

            paymentSettings.SetActivity = new List<string>();
            paymentSettings.FundingTrigger = listSplitElements[10];
            var active = listSplitElements[11];
            if (active.Equals("TRUE") || active.Equals("true"))
            {
                paymentSettings.IsActive = true;
            }
            paymentSettings.FundNonSecuredPlans = listSplitElements[12];
            if (listSplitElements[17].Contains("+"))
            {
                var activities = listSplitElements[17].Split("+");
                foreach (var activity in activities)
                {
                    paymentSettings.SetActivity.Add(activity);
                }
            }
            else
            {
                paymentSettings.SetActivity.Add(listSplitElements[17]);
            }
            _skuDict.TryAdd(key, paymentSettings);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ListToDict\n" + exception);
            throw;
        }
    }

    public bool ValidateSkuValue(string skuValue, string testName)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateSkuValue");
            var expectedResultSplitList = testName.Split("_");
            var actualResult = skuValue.Split("-");
            var seg = actualResult[6];
            var segRemove = seg.Remove(0, 1);
            var intSeg = int.Parse(segRemove);
            var installmentSeg = "";
            var fundedSeg = testName.Contains("FUN") ? "F" : "U";
            if (Enumerable.Range(13, 15).Contains(intSeg))
            {
                installmentSeg = fundedSeg + "15";
            }
            if (Enumerable.Range(16, 18).Contains(intSeg))
            {
                installmentSeg = fundedSeg + "18";
            }
            if (Enumerable.Range(19, 21).Contains(intSeg))
            {
                installmentSeg = fundedSeg + "21";
            }
            if (Enumerable.Range(22, 24).Contains(intSeg))
            {
                installmentSeg = fundedSeg + "24";
            }
            if (Enumerable.Range(25, 36).Contains(intSeg))
            {
                installmentSeg = fundedSeg + "36";
            }
            if (!installmentSeg.Equals(""))
            {
                expectedResultSplitList.SetValue(installmentSeg, 7);
            }
            foreach (var segment in actualResult)
            {
                Assert.That(expectedResultSplitList, Does.Contain(segment));
            }
            Console.WriteLine("Done ValidateSkuValue\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateSkuValue" + exception + "\n");
            return false;
        }
    }
}