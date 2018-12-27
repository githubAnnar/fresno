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

        public void UpdateUser(User entity)
        {
            _unitOfWork.UserRepository.Update(entity);
        }

        public void RemoveUser(User entity)
        {
            _unitOfWork.UserRepository.Remove(entity);
        }

        public void AddUser(User entity)
        {
            _unitOfWork.UserRepository.Add(entity);
        }

        public IEnumerable<StepTest> GetAllStepTests()
        {
            return _unitOfWork.StepTestRepository.All();
        }

        public void UpdateStepTest(StepTest entity)
        {
            _unitOfWork.StepTestRepository.Update(entity);
        }

        public void RemoveStepTest(StepTest entity)
        {
            _unitOfWork.StepTestRepository.Remove(entity);
        }

        public void AddStepTest(StepTest entity)
        {
            _unitOfWork.StepTestRepository.Add(entity);
        }

        public IEnumerable<Measurement> GetAllMeasurements()
        {
            return _unitOfWork.MeasurementRepository.All();
        }

        public void UpdateMeasurement(Measurement entity)
        {
            _unitOfWork.MeasurementRepository.Update(entity);
        }

        public void RemoveMeasurement(Measurement entity)
        {
            _unitOfWork.MeasurementRepository.Remove(entity);
        }

        public void AddMeasurement(Measurement entity)
        {
            _unitOfWork.MeasurementRepository.Add(entity);
        }
    }
}
