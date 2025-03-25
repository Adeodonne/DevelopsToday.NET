using System.Data;

namespace Domain.Interfaces;

public interface IDatabaseFactory
{
    IDbConnection GetConnection();
}