using System;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string PostCode { get; set; }

        public string PostCity { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public float Weight { get; set; }

        public string Sex { get; set; }

        public string Email { get; set; }
    }
}
