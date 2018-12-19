using System.Data;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
