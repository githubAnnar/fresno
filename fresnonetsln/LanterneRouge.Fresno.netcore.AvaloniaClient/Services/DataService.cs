using LanterneRouge.Fresno.netcore.AvaloniaClient.Models;
using LanterneRouge.Fresno.netcore.AvaloniaClient.Services.Interfaces;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Infrastructure;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.netcore.AvaloniaClient.Services
{
    public class DataService : IDataService
    {
        private IUnitOfWork _unitOfWork;
        private ConnectionFactory _connectionFactory;

        public event CommittedHandler Committed;

        private ConnectionFactory LocalConnectionFactory => _connectionFactory ??= new ConnectionFactory(Filename);

        private string Filename { get; set; } = string.Empty;

        public bool LoadDatabase(string filename)
        {
            bool response;
            try
            {
                Filename = filename;
                _unitOfWork = new UnitOfWork(LocalConnectionFactory);
                response = true;
            }

            catch (Exception)
            {
                response = false;
            }

            return response;
        }

        public void Commit()
        {
            _unitOfWork.Commit();
            Committed?.Invoke();
        }

        public IEnumerable<User> GetAllUsers(bool refresh = false) => _unitOfWork.GetAllUsers<User, StepTest, Measurement>(refresh);

        public void UpdateUser(User entity) => _unitOfWork.UpsertUser(entity);

        public void RemoveUser(User entity) => _unitOfWork.RemoveUser(entity);

        public void AddUser(User entity) => _unitOfWork.UpsertUser(entity);

        public IEnumerable<StepTest> GetAllStepTests() => _unitOfWork.GetAllStepTests<User, StepTest, Measurement>();

        public IEnumerable<StepTest> GetAllStepTestsByUser(User parent) => _unitOfWork.GetUserById<User, StepTest, Measurement>(parent.Id).StepTests.Cast<StepTest>();

        public void UpdateStepTest(StepTest entity) => _unitOfWork.UpsertStepTest(entity);

        public void RemoveStepTest(StepTest entity) => _unitOfWork.RemoveStepTest(entity);

        public void AddStepTest(StepTest entity) => _unitOfWork.UpsertStepTest(entity);

        public IEnumerable<Measurement> GetAllMeasurements() => _unitOfWork.GetAllMeasurements<User, StepTest, Measurement>();

        public void UpdateMeasurement(Measurement entity) => _unitOfWork.UpsertMeasurement(entity);

        public void RemoveMeasurement(Measurement entity) => _unitOfWork.RemoveMeasurement(entity);

        public void AddMeasurement(Measurement entity) => _unitOfWork.UpsertMeasurement(entity);
    }
}