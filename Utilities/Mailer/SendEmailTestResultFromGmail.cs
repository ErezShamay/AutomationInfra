using System.Net;
using System.Net.Mail;

namespace Splitit.Automation.NG.Utilities.Mailer;

public class SendEmailTestResultFromGmail
{
    private const string SenderEmailId = "splitit.automation@splitit.com";
    private const string SenderPassword = "Sa@1q2w3e";
    private const string EmailSmtpServer = "smtp.gmail.com";
    private const int EmailSmtpPort = 587;

    public SendEmailTestResultFromGmail(string subject, string recipients, string body, string jobName , string path)
    {
        try
        {
            var smtpServer = new SmtpClient(EmailSmtpServer, EmailSmtpPort);
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            var email = new MailMessage();
            email.From = new MailAddress(SenderEmailId);
            foreach (var address in recipients.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                email.To.Add(address);
            email.Subject = subject;
            email.Body = "Path to the report: " + path + "\n\n Comments: " + body;
            var attachment = new Attachment("/home/jenkins/agent/workspace/Splitit.Automation.NG_"+ jobName +"/allure-report/index.html");
            email.Attachments.Add(attachment);
            smtpServer.EnableSsl = true;
            smtpServer.UseDefaultCredentials = false;
            smtpServer.Credentials = new NetworkCredential(SenderEmailId, SenderPassword);
            smtpServer.Send(email);
            Console.WriteLine("Email Was Successfully Sent\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendEmailTestResultFromGmail" + exception + "\n");
            throw;
        }
    }

    public static void Sender(string info, string recipients, string body)
    {
        Console.WriteLine("\nStarting with sending Email test result");
        var pathToReport = Environment.GetEnvironmentVariable("BUILD_URL");
        var path = pathToReport +"/allure/";
        var jobName = Environment.GetEnvironmentVariable("JOB_NAME");
        var now = DateTime.Now;
        new SendEmailTestResultFromGmail("Test Result of: " + now.ToString("F") + " -> " + info, recipients, body, jobName, path);
    }
}