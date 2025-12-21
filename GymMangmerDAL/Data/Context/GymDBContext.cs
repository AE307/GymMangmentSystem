using GymMangmerDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Data.Context
{
    public class GymDBContext : DbContext
    {
        public GymDBContext(DbContextOptions<GymDBContext> option) : base(option)
        {

        }
        //override protected void onconfiguring(dbcontextoptionsbuilder optionsbuilder)
        //{
        //    optionsbuilder.usesqlserver("server=.;database=gymmanagementdb;trusted_connection=true;trustservercertificate=true;");
        //}
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        #region DbSets
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
        #endregion
    }
}
