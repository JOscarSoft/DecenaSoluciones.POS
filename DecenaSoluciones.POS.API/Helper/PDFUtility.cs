using SelectPdf;

namespace DecenaSoluciones.POS.API.Helper
{
    public class PDFUtility
    {
        public static MemoryStream GeneratePDFFile(string htmlTemplate)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MarginBottom = 30;
            converter.Options.MarginTop = 15;

            PdfDocument document = converter.ConvertHtmlString(htmlTemplate);

            var ms = new MemoryStream();
            document.Save(ms);

            return ms;
        }
    }
}
