using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<BlockedTimeSlot> BlockedTimeSlots { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingParticipant> BookingParticipants { get; set; }
        public DbSet<BookingStatus> BookingStatus { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<SupportTicketReply> SupportTicketReplies { get; set; }
        public DbSet<WorkSpace> WorkSpaces { get; set; }
        public DbSet<WorkSpaceFavorite> WorkSpaceFavorites { get; set; }
        public DbSet<WorkSpaceImage> WorkSpaceImages { get; set; }
        public DbSet<WorkSpaceRoom> WorkSpaceRooms { get; set; }
        public DbSet<WorkSpaceRoomAmenity> WorkSpaceRoomAmenities { get; set; }
        public DbSet<WorkSpaceRoomImage> WorkSpaceRoomImages { get; set; }
        public DbSet<WorkSpaceRoomType> WorkSpaceRoomTypes { get; set; }
        public DbSet<WorkSpaceType> WorkSpaceTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.Customer).WithMany(b => b.Bookings).HasForeignKey(b => b.CustomerId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(b => b.Guest).WithMany(g => g.Bookings).HasForeignKey(b => b.GuestId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(b => b.WorkSpaceRoom).WithMany(b => b.Bookings).HasForeignKey(b => b.WorkSpaceRoomId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(b => b.BookingStatus).WithMany(b => b.Bookings).HasForeignKey(b => b.BookingStatusId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(b => b.PaymentMethod).WithMany(b => b.Bookings).HasForeignKey(b => b.PaymentMethodID).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(b => b.BookingParticipants).WithOne(bp => bp.Booking).HasForeignKey(bp => bp.BookingId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(b => b.Reviews).WithOne(r => r.Booking).HasForeignKey(r => r.BookingId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WorkSpace>(entity =>
            {
                entity.HasOne(w => w.Address).WithMany(a => a.Workspaces).HasForeignKey(w => w.AddressId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w => w.WorkSpaceType).WithMany(t => t.Workspaces).HasForeignKey(w => w.WorkSpaceTypeId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w => w.Host).WithMany(h => h.Workspaces).HasForeignKey(w => w.HostId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(w => w.WorkSpaceRooms).WithOne(r => r.WorkSpace).HasForeignKey(r => r.WorkSpaceId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(w => w.WorkSpaceImages).WithOne(i => i.WorkSpace).HasForeignKey(i => i.WorkSpaceId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WorkSpaceRoom>(entity =>
            {
                entity.HasOne(r => r.WorkSpaceRoomType).WithMany(t => t.WorkSpaceRooms).HasForeignKey(r => r.WorkSpaceRoomTypeId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(r => r.WorkSpaceRoomAmenities).WithOne(a => a.WorkSpaceRoom).HasForeignKey(a => a.WorkSpaceRoomId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(r => r.WorkSpaceRoomImages).WithOne(i => i.WorkSpaceRoom).HasForeignKey(i => i.WorkSpaceRoomId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(r => r.Reviews).WithOne(r => r.WorkSpaceRoom).HasForeignKey(r => r.WorkSpaceRoomId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SupportTicket>().HasOne(u => u.AssignedToStaff).WithMany().HasForeignKey(u => u.AssignedToStaffId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SupportTicketReply>().HasOne(r => r.RepliedByUser).WithMany().HasForeignKey(r => r.RepliedByUserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Post>().HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkSpaceFavorite>().HasOne(f => f.User).WithMany(u => u.WorkSpaceFavorites).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkSpaceFavorite>().HasOne(f => f.Workspace).WithMany(w => w.WorkSpaceFavorites).HasForeignKey(f => f.WorkspaceId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HostProfile>().HasOne(h => h.User).WithOne(u => u.HostProfile).HasForeignKey<HostProfile>(h => h.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Promotion>().HasOne(h => h.Host).WithMany().HasForeignKey(h => h.HostId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Amenity>().HasMany(a => a.WorkspaceRoomAmenities).WithOne(wra => wra.Amenity).HasForeignKey(wra => wra.AmenityId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BlockedTimeSlot>().HasOne(bts => bts.WorkSpaceRoom).WithMany(wr => wr.BlockedTimeSlots).HasForeignKey(bts => bts.WorkSpaceRoomId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}