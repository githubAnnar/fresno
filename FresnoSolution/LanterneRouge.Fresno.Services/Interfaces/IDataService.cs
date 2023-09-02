﻿using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        IEnumerable<IUserEntity> GetAllUsers();

        IUserEntity GetUser(Guid id);

        IUserEntity GetUserByStepTest(IStepTestEntity stepTest);

        void SaveUser(IUserEntity entity);

        void RemoveUser(IUserEntity entity);

        bool IsChanged(IUserEntity entity);

        bool IsChanged(IStepTestEntity entity);

        bool IsChanged(IMeasurementEntity entity);

        IEnumerable<IStepTestEntity> GetAllStepTests();

        IEnumerable<IStepTestEntity> GetAllStepTestsByUser(IUserEntity entity);

        int StepTestCountByUser(IUserEntity entity, bool onlyInCalculation = false);

        void SaveStepTest(IStepTestEntity entity);

        void RemoveStepTest(IStepTestEntity entity);

        IEnumerable<IMeasurementEntity> GetAllMeasurements();

        IEnumerable<IMeasurementEntity> GetAllMeasurementsByStepTest(IStepTestEntity entity);

        int MeasurementsCountByStepTest(IStepTestEntity entity, bool onlyInCalculation = false);

        void SaveMeasurement(IMeasurementEntity entity);

        void RemoveMeasurement(IMeasurementEntity entity);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
