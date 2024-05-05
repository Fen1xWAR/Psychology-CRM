using CRM.Core.Interfaces;

namespace CRM.Core.Implement;

public class InternalError<T> : ResultBase<T>, IOperationResult<T>
{
    public InternalError(T element, string errorMessage)
        : base(element, false, errorMessage)
    {

    }
}

public class InternalError : ResultBase
{
    public InternalError(string errorMessage)
        : base(false, errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
            this.ErrorMessage = $"Что-то пошло не так";
    }
}