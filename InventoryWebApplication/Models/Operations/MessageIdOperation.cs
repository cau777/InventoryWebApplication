namespace InventoryWebApplication.Models.Operations
{
    public class MessageIdOperation : MessageOperation
    {
        public int UserId { get; set; }
        
        public MessageIdOperation(int userId)
        {
            UserId = userId;
        }
        public MessageIdOperation(string message, int userId) : base(message)
        {
            UserId = userId;
        }
        public MessageIdOperation(string message, MessageSeverity severity, int userId) : base(message, severity)
        {
            UserId = userId;
        }
    }
}