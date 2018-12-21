using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Services
{
    public interface IService
    {
        void Commit();

        IEnumerable<User> GetAllUsers(bool refresh = false);
    }
}
