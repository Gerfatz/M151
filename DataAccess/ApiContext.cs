using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
