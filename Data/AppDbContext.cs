using Microsoft.EntityFrameworkCore;

namespace IdentityLesson.Data {
	public class AppDbContext : DbContext {
		public AppDbContext(DbContextOptions<AppDbContext> opts)
			: base(opts) {
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		public DbSet<User> Users { get; set; }
	}
}
