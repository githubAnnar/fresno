using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LanterneRouge.Fresno.Core.Configuration
{
    public class StepTestConfig : IEntityTypeConfiguration<StepTest>
    {
        public void Configure(EntityTypeBuilder<StepTest> entity)
        {
            entity.ToTable("StepTest", b => b.HasCheckConstraint("CK_TESTTYPE", "[TestType] = 'Bike' OR [TestType] = 'Run'"))
                .HasKey(t => t.Id);

            entity.ToTable("CK_EFFORTUNIT", "[EffortUnit] = 'W' OR [EffortUnit] = 'm-s'");

            entity.Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.Property(e => e.TestType)
                .HasDefaultValue("Bike")
                .IsRequired();

            entity.Property(e => e.EffortUnit)
                .HasDefaultValue("W")
                .IsRequired();

            entity.Property(e => e.StepDuration)
                .IsRequired();

            entity.Property(e => e.LoadPreset)
                .IsRequired();

            entity.Property(e => e.Increase)
                .IsRequired();

            entity.HasMany(e => e.Measurements)
                .WithOne(e => e.StepTest)
                .HasForeignKey(e => e.StepTestId);
        }
    }
}
