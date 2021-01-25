using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StudentPlatformContext : IdentityDbContext
    {
        public StudentPlatformContext(DbContextOptions<StudentPlatformContext> options)
            : base(options)
        {

        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Test> Tests { get; set; }
    }
}
