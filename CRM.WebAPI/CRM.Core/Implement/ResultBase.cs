using CRM.Core.Interfaces;

namespace CRM.Core.Implement;

public abstract class ResultBase<T> : IOperationResult, IOperationResult<T>
{
    public bool Successful { get; protected set; }
    public string ErrorMessage { get; protected set; }
    public T Result { get; protected set; }

    public ResultBase(T result, bool successful = true, string errorMessage = null)
    {
        if (result == null && successful)

            throw new ArgumentException("Result field required");
        this.Result = result;
        this.ErrorMessage = errorMessage;
        this.Successful = successful;
    }
}

public abstract class ResultBase : IOperationResult
{
    public bool Successful { get; protected set; }
    public string ErrorMessage { get; protected set; }

    protected ResultBase(bool successful, string errorMessage)
    {
        if (!successful && string.IsNullOrEmpty(errorMessage))
            throw new ArgumentException("Error message required");
        Successful = successful;
        ErrorMessage = errorMessage;
    }
}