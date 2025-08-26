using System.ComponentModel;
using CMS.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace CMS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetRequest> AssetRequests { get; set; }
        public DbSet<AssetMaintenance> AssetMaintenances { get; set; }
        public DbSet<AssetLog> AssetLogs { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseDocument> ExpenseDocuments { get; set; }
        public DbSet<Reimbursement> Reimbursements { get; set; }
        public DbSet<ComplaintSuggestion> ComplaintSuggestions { get; set; }
        public DbSet<LedgerEntry> LedgerEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department relationships
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Asset relationships
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.AssignedUser)
                .WithMany(u => u.AssignedAssets)
                .HasForeignKey(a => a.AssignedTo)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Department)
                .WithMany(d => d.Assets)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Asset Serial Number unique constraint
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.SerialNumber)
                .IsUnique()
                .HasFilter("[SerialNumber] IS NOT NULL");

            // AssetRequest relationships
            modelBuilder.Entity<AssetRequest>()
                .HasOne(ar => ar.User)
                .WithMany(u => u.AssetRequests)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetRequest>()
                .HasOne(ar => ar.Department)
                .WithMany(d => d.AssetRequests)
                .HasForeignKey(ar => ar.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetRequest>()
                .HasOne(ar => ar.ApprovedByUser)
                .WithMany()
                .HasForeignKey(ar => ar.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // AssetMaintenance relationships
            modelBuilder.Entity<AssetMaintenance>()
                .HasOne(am => am.Asset)
                .WithMany(a => a.AssetMaintenances)
                .HasForeignKey(am => am.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssetMaintenance>()
                .HasOne(am => am.ReportedByUser)
                .WithMany(u => u.ReportedMaintenances)
                .HasForeignKey(am => am.ReportedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetMaintenance>()
                .HasOne(am => am.AssignedToUser)
                .WithMany(u => u.AssignedMaintenances)
                .HasForeignKey(am => am.AssignedTo)
                .OnDelete(DeleteBehavior.SetNull);

            // AssetLog relationships
            modelBuilder.Entity<AssetLog>()
                .HasOne(al => al.Asset)
                .WithMany(a => a.AssetLogs)
                .HasForeignKey(al => al.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssetLog>()
                .HasOne(al => al.PerformedByUser)
                .WithMany()
                .HasForeignKey(al => al.PerformedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Expense relationships
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Expenses)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ReviewedByUser)
                .WithMany()
                .HasForeignKey(e => e.ReviewedById)
                .OnDelete(DeleteBehavior.SetNull);

            // ExpenseDocument relationships
            modelBuilder.Entity<ExpenseDocument>()
                .HasOne(ed => ed.Expense)
                .WithMany(e => e.ExpenseDocuments)
                .HasForeignKey(ed => ed.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reimbursement relationships
            modelBuilder.Entity<Reimbursement>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reimbursements)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reimbursement>()
                .HasOne(r => r.Department)
                .WithMany(d => d.Reimbursements)
                .HasForeignKey(r => r.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reimbursement>()
                .HasOne(r => r.ApprovedByUser)
                .WithMany()
                .HasForeignKey(r => r.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // ComplaintSuggestion relationships
            modelBuilder.Entity<ComplaintSuggestion>()
                .HasOne(cs => cs.SubmittedByUser)
                .WithMany(u => u.ComplaintSuggestions)
                .HasForeignKey(cs => cs.SubmittedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComplaintSuggestion>()
                .HasOne(cs => cs.Department)
                .WithMany(d => d.ComplaintSuggestions)
                .HasForeignKey(cs => cs.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComplaintSuggestion>()
                .HasOne(cs => cs.AssignedToUser)
                .WithMany()
                .HasForeignKey(cs => cs.AssignedTo)
                .OnDelete(DeleteBehavior.SetNull);

            // LedgerEntry relationships
            modelBuilder.Entity<LedgerEntry>()
                .HasOne(le => le.Department)
                .WithMany(d => d.LedgerEntries)
                .HasForeignKey(le => le.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LedgerEntry>()
                .HasOne(le => le.PostedByUser)
                .WithMany()
                .HasForeignKey(le => le.PostedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // UserPermission relationships
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.GrantedByUser)
                .WithMany()
                .HasForeignKey(up => up.GrantedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Unique constraints
            modelBuilder.Entity<UserPermission>()
                .HasIndex(up => new { up.UserId, up.Permission })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Name)
                .IsUnique();

            // Configure arrays for PostgreSQL
            modelBuilder.Entity<Reimbursement>()
                .Property(r => r.ReceiptUrls)
                .HasColumnType("text[]");

            // Configure decimal precision
            modelBuilder.Entity<Asset>()
                .Property(a => a.Value)
                .HasPrecision(15, 2);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<Reimbursement>()
                .Property(r => r.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<AssetMaintenance>()
                .Property(am => am.EstimatedCost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AssetMaintenance>()
                .Property(am => am.ActualCost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<LedgerEntry>()
                .Property(le => le.DebitAmount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<LedgerEntry>()
                .Property(le => le.CreditAmount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<Department>()
                .Property(d => d.Budget)
                .HasPrecision(15, 2);
        }
    }
}
