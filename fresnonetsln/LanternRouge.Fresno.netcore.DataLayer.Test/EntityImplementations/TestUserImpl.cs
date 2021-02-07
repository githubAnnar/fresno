using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanternRouge.Fresno.netcore._DataLayer.Test.EntityImplementations
{
    public class TestUserImpl : BaseEntity, IUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string PostCity { get; set; }
        public DateTime BirthDate { get; set; }
        public int Height { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public int MaxHr { get; set; }
        public IList<IStepTest> StepTests { get; set; }

        public bool IsLoaded { get; set; }
    }
}
