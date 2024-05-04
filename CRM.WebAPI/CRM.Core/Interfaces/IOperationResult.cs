namespace CRM.Core.Interfaces;

public interface IOperationResult
{
    public bool Successful { get; }
    public string ErrorMessage { get; }
    
}

public interface IOperationResult<T> : IOperationResult
{
    public T Result { get; }
}