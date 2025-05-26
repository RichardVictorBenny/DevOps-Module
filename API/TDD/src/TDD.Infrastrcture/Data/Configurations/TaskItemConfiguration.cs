using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastructure.Data.Entities;

namespace TDD.Infrastructure.Data.Configurations
{

    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> entity)
        {
            entity.HasKey(x => x.Id).IsClustered(false);
            entity.ToTable(nameof(TaskItem));

            entity.Property(x => x.Title)
                .HasMaxLength(250)
                .IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastModifiedBy).IsRequired();
            entity.Property(e => e.LastModifiedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(x => x.User)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
