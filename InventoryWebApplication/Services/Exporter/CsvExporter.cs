using System.Text;
using InventoryWebApplication.Models.Interfaces;
using InventoryWebApplication.Services.Database;

namespace InventoryWebApplication.Services.Exporter
{
    public class CsvExporter<T> : ExporterService where T : class, IIdBasedModel, ITableRow
    {
        private readonly DatabaseService<T> _databaseService;

        public CsvExporter(DatabaseService<T> databaseService)
        {
            _databaseService = databaseService;
        }

        public override string ContentType => "text/csv";
        public override string FileExtension => "csv";

        public override byte[] Export()
        {
            StringBuilder builder = new();
            bool firstTime = true;

            foreach (T model in _databaseService.GetAll())
            {
                if (firstTime)
                {
                    firstTime = false;
                    builder.AppendJoin(',', model.TableRowHeaders).AppendLine();
                }

                builder.AppendJoin(',', model.ToTableRow()).AppendLine();
            }

            return EncodeUtf8(builder.ToString());
        }
    }
}