namespace InventoryWebApplication.Operations
{
    /// <summary>
    ///     Contains a message, a severity, and an id to show in a view
    /// </summary>
    public class MessageIdOperation : MessageOperation
    {
        public MessageIdOperation(int id, string message = "", MessageSeverity severity = MessageSeverity.warning) :
            base(message, severity)
        {
            Id = id;
        }

        public int Id { get; }
    }
}