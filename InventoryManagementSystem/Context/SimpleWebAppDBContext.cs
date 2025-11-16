using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace InventoryManagementSystem.Context
{
    public class SimpleWebAppDBContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public SimpleWebAppDBContext(DbContextOptions<SimpleWebAppDBContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
