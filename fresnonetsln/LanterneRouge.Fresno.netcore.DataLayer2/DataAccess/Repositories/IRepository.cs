using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Repositories
{
    public interface IRepository<TBaseInterface, TUserInterface, TStepTestInterface, TMeasurementInterface> where TBaseInterface : IBaseEntity where TUserInterface : IUser where TStepTestInterface : IStepTest where TMeasurementInterface : IMeasurement
    {
        void Add(TBaseInterface entity);

        IEnumerable<TBaseInterface> All<TUser, TStepTest, TMeasurement>() where TUser : TUserInterface where TStepTest : TStepTestInterface where TMeasurement : TMeasurementInterface;

        TBaseInterface FindSingle<TBase>(int id) where TBase : TBaseInterface;

        TBaseInterface FindWithParent<TUser, TStepTest, TMeasurement>(int id) where TUser : TUserInterface where TStepTest : TStepTestInterface where TMeasurement : TMeasurementInterface;

        TBaseInterface FindWithParentAndChilds<TUser, TStepTest, TMeasurement>(int id) where TUser : TUserInterface where TStepTest : TStepTestInterface where TMeasurement : TMeasurementInterface;

        IEnumerable<TBaseInterface> FindByParentId<TUser, TStepTest, TMeasurement>(IBaseEntity parent) where TUser : TUserInterface where TStepTest : TStepTestInterface where TMeasurement : TMeasurementInterface;

        void Remove(int id);

        void Remove(TBaseInterface entity);

        void Update(TBaseInterface entity);
    }
}
