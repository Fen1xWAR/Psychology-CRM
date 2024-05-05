using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IPaymentRepository
{
    Task<IOperationResult<IEnumerable<Payment>>> GetAll();
    Task<IOperationResult<Payment>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Payment payment);
    Task<IOperationResult> Update(Payment payment);
    Task<IOperationResult> RemoveById(Guid id);
}