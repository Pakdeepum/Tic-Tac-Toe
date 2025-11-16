using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tictactoe.Service.Models.Entities;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<MUser> MUser { get; set; }
    public DbSet<UserScoreLog> UserScoreLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MUser>().ToTable("M_USER");

        modelBuilder.Entity<UserScoreLog>().ToTable("USER_SCORE_LOG");
    }
}
