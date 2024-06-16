using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class ConvertPdfToStringController
{
    public string ConvertPdfToString(string pathToPdf)
    {
        var text = new StringBuilder();

        using (var pdfReader = new PdfReader(pathToPdf))
        {
            using (var pdfDocument = new PdfDocument(pdfReader))
            {
                for (var page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page)));
                }
            }
        }
        return text.ToString();
    }
}