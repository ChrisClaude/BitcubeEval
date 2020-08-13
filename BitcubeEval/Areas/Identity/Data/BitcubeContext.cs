using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcubeEval.Areas.Identity.Data;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BitcubeEval.Data
{
    public class BitcubeContext : IdentityDbContext<BitcubeUser>
    {
        public BitcubeContext(DbContextOptions<BitcubeContext> options)
            : base(options)
        {
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Friendship>()
                .HasKey(f => new { f.UserId, f.FriendUserId});
            
        }
    }
}
