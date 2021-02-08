using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.AvaloniaClient.Models
{
    public class User : BaseEntity, IUser
    {
        public User()
        {
            StepTests = new List<IStepTest>();
        }

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
        public bool IsLoaded { get; set; } = false;

        public static User Create(string firstName, string lastName, string street, string postCode, string postCity, DateTime birthDate, int height, float weight, string sex, string email)
        {
            var newUser = new User { FirstName = firstName, LastName = lastName, Street = street, PostCode = postCode, PostCity = postCity, BirthDate = birthDate, Height = height, Sex = sex, Email = email, IsLoaded = true };
            newUser.AcceptChanges();
            return newUser;
        }
    }
}
