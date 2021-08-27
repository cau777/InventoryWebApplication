using System;
using System.Text;

namespace InventoryWebApplication.Services.Exporter
{
    public abstract class ExporterService
    {
        public string FileName => $"Export {DateTime.Today:MM-dd-yyyy}.{FileExtension}";

        public abstract string FileType { get; }
        public abstract string FileExtension { get; }
        public abstract byte[] Export();

        protected byte[] EncodeUtf8(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}