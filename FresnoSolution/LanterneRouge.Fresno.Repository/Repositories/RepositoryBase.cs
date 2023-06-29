using LanterneRouge.Fresno.Repository.Infrastructure;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Repositories
{
    public abstract class RepositoryBase
    {
        //protected IDbTransaction Transaction { get; private set; }
        protected StepTestContext Context { get; private set; }

        public RepositoryBase(IDbConnection connection)
        {
            Context = connection!=null ? new StepTestContext(connection) : throw new ArgumentNullException(nameof(connection));
        }
    }
}
