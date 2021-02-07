using System.Data;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}