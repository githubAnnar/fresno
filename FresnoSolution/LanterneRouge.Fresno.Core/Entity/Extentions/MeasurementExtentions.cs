using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Entity.Extentions
{
    public static class MeasurementExtentions
    {
        public static bool IsValid(this IMeasurementEntity measurementEntity)
        {
            if (measurementEntity == null)
            {
                return false;
            }

            if (measurementEntity.StepTestId == Guid.Empty)
            {
                return false;
            }

            if (measurementEntity.Sequence <= 0)
            {
                return false;
            }

            if (measurementEntity.HeartRate<=0)
            {
                return false;
            }

            if (measurementEntity.Lactate == default)
            {
                return false;
            }

            if (measurementEntity.Load == default)
            {
                return false;
            }

            return true;
        }
    }
}
