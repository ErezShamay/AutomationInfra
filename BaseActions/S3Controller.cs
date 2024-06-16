namespace Splitit.Automation.NG.Backend.BaseActions;

public class S3Controller
{
    private readonly FileLocationController _fileLocationController = new();
    
    public async Task<string> DownloadFileFromS3(string fileUrl, string fileName, string fileTypeEnding)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                Console.WriteLine("Starting DownloadFileFromS3");
                string fileLocation = null!;
                var response = await httpClient.GetAsync(fileUrl);
                if (response.IsSuccessStatusCode)
                {
                    using (var fileStream = await response.Content.ReadAsStreamAsync())
                    {
                        fileLocation = _fileLocationController.ReturnFileLocation("ReportTests", fileName + fileTypeEnding);
                        using (var outputStream = File.Create(fileLocation))
                        {
                            fileStream.CopyTo(outputStream);
                        }
                    }
                    Console.WriteLine("File downloaded successfully!");
                    Console.WriteLine("File location is: " + fileLocation);
                }
                else
                {
                    Console.WriteLine("Failed to download the file. Status code: " + response.StatusCode);
                }
                return fileLocation;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered: " + e.Message);
                throw;
            }
        }
    }   
}