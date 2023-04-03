using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public abstract class RepositoryBase
    {
        //protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get; private set; }

        public RepositoryBase(IDbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
    }
}
