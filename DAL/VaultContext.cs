using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class VaultContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}