using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

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
        return (await GetDataSql<Payment, PaymentCreator>("SELECT * FROM payments WHERE payment_id = @id",
            new NpgsqlParameter("@id", id))).First();
    }

    public async Task Put(Payment payment)
    {
        var paymentId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO payments (payment_id, client_id, visit_id, payment_date, payment_amount, payment_method) VALUES (@id, @clientId, @visitId, @paymentDate, @paymentAmount, @paymentMethod)",
            new NpgsqlParameter("@id", paymentId),
            new NpgsqlParameter("@clientId", payment.ClientId),
            new NpgsqlParameter("@visitId", payment.VisitId),
            new NpgsqlParameter("@paymentDate", payment.PaymentDate),
            new NpgsqlParameter("@paymentAmount", payment.PaymentAmount),
            new NpgsqlParameter("@paymentMethod", payment.PaymentMethod));
    }

    public async Task Update(Payment dataToUpdate)
    {
        await ExecuteSql(
            "UPDATE payments SET client_id = COALESCE(@clientId, client_id), visit_id = COALESCE(@visitId, visit_id), payment_date = COALESCE(@paymentDate, payment_date), payment_amount = COALESCE(@paymentAmount, payment_amount), payment_method = COALESCE(@paymentMethod, payment_method) WHERE payment_id = @id",
            new NpgsqlParameter("@clientId", dataToUpdate.ClientId),
            new NpgsqlParameter("@visitId", dataToUpdate.VisitId),
            new NpgsqlParameter("@paymentDate", dataToUpdate.PaymentDate),
            new NpgsqlParameter("@paymentAmount", dataToUpdate.PaymentAmount),
            new NpgsqlParameter("@paymentMethod", dataToUpdate.PaymentMethod),
            new NpgsqlParameter("@id", dataToUpdate.PaymentId));
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql("DELETE FROM payments WHERE payment_id = @id", new NpgsqlParameter("@id", id));
    }
}