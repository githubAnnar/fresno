using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LanterneRouge.Fresno.Core.Configuration
{
    public class StepTestConfig : IEntityTypeConfiguration<StepTest>
    {
        public void Configure(EntityTypeBuilder<StepTest> entity)
        {
            var orderCounter = 0;

            entity.ToTable("StepTest", b =>
            {
                b.HasCheckConstraint("CK_TESTTYPE", "[TestType] = 'Bike' OR [TestType] = 'Run'");
                b.HasCheckConstraint("CK_EFFORTUNIT", "[EffortUnit] = 'W' OR [EffortUnit] = 'm-s'");
            })
              .HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedNever()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.TestType)
                .HasDefaultValue("Bike")
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.EffortUnit)
                .HasDefaultValue("W")
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.StepDuration)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.LoadPreset)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.Increase)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.HasMany(e => e.Measurements)
                .WithOne(e => e.StepTest)
                .HasForeignKey(e => e.StepTestId);
        }
    }
}
