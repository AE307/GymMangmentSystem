using GymMangmerDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Data.Configrations
{
    internal class PlanConfigration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Description).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Price).HasPrecision(10,2);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationCheck", "DurationDays  Between 1 and 365");
            });
        }
    }
}
