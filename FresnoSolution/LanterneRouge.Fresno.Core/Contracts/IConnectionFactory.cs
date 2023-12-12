using LanterneRouge.Fresno.Core.Infrastructure;
using System.Data;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
}
