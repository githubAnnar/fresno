using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure;
using LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using log4net;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.WpfClient.Services
{
    public class DataService : IDataService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataService));
        private IUnitOfWork _unitOfWork;
        private ConnectionFactory _connectionFactory;

        public event CommittedHandler Committed;

        private ConnectionFactory LocalConnectionFactory => _connectionFactory ?? (_connectionFactory = new ConnectionFactory(Filename));

        private string Filename { get; set; }

        public bool LoadDatabase(string filename)
        {
            bool response;
            try
            {
                Filename = filename;
                _unitOfWork = new UnitOfWork(LocalConnectionFactory);
                response = true;
            }

            catch (Exception e)
            {
                Logger.Error("Unexpected error", e);
                response = false;
            }

            if (response)
            {
                Logger.Info($"Database '{filename}' is loaded");
            }

            else
            {
                Logger.Warn($"Database '{filename}' is not loaded");
            }

            return response;
        }

        public bool CloseDatabase()
        {
            bool response;
            try
            {
                if (_unitOfWork != null)
                {
                    _unitOfWork.Close();
                    _connectionFactory = null;
                }

                response = true;
            }

            catch (Exception e)
            {
                Logger.Error($"Unexpected error when closing database '{Filename}'", e);
                response = false;
            }

            if (response && !string.IsNullOrEmpty(Filename))
            {
                Logger.Info($"Database '{Filename}' is closed");
            }

            else if (!response && !string.IsNullOrEmpty(Filename))
            {
                Logger.Warn($"Database '{Filename}' is not closed");
            }

            return response;
        }

        public void Commit()
        {
            _unitOfWork.Commit();
            Committed?.Invoke();
        }

        public IEnumerable<User> GetAllUsers(bool refresh = false) => _unitOfWork.GetAllUsers(refresh);

        public void UpdateUser(User entity) => _unitOfWork.UpsertUser(entity);

        public void RemoveUser(User entity) => _unitOfWork.RemoveUser(entity);

        public void AddUser(User entity) => _unitOfWork.UpsertUser(entity);

        public IEnumerable<StepTest> GetAllStepTests() => _unitOfWork.GetAllStepTests();

        public IEnumerable<StepTest> GetAllStepTestsByUser(User parent) => _unitOfWork.GetUserById(parent.Id).StepTests;

        public void UpdateStepTest(StepTest entity) => _unitOfWork.UpsertStepTest(entity);

        public void RemoveStepTest(StepTest entity) => _unitOfWork.RemoveStepTest(entity);

        public void AddStepTest(StepTest entity) => _unitOfWork.UpsertStepTest(entity);

        public IEnumerable<Measurement> GetAllMeasurements() => _unitOfWork.GetAllMeasurements();

        public void UpdateMeasurement(Measurement entity) => _unitOfWork.UpsertMeasurement(entity);

        public void RemoveMeasurement(Measurement entity) => _unitOfWork.RemoveMeasurement(entity);

        public void AddMeasurement(Measurement entity) => _unitOfWork.UpsertMeasurement(entity);
    }
}
