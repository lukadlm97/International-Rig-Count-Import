using Homework.Enverus.InternationalRigCountImport.Core.Models.Enums;

namespace Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs
{
    public record OperationResult<T> where T : class
    {
        public T Result { get; set; }
        public OperationStatus Status { get; set; }
        public string Description { get; set; }

        public OperationResult(OperationStatus status)
        {
            Status = status;
        } 
        
        public OperationResult(OperationStatus status, T result)
        {
            Status = status;
            Result = result;
        }
        public OperationResult(OperationStatus status, string description)
        {
            Status = status;
            Description = description;
        }

    }
}
