namespace Splitit.Automation.NG.Backend.Services.Ams.MockData;

public class FundingSetupDefaultValues
{
    public int CreditLine;
    public string RiskRating;
    public int ReservePool;
    public string FundingTrigger;
    public bool DebitOnHold;
    public bool FundingOnHold;
    public DateTime FundingEndDate;
    public DateTime FundingStartDate;
    public string SettlementType;
    public string FundNonSecuredPlans;
        
    public FundingSetupDefaultValues()
    {
        var faker = new Bogus.Faker();
        CreditLine = faker.Random.Int();
        RiskRating = "High";
        ReservePool = faker.Random.Int();
        FundingTrigger = "PlanActivation";
        DebitOnHold = faker.Random.Bool();
        FundingOnHold = faker.Random.Bool();
        FundingEndDate = DateTime.Now.AddYears(1);
        FundingStartDate = DateTime.Now;
        SettlementType = "NetSettle";
        FundNonSecuredPlans = "YES";
    }
}