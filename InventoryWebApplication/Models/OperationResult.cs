namespace InventoryWebApplication.Models
{
    public class OperationResult
    {
        public static OperationResult Empty => new();
        
        public bool IsEmpty => Message.Equals(string.Empty);
        public string Message { get; }
        public OperationResultSeverity Severity { get; }
        
        public OperationResult()
        {
            Message = "";
        }
        
        public OperationResult(string message)
        {
            Message = message;
            Severity = OperationResultSeverity.warning;
        }
        
        public OperationResult(string message, OperationResultSeverity severity)
        {
            Message = message;
            Severity = severity;
        }
    }
}