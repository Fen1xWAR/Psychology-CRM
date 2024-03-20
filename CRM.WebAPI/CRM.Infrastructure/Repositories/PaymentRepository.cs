using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class PaymentRepository : RepositoryBase, IPaymentRepository
{
    public PaymentRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Payment>> GetAll()
    {
        return await GetDataSql<Payment, PaymentCreator>("SELECT * FROM payments");
    }

    public async Task<Payment> GetById(Guid id)
    {
        return (await GetDataSql<Payment, PaymentCreator>($"SELECT * FROM payments WHERE payment_id='{id}'")).First();
    }

    public async Task Put(Payment payment)
    {
        var paymentId = Guid.NewGuid();
         await ExecuteSql(
            $"Insert into payments(payment_id, client_id, visit_id, payment_date, payment_amount, payment_method) VALUES " +
            $" ('{paymentId}','{payment.ClientId}','{payment.VisitId}','{payment.PaymentDate:yyyy-MM-dd HH:mm:ss.fff}','{payment.PaymentAmount}','{payment.PaymentMethod}')");
    }

    public async Task Update(Payment dataToUpdate)
    {
        await ExecuteSql($"Update payments Set client_id=coalesce('{dataToUpdate.ClientId}',client_id )," +
                         $"visit_id=coalesce('{dataToUpdate.VisitId}', visit_id)," +
                         $"payment_date=coalesce('{dataToUpdate.PaymentDate:yyyy-MM-dd HH:mm:ss.fff}',payment_date)," +
                         $"payment_amount=coalesce('{dataToUpdate.PaymentAmount}', payment_amount)," +
                         $"payment_method=coalesce('{dataToUpdate.PaymentMethod}', payment_method)" +
                         $"where payment_id='{dataToUpdate.PaymentId}'");

    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"delete from payments where payment_id='{id}'");
            
    }
}