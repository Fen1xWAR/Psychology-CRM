using CRM.Core.Interfaces;

namespace CRM.Core.Implement;

public class Success<T> : ResultBase<T>, IOperationResult<T>
{
    public Success(T element) :
        base(element, true, string.Empty)
    {

    }
}

public class Success : ResultBase, IOperationResult
{
    public Success() :
        base(true, string.Empty)
    {

    }
}