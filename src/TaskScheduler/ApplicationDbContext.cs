using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace TaskScheduler
{
    public class ApplicationDbContext : DbContext
    {
        // public DbSet<QueueEntry> QueueTable { get; set; }

        public DbSet<SimpleQueueBufferEntry> QueueBuffer { get; set; }

        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options)
        {

        }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating (builder);
        }
    }
}