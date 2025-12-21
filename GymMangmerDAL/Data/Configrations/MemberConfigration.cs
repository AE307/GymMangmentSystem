using GymMangmerDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Data.Configrations
{
    internal class MemberConfigration:GymUserConfigration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x=>x.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GETDATE()");
            base.Configure(builder);

        }
    }
}
