using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.App.Extensions
{
    public interface IResourceService
    {
        Task<string> GetFileContent(string path);
        Task DownloadPdfReceiptStream(Stream pdfFile);
    }
}
