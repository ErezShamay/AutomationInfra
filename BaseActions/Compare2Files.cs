namespace Splitit.Automation.NG.Backend.BaseActions;

public class Compare2Files
{
    public async Task<bool> Compering2Files(string pathFirstFile, string pathSecondFile)
    {
        try
        {
            Console.WriteLine("Starting Compering2Files");
            await Task.Delay(5 * 1000);
            var content1 = File.ReadAllText(pathFirstFile);
            var content2 = File.ReadAllText(pathSecondFile);
            if (content1 == content2)
            {
                Console.WriteLine("The two files are identical.");
                return true;
            }
            else
            {
                Console.WriteLine("The two files are not identical.");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in Compering2Files" + e);
            throw;
        }
    }
}