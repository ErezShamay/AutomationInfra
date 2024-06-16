using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Responses;

namespace Splitit.Automation.NG.Backend.Tests.AdminPortalTests.EmailsTests;

public class EmailsController
{
    public bool ValidateEmailSubject(GetEmailsResponse.Root jResponse, string ipn, string subjectMessage,
    string merchantFlag = default!)
    {
        var logsResults = new List<bool>();
        try
        {
            Console.WriteLine("Starting ValidateEmailSubject");
            foreach (var log in jResponse.Logs)
            {
                Console.WriteLine("Log to look at ----> " + log.Subject);
                if (merchantFlag == null!)
                {
                    if (log.InstallmentPlanNumber.Equals(ipn))
                    {
                        if (log.Subject.Contains(subjectMessage))
                        {
                            logsResults.Add(true);
                        }
                    }
                }
                else
                {
                    if (log.Subject.Contains(subjectMessage))
                    {
                        logsResults.Add(true);
                    }
                }
            }
            Console.WriteLine("Done with ValidateEmailSubject");
            return logsResults.Count != 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateEmailSubject" + e);
            throw;
        }
    }
    
    public bool ValidateEmailType(GetEmailsResponse.Root jResponse, string ipn, string emailType)
    {
        var logsResults = new List<bool>();
        try
        {
            Console.WriteLine("Starting ValidateEmailType");
            foreach (var log in jResponse.Logs)
            {
                Console.WriteLine("Log to look at ----> " + log.EmailType);
                if (log.InstallmentPlanNumber.Equals(ipn))
                {
                    if (log.EmailType.Contains(emailType))
                    {
                        logsResults.Add(true);
                    }
                }
            }
            Console.WriteLine("Done with ValidateEmailType");
            return logsResults.Count != 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateEmailType" + e);
            throw;
        }
    }
}