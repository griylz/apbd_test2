using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using test2.Models;

namespace test2.Context;

public class ApplicationContext : DbContext
{
    protected ApplicationContext()
    {
    }
    public ApplicationContext(DbContextOptions options) : base(options) 
    { 
    } 
    public DbSet<Backpacks> Backpacks { get; set; }
    public DbSet<Characters?> Characters { get; set; }
    public DbSet<CharacterTitles> CharacterTitles { get; set; }
    public DbSet<Items> Items { get; set; }
    public DbSet<Titles> Titles { get; set; }
}