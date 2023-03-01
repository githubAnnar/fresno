using System.Data;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
