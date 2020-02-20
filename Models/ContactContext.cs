using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ContactApi2.Models;

namespace ContactApi2.Models
{
    public class ContactContext : DbContext
    {
        public DbSet<UserContact> UserContacts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=contactdatabase;Username=nadulang;Password=brockhampton");
    
    }
}