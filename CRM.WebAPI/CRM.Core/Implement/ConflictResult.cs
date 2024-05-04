using CRM.Core.Interfaces;

namespace CRM.Core.Implement;

public class ConflictResult<T> : ResultBase<T>, IOperationResult<T>
{
    public ConflictResult(T element, string errorMessage)
        : base(element, false, errorMessage)
    {

    }
}

public class ConflictResult : ResultBase
{
    public ConflictResult(string errorMessage)
        : base(false, errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
            this.ErrorMessage = $"Элемент конфликтует с существующим состоянием БД!";
    }
}