using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LanterneRouge.Fresno.Core.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            var orderCounter = 0;

            entity.ToTable("User", b => b.HasCheckConstraint("CK_SEX","[Sex] = 'M' OR [Sex] = 'F'"))
                .HasKey(x => x.Id);

            entity.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnOrder(orderCounter++);

            entity.HasMany(e => e.StepTests)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        }
    }
}
