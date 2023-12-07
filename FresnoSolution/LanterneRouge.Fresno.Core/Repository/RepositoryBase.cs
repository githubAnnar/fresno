using LanterneRouge.Fresno.Core.Infrastructure;
using System.Data;

namespace LanterneRouge.Fresno.Core.Repository
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
                if (Context.Database.EnsureCreated())
                    Console.WriteLine("Database is there!");
            }

            else
            {
                throw new ArgumentNullException(nameof(connection));
            }
        }
    }
}
