using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities
{
    public interface IUser : IBaseEntity
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Street { get; set; }

        string PostCode { get; set; }

        string PostCity { get; set; }

        DateTime BirthDate { get; set; }

        int Height { get; set; }

        string Sex { get; set; }

        string Email { get; set; }

        int MaxHr { get; set; }

        IList<IStepTest> StepTests { get; set; }

        bool IsLoaded { get; set; }
    }
}
