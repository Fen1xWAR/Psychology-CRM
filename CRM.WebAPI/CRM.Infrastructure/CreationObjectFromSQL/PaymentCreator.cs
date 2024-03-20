using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class PaymentCreator:ICreator<Payment>
{
    public Payment Map(NpgsqlDataReader reader)
    {
        return new Payment()
        {
            PaymentId = reader.GetGuid(0),
            ClientId = reader.GetGuid(1),
            VisitId = reader.GetGuid(2),
            PaymentDate = reader.GetDateTime(3),
            PaymentAmount = reader.GetDecimal(4),
            PaymentMethod = reader.GetString(5)
        };
    }
}