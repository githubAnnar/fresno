using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LanterneRouge.Fresno.Core.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User", b => b.HasCheckConstraint("CK_SEX","[Sex] = 'M' OR [Sex] = 'F'"))
                .HasKey(x => x.Id);

            entity.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.FirstName)
                .IsRequired();

            entity.Property(e => e.LastName)
                .IsRequired();

            entity.Property(e => e.Email)
                .IsRequired();

            entity.HasMany(e => e.StepTests)
                .WithOne(e => e.ParentUser)
                .HasForeignKey(e => e.UserId);
        }
    }
}
