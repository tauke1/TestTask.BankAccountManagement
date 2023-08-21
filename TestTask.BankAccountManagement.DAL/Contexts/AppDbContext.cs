using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Interceptors;
using TestTask.BankAccountManagement.DAL.Models;

namespace TestTask.BankAccountManagement.DAL.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOperation> AccountOperations { get; set; }
        public DbSet<AccountTypeSettings> AccountTypeSettings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Country> Countries { get; set; }

        public AppDbContext(DbContextOptions opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Account>().HasMany(a => a.CreditTransactions).WithOne(t => t.FromAccount);
            modelBuilder.Entity<Account>().HasMany(a => a.DebitTransactions).WithOne(t => t.ToAccount);
            modelBuilder.Entity<Account>().HasMany(a => a.Operations).WithOne(t => t.Account);
            modelBuilder.Entity<Account>().HasOne(a => a.Country).WithMany(c => c.Accounts);
            modelBuilder.Entity<Account>().HasIndex(a => a.Iban).IsUnique();
            modelBuilder.Entity<Account>().Property(a => a.Iban).IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasColumnType("decimal(28,2)");
            modelBuilder.Entity<Account>().HasOne(a => a.CreatedByManager).WithMany(m => m.CreatedAccounts).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Country>().HasKey(c => c.Id);
            modelBuilder.Entity<Country>().HasMany(c => c.Accounts).WithOne(a => a.Country);
            modelBuilder.Entity<Country>().Property(c => c.Code).IsRequired();
            modelBuilder.Entity<Country>().HasIndex(c => new { c.Code }).IsUnique();
            modelBuilder.Entity<Country>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Country>().HasIndex(c => new { c.Name }).IsUnique();

            modelBuilder.Entity<AccountTypeSettings>().HasKey(s => s.Id);
            modelBuilder.Entity<AccountTypeSettings>().HasIndex(s => new { s.AccountType }).IsUnique();

            modelBuilder.Entity<AccountOperation>().HasOne(o => o.Account);
            modelBuilder.Entity<AccountOperation>().HasOne(o => o.Transaction);
            modelBuilder.Entity<AccountOperation>().Property(o => o.Amount).HasColumnType("decimal(28,2)");
            modelBuilder.Entity<AccountOperation>().Property(o => o.BalanceBefore).HasColumnType("decimal(28,2)");

            modelBuilder.Entity<Transaction>().Property(t => t.ReferenceRetrievalNumber).IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(28,2)");
            modelBuilder.Entity<Transaction>().HasIndex(t => t.ReferenceRetrievalNumber).IsUnique();
            modelBuilder.Entity<Transaction>().HasOne(t => t.TriggeredByManager);
            modelBuilder.Entity<Transaction>().HasOne(t => t.FromAccount).WithMany(a => a.CreditTransactions).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(t => t.ToAccount).WithMany(a => a.DebitTransactions).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Manager>().HasKey(m => m.Id);
            modelBuilder.Entity<Manager>().Property(m => m.Login).IsRequired();
            modelBuilder.Entity<Manager>().HasIndex(m => new { m.Login }).IsUnique();
            modelBuilder.Entity<Manager>().Property(m => m.PinHashed).IsRequired();
            modelBuilder.Entity<Manager>().HasMany(m => m.CreatedAccounts).WithOne(a => a.CreatedByManager).OnDelete(DeleteBehavior.Restrict);

            // soft delete behaviour
            modelBuilder.Entity<Account>()
                .HasQueryFilter(a => !a.DeletedAt.HasValue);
            modelBuilder.Entity<Transaction>()
                .HasQueryFilter(t => !t.DeletedAt.HasValue);
        }
    }
}
