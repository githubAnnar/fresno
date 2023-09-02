using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LanterneRouge.Fresno.Core.Configuration
{
    public class MeasurementConfig : IEntityTypeConfiguration<Measurement>
    {
        public void Configure(EntityTypeBuilder<Measurement> entity)
        {
            entity.ToTable("Measurement")
                .HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.Sequence)
                .IsRequired();

            entity.Property(e => e.StepTestId)
                .IsRequired();

            entity.Property(e => e.HeartRate)
                .IsRequired();

            entity.Property(e => e.Lactate)
                .IsRequired();

            entity.Property(e => e.Load)
                .IsRequired();

            entity.Property(e => e.InCalculation)
                .HasDefaultValue(true);
        }
    }
}
