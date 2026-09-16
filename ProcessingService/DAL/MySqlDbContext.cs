using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProcessingService.Models.DTOs;

namespace ProcessingService.DAL
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
            
        }
        public DbSet<StationInformationDTO> StationsInformation {get; set;}
        public DbSet<VehicleTypesDTO> VehicleTypes {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StationInformationDTO>(e=> e.HasKey(si=> si.StationId));
        }
    }
}