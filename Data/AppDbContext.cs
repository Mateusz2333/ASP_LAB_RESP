﻿using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDbContext: IdentityDbContext<IdentityUser>
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }

        public DbSet<BookEntity> Books { get; set; }
        private string Path { get; set; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            Path = System.IO.Path.Join(path, "contacts.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data source={Path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//last lab
            var user = new IdentityUser()
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "bartek",
                NormalizedUserName = "BARTEK",
                Email = "bartek@wsei.pl",
                NormalizedEmail = "BARTEK@WSEI.PL",
                EmailConfirmed = true,
            };
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Bartek123!");

            modelBuilder.Entity<IdentityUser>()
                .HasData(user);

            var adminRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
            };

            adminRole.ConcurrencyStamp = adminRole.Id;
            //skojarzenie uzytkownika z rola admina
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(
                 new IdentityUserRole<string>()
                 {
                     RoleId = adminRole.Id,
                     UserId = user.Id,
                 }
                );
            modelBuilder.Entity<IdentityRole>()
                .HasData(adminRole);




            /////////////////////////////////////////////////////////

            modelBuilder.Entity<ContactEntity>()
                .HasOne(c => c.Organization)
                .WithMany(o => o.Contacts)
                .HasForeignKey(c => c.OrganizationId);

            modelBuilder.Entity<OrganizationEntity>().HasData
            (
                new OrganizationEntity() { Id = 101, Name = "WSEI", Description = "Uczelnia wyższa" },
                new OrganizationEntity() { Id = 102, Name = "Comarch", Description = "Przedsiębiorstwo IT" }
            );

            modelBuilder.Entity<ContactEntity>().HasData
            (
                new ContactEntity() { ContactId = 1, Name = "Adam", Email = "adam@wsei.edu.pl", Phone = "124124234", Birth = DateTime.Parse("2000-10-10"), OrganizationId = 101 },
                new ContactEntity() { ContactId = 2, Name = "Adam", Email = "adam@wsei.edu.pl", Phone = "124124234", Birth = DateTime.Parse("2000-10-10"), OrganizationId = 102 },
                new ContactEntity() { ContactId = 3, Name = "Adam", Email = "adam@wsei.edu.pl", Phone = "124124234", Birth = DateTime.Parse("2000-10-10"), OrganizationId = 102 }
            );

            modelBuilder.Entity<OrganizationEntity>()
                .OwnsOne(o => o.Adress)
                .HasData
                (
                    new { OrganizationEntityId = 101, City = "Kraków", Street = "św. Filipa 17", PostalCode = "31-150" },
                    new { OrganizationEntityId = 102, City = "Kraków", Street = "Rozwoju 1/4", PostalCode = "36-160" }
                );
        }


    }
}
