using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.App.Extensions
{
    internal class ResourceService : IResourceService
    {
        public async Task<string> GetFileContent(string path)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(path);
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();

            return contents;
        }

        public async Task DownloadPdfReceiptStream(Stream pdfFile)
        {
            var pdfByteArray = ReadFully(pdfFile);

            // cache the file
            string file = Path.Combine(FileSystem.CacheDirectory, "Recibo.pdf");
            await File.WriteAllBytesAsync(file, pdfByteArray);

            await Launcher.Default.OpenAsync(new OpenFileRequest("Recibo", new ReadOnlyFile(file)));
        }

        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
