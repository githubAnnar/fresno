using LanterneRouge.Fresno.Core.Infrastructure;

namespace LanterneRouge.Fresno.Core.Repository
{
    public abstract class RepositoryBase
    {
        //protected IDbTransaction Transaction { get; private set; }
        protected StepTestContext Context { get; private set; }

        public RepositoryBase(StepTestContext context)
        {
            if (context != null)
            {
                Context = context;
                if (Context.Database.EnsureCreated())
                    Console.WriteLine("Database is there!");
            }

            else
            {
                throw new ArgumentNullException(nameof(context));
            }
        }
    }
}
