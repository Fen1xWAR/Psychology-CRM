using System.Data;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Domain.Models;
public class FileCreator : ICreator<File>
{
    public File Map(NpgsqlDataReader reader)
    {
        return new File()
        {
            FileId = reader.GetGuid(0),
            ClientId = reader.GetGuid(1),
            PsychologistId = reader.GetGuid(2),
            FileName = reader.GetString(3),
            FileContent = reader.GetFieldValue<byte[]>(4)
        };
    }
}