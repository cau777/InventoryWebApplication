using System;
using System.Text;

namespace InventoryWebApplication.Services.Exporter
{
    public abstract class ExporterService
    {
        public string FileName => $"Export {DateTime.Today:MM-dd-yyyy}.{FileExtension}";

        public abstract string ContentType { get; }
        public abstract string FileExtension { get; }

        /// <summary>
        ///     Exports a table to an array of bytes according to different rules
        /// </summary>
        /// <returns>An array of bytes representing the table</returns>
        public abstract byte[] Export();

        protected byte[] EncodeUtf8(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}