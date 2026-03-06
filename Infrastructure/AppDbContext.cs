using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // --- DbSets ---
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
        public DbSet<HostProfile> HostProfiles { get; set; }
        public DbSet<WorkSpacePromotion> WorkSpacePromotions { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. HỆ THỐNG BOOKING & REVIEW ---
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.Customer).WithMany(u => u.Bookings).HasForeignKey(b => b.CustomerId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(b => b.Guest).WithMany(g => g.Bookings).HasForeignKey(b => b.GuestId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(b => b.WorkSpaceRoom).WithMany(r => r.Bookings).HasForeignKey(b => b.WorkSpaceRoomId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(b => b.BookingStatus).WithMany(s => s.Bookings).HasForeignKey(b => b.BookingStatusId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(b => b.PaymentMethod).WithMany(p => p.Bookings).HasForeignKey(b => b.PaymentMethodID).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(b => b.BookingParticipants).WithOne(bp => bp.Booking).HasForeignKey(bp => bp.BookingId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(b => b.Reviews).WithOne(r => r.Booking).HasForeignKey(r => r.BookingId).OnDelete(DeleteBehavior.Cascade);
            });

            // --- 2. HỆ THỐNG WORKSPACE ---
            modelBuilder.Entity<WorkSpace>(entity =>
            {
                entity.HasOne(w => w.Address).WithMany(a => a.Workspaces).HasForeignKey(w => w.AddressId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w => w.WorkSpaceType).WithMany(t => t.Workspaces).HasForeignKey(w => w.WorkSpaceTypeId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w => w.Host).WithMany(h => h.Workspaces).HasForeignKey(w => w.HostId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(w => w.WorkSpaceRooms).WithOne(r => r.WorkSpace).HasForeignKey(r => r.WorkSpaceId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(w => w.WorkSpaceImages).WithOne(i => i.WorkSpace).HasForeignKey(i => i.WorkSpaceId).OnDelete(DeleteBehavior.Cascade);
            });

            // --- 3. HỆ THỐNG SUPPORT TICKET (QUAN TRỌNG) ---
            modelBuilder.Entity<SupportTicket>(entity =>
            {
                // Fix lỗi quan hệ 1-N với Replies
                entity.HasMany(t => t.Replies)
                      .WithOne(r => r.Ticket)
                      .HasForeignKey(r => r.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Quan hệ với người dùng & nhân viên
                entity.HasOne(t => t.SubmittedByUser).WithMany().HasForeignKey(t => t.SubmittedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.AssignedToStaff).WithMany().HasForeignKey(t => t.AssignedToStaffId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SupportTicketReply>()
                .HasOne(r => r.RepliedByUser).WithMany().HasForeignKey(r => r.RepliedByUserId).OnDelete(DeleteBehavior.Restrict);

            // --- 4. HỆ THỐNG PROMOTION (MANY-TO-MANY) ---
            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasOne(p => p.Host).WithMany(h => h.Promotions).HasForeignKey(p => p.HostId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<WorkSpacePromotion>(entity =>
            {
                entity.HasKey(wp => new { wp.WorkSpaceId, wp.PromotionId });
                entity.HasOne(wp => wp.WorkSpace).WithMany(w => w.WorkSpacePromotions).HasForeignKey(wp => wp.WorkSpaceId);
                entity.HasOne(wp => wp.Promotion).WithMany(p => p.WorkSpacePromotions).HasForeignKey(wp => wp.PromotionId);
            });

            // --- 5. CÁC QUAN HỆ CÒN LẠI ---
            modelBuilder.Entity<HostProfile>()
                .HasOne(h => h.User).WithOne(u => u.HostProfile).HasForeignKey<HostProfile>(h => h.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkSpaceFavorite>(entity =>
            {
                entity.HasOne(f => f.User).WithMany(u => u.WorkSpaceFavorites).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(f => f.Workspace).WithMany(w => w.WorkSpaceFavorites).HasForeignKey(f => f.WorkspaceId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Post>().HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>(entity =>
            {
                // Đổi Cascade thành Restrict để tránh vòng lặp xóa với WorkSpaceRoom
                entity.HasOne(r => r.WorkSpaceRoom)
                      .WithMany(wr => wr.Reviews)
                      .HasForeignKey(r => r.WorkSpaceRoomId)
                      .OnDelete(DeleteBehavior.Restrict); // <--- THAY ĐỔI Ở ĐÂY

                entity.HasOne(r => r.Booking)
                      .WithMany(b => b.Reviews)
                      .HasForeignKey(r => r.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // --- Cấu hình cho hệ thống Chat ---
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(c => c.Id);

                // Quan hệ với Customer: Dùng Restrict để tránh lỗi vòng lặp xóa (Multiple Cascade Paths)
                entity.HasOne(c => c.Customer)
                      .WithMany(u => u.CustomerConversations)
                      .HasForeignKey(c => c.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Quan hệ với Owner: Dùng Restrict
                entity.HasOne(c => c.Owner)
                      .WithMany(u => u.OwnerConversations)
                      .HasForeignKey(c => c.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(m => m.Id);

                // Một cuộc hội thoại có nhiều tin nhắn: Xóa hội thoại thì xóa tin nhắn (Cascade)
                entity.HasOne(m => m.Conversation)
                      .WithMany(c => c.Messages)
                      .HasForeignKey(m => m.ConversationId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Người gửi tin nhắn
                entity.HasOne(m => m.Sender)
                      .WithMany()
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.WorkSpace).WithMany(w => w.Notifications).HasForeignKey(n => n.WorkSpaceId).OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Amenity>().HasMany(a => a.WorkspaceRoomAmenities).WithOne(wra => wra.Amenity).HasForeignKey(wra => wra.AmenityId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlockedTimeSlot>().HasOne(bts => bts.WorkSpaceRoom).WithMany(wr => wr.BlockedTimeSlots).HasForeignKey(bts => bts.WorkSpaceRoomId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}