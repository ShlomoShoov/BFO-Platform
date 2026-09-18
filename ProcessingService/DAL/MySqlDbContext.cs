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
        public DbSet<StationStatusDTO> StationStatuses {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StationInformationDTO>(e=> 
            {
                e.HasKey(si=> si.StationId);
                e.HasOne(si=> si.StationStatus)
                    .WithOne(ss=> ss.StationInformation)
                        .HasForeignKey<StationStatusDTO>(s=> s.StationId);
                
            }
            );
            modelBuilder.Entity<StationStatusDTO>(e =>
            {
                e.HasKey(ss => ss.StationId);

            }
            );

            modelBuilder.Entity<VehicleTypesDTO>(e=> e.HasKey(v=> v.VehicleTypeId));

        }
    }
}