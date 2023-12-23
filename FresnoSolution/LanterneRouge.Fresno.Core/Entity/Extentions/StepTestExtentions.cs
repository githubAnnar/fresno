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

            if (stepTestEntity.Increase == 0f)
            {
                return false;
            }

            if (stepTestEntity.TestDate == default)
            {
                return false;
            }

            return true;
        }

        public static void CopyFrom(this StepTest stepTest, IStepTestEntity stepTestEntity)
        {
            stepTest.EffortUnit = stepTestEntity.EffortUnit;
            stepTest.Increase = stepTestEntity.Increase;
            stepTest.LoadPreset = stepTestEntity.LoadPreset;
            stepTest.StepDuration = stepTestEntity.StepDuration;
            stepTest.Temperature = stepTestEntity.Temperature;
            stepTest.TestDate = stepTestEntity.TestDate;
            stepTest.TestType = stepTestEntity.TestType;
            stepTest.Weight = stepTestEntity.Weight;
        }
    }
}
