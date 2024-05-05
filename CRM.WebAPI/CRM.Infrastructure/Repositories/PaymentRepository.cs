using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
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

    public async Task<IOperationResult<IEnumerable<Payment>>> GetAll()
    {
        return new Success<IEnumerable<Payment>>(await GetDataSql<Payment, PaymentCreator>("SELECT * FROM payments"));
    }

    public async Task<IOperationResult< Payment>> GetById(Guid id)
    {
        var result = (await GetDataSql<Payment, PaymentCreator>("SELECT * FROM payments WHERE payment_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Payment>(null,"Not found payment with current id!");
        return new Success<Payment>(result);
    }

    public async Task<IOperationResult<Guid>> Put(PaymentModel payment)
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
        return new Success<Guid>(paymentId);
    }

    public async Task<IOperationResult> Update(Payment dataToUpdate)
    {
        var paymentToUpdate = await GetById(dataToUpdate.PaymentId);
        if (!paymentToUpdate.Successful)
            return new ElementNotFound("Not found payment with current id");
        
        await ExecuteSql(
            "UPDATE payments SET client_id = COALESCE(@clientId, client_id), visit_id = COALESCE(@visitId, visit_id), payment_date = COALESCE(@paymentDate, payment_date), payment_amount = COALESCE(@paymentAmount, payment_amount), payment_method = COALESCE(@paymentMethod, payment_method) WHERE payment_id = @id",
            new NpgsqlParameter("@clientId", dataToUpdate.ClientId),
            new NpgsqlParameter("@visitId", dataToUpdate.VisitId),
            new NpgsqlParameter("@paymentDate", dataToUpdate.PaymentDate),
            new NpgsqlParameter("@paymentAmount", dataToUpdate.PaymentAmount),
            new NpgsqlParameter("@paymentMethod", dataToUpdate.PaymentMethod),
            new NpgsqlParameter("@id", dataToUpdate.PaymentId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var paymentToDelete = await GetById(id);
        if (!paymentToDelete.Successful)
            return new ElementNotFound("Not found payment with current id");
        await ExecuteSql("DELETE FROM payments WHERE payment_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}