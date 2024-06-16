namespace Splitit.Automation.NG.Backend.Services.V1.DefaultValues;

public class ConsumerDataDefaultValues
{
    public string firstName;
    public string lastName;
    public string fullName;
    public string email;
    public string phoneNumber;
	
		
    public ConsumerDataDefaultValues() {
        fullName = "Automation Shopper";
        email = "Splitit.Automation@splitit.com";
        phoneNumber = "0502223344";
    }
}