namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class PostFilesUploadAccountIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class File
    {
        public string FileId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<File> Files { get; set; }
    }
}