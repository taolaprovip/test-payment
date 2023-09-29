using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public partial class RePurposeContext : DbContext
    {
        public RePurposeContext()
        {
        }

        public RePurposeContext(DbContextOptions<RePurposeContext> options)
            : base(options)
        {
        }
       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Server=database.monoinfinity.net, 1433;Database=RePurpose;Persist Security Info=False;User ID=sa;Password=1234567890Aa;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
            optionsBuilder.UseSqlServer(connection);
        }*/

        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>().HasKey(w => w.WalletId);
            modelBuilder.Entity<Transaction>().HasKey(t => t.TransactionId);
            modelBuilder.Entity<Transaction>()
            .Property(e => e.TransactionId)
            .ValueGeneratedOnAdd();
            // Thiết lập mối quan hệ giữa Wallet và Transaction
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletId);
            modelBuilder.Entity<Wallet>()
               .HasOne(w => w.Member)
               .WithOne(m => m.Wallet)
               .HasForeignKey<Wallet>(w => w.MemberId);



            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(cat => cat.Id);
                entity.Property(cat => cat.Name).IsRequired();
                entity.Property(cat => cat.IsDeleted).IsRequired();
                entity.Property(cat => cat.PointX).IsRequired();
                entity.Property(cat => cat.PointY).IsRequired();

               
                
                   
            });
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(loc => loc.Id);
                entity.Property(loc => loc.Latitude).IsRequired();
                entity.Property(loc => loc.Longitude).IsRequired();

                entity.HasOne(loc => loc.Member)
                    .WithOne(member => member.Location)
                    .HasForeignKey<Location>(loc => loc.LocationMember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .IsRequired(false);
                


                entity.HasMany(loc => loc.Items)
                    .WithOne(item => item.Location)
                    .HasForeignKey(item => item.ItemLocation)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false); 
                   
            });
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(mem => mem.Id);
                entity.Property(mem => mem.Name).IsRequired();
                entity.Property(mem => mem.Email).IsRequired();
                entity.Property(mem => mem.PhoneNumberConfirmed).IsRequired();
                entity.Property(mem => mem.EmailConfirmed).IsRequired();
                entity.Property(mem => mem.IsDeleted).IsRequired();
                entity.Property(mem => mem.CreateAt).IsRequired();



                entity.HasMany(mem => mem.ItemsGiven)
                    .WithOne(item => item.Giver)
                    .HasForeignKey(item => item.GiverId)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false); 


                entity.HasMany(mem => mem.ItemsReceived)
                    .WithOne(item => item.Receiver)
                    .HasForeignKey(item => item.ReceiverId)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false); 


                entity.HasMany(mem => mem.RefreshTokens)
                    .WithOne(rt => rt.Member)
                    .HasForeignKey(rt => rt.TokenMember)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false); 
                   

            });
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Name).IsRequired();
                entity.Property(item => item.Type).IsRequired();
                entity.Property(item => item.Quantity).IsRequired();
                entity.Property(item => item.IsDeleted).IsRequired();
                entity.Property(item => item.Created).IsRequired();
                entity.Property(item => item.PickupTime).IsRequired();

                entity.HasOne(item => item.Giver)
                    .WithMany(member => member.ItemsGiven)
                    .HasForeignKey(item => item.GiverId)

                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false);


                entity.HasOne(item => item.Receiver)
                    .WithMany(member => member.ItemsReceived)
                    .HasForeignKey(item => item.ReceiverId)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false);

                entity.HasOne(item => item.Location)
                    .WithMany(loc => loc.Items)
                    .HasForeignKey(item => item.ItemLocation)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false);
                entity.HasOne(item => item.Category)
                .WithMany(ca => ca.Items)
                .HasForeignKey(item => item.CategoryId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
                
            });
            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(img => img.Id);
                entity.Property(img => img.ImageUrl).IsRequired();

                entity.HasOne(img => img.Item)
                    .WithMany(item => item.Images)
                    .HasForeignKey(img => img.ItemImage)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false);
                
            });
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.TokenValue).IsRequired();
                entity.Property(rt => rt.ExpiresAt).IsRequired();
                entity.Property(rt => rt.IssuedAt).IsRequired();
                entity.Property(rt => rt.IsActive).IsRequired();

                entity.HasOne(rt => rt.Member)
                    .WithMany(mem => mem.RefreshTokens)
                    .HasForeignKey(rt => rt.TokenMember)
                     .OnDelete(DeleteBehavior.NoAction)
                     .IsRequired(false);
                
            });
        }
    }
}
