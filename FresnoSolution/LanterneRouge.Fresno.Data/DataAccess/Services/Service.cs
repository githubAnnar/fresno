using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Services
{
    public class Service : IService
    {
        private IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<User> GetAllUsers(bool refresh = false)
        {
            return _unitOfWork.GetAllUsers(refresh);
        }
    }
}
