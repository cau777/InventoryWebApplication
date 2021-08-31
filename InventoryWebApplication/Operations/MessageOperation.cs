namespace InventoryWebApplication.Operations
{
    /// <summary>
    ///     Contains a message and a severity to show in a view
    /// </summary>
    public class MessageOperation
    {
        public MessageOperation(string message = "", MessageSeverity severity = MessageSeverity.warning)
        {
            Message = message;
            Severity = severity;
        }

        public static MessageOperation Empty => new();

        public bool IsEmpty => Message.Equals(string.Empty);
        public string Message { get; }
        public MessageSeverity Severity { get; }
    }
}