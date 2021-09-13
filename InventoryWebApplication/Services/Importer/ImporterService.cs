using System.IO;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace InventoryWebApplication.Services.Importer
{
    public abstract class ImporterService
    {
        /// <summary>
        ///     Imports data to a table from the specified file asynchronously
        /// </summary>
        /// <param name="stream">Buffered stream containing data to import</param>
        public abstract Task Import([NotNull] Stream stream);

        protected string DecodeUtf8([NotNull] byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}