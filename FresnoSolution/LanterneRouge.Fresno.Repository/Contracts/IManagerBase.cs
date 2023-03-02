namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IManagerBase
    {
        public abstract void Commit();

        public abstract void Close();
    }
}
