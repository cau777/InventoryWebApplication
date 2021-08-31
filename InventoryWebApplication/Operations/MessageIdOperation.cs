namespace InventoryWebApplication.Operations
{
    /// <summary>
    /// Contains a message, a severity, and an id to show in a view
    /// </summary>
    public class MessageIdOperation : MessageOperation
    {
        public int Id { get; }

        public MessageIdOperation(int userId)
        {
            Id = userId;
        }

        public MessageIdOperation(string message, int userId) : base(message)
        {
            Id = userId;
        }

        public MessageIdOperation(string message, MessageSeverity severity, int userId) : base(message, severity)
        {
            Id = userId;
        }
    }
}