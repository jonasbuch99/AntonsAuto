using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AntonsAuto.Models;

namespace AntonsAuto.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Bilfabrikant> Bilfabritants { get; set; }
        public DbSet<BilModel> BilModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bilfabrikant>().ToTable("Bilfabrikant");
            modelBuilder.Entity<BilModel>().ToTable("BilModel");

        }
    }
}
