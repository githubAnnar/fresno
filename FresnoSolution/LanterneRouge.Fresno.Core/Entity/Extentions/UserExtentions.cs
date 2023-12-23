using LanterneRouge.Fresno.Core.Interface;
using System.Runtime.CompilerServices;

namespace LanterneRouge.Fresno.Core.Entity.Extentions
{
    public static class UserExtentions
    {
        public static bool IsValid(this IUserEntity userEntity)
        {
            if (userEntity == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(userEntity.Email))
            {
                return false;
            }

            if (string.IsNullOrEmpty(userEntity.FirstName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(userEntity.LastName))
            {
                return false;
            }

            if (!(userEntity.Sex.Equals("M", StringComparison.InvariantCultureIgnoreCase) || userEntity.Sex.Equals("F", StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }

            return true;
        }

        public static void CopyFrom(this User user, IUserEntity userEntity)
        {
            user.BirthDate = userEntity.BirthDate;
            user.Email = userEntity.Email;
            user.FirstName = userEntity.FirstName;
            user.Height = userEntity.Height;
            user.LastName = userEntity.LastName;
            user.MaxHr = userEntity.MaxHr;
            user.PostCity = userEntity.PostCity;
            user.PostCode = userEntity.PostCode;
            user.Sex = userEntity.Sex;
            user.Street = userEntity.Street;
        }
    }
}
