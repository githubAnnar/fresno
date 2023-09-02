using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Entity.Extentions
{
    public static class StepTestExtentions
    {
        public static bool IsValid(this IStepTestEntity stepTestEntity)
        {
            if (stepTestEntity == null)
            {
                return false;
            }

            if (stepTestEntity.UserId == Guid.Empty)
            {
                return false;
            }

            if (!(stepTestEntity.TestType.Equals("Bike", StringComparison.InvariantCultureIgnoreCase) || stepTestEntity.TestType.Equals("Run", StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }

            if (!(stepTestEntity.EffortUnit.Equals("W", StringComparison.InvariantCultureIgnoreCase) || stepTestEntity.EffortUnit.Equals("m-s", StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }

            if (stepTestEntity.LoadPreset == 0f)
            {
                return false;
            }

            if (stepTestEntity.Increase== 0f)
            {
                return false;
            }

            if (stepTestEntity.TestDate == default)
            {
                return false;
            }

            return true;
        }
    }
}
