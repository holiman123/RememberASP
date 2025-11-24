using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RememberASP.Models;

namespace RememberASP;

public class LettersAppDbContext : IdentityDbContext<ApplicationUser>
{
    public string ConnectionStr { get; set; }

    public LettersAppDbContext(string connectionStr)
    {
        ConnectionStr = connectionStr;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionStr);

        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<LetterModel> Letters { get; set; }


}