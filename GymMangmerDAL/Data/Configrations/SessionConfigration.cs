using GymMangmerDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Data.Configrations
{
    internal class SessionConfigration : IEntityTypeConfiguration<Session>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                tb.HasCheckConstraint("SessionEndDate", "EndDate > StartDate");
            });

            builder.HasOne(x=>x.SessionCategory).WithMany(x=>x.Sessions).HasForeignKey(x=>x.CategoryId);
            builder.HasOne(x=>x.SessionTrainer).WithMany(x=>x.TrainerSession).HasForeignKey(x=>x.TrainerId);
        }
    }
}
