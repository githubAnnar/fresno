using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LanterneRouge.Fresno.Core.Configuration
{
    public class MeasurementConfig : IEntityTypeConfiguration<Measurement>
    {
        public void Configure(EntityTypeBuilder<Measurement> entity)
        {
            int orderCounter = 0;
            entity.ToTable("Measurement")
                .HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.Sequence)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.StepTestId)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.HeartRate)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.Lactate)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.Load)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.InCalculation)
                .HasDefaultValue(true)
                .HasColumnOrder(orderCounter++);
        }
    }
}
