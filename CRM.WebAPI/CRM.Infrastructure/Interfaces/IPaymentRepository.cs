using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAll();
    Task<Payment> GetById(Guid id);
    Task Put(Payment payment);
    Task Update(Payment payment);
    Task RemoveById(Guid id);
}