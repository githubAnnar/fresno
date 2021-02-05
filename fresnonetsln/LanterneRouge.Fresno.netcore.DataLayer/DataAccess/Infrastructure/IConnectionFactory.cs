using System.Data;

namespace LanterneRouge.Fresno.netcore.DataLayer.DataAccess.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
