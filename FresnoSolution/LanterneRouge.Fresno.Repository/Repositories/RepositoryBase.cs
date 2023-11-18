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
            if (connection != null)
            {
                Context = new StepTestContext(connection);
                Context.Database.EnsureCreated();
            }

            else
            {
                throw new ArgumentNullException(nameof(connection));
            }
        }
    }
}
