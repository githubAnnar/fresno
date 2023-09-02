using LanterneRouge.Fresno.Core.Interface;

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
    }
}
