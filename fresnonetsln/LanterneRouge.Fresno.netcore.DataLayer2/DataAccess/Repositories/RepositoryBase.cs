using System.Data;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public abstract class RepositoryBase
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection => Transaction.Connection; 

        public RepositoryBase(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}
