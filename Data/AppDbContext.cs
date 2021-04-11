using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityLesson.Data {
	public class AppDbContext : IdentityDbContext<AppUser> {
		public AppDbContext(DbContextOptions<AppDbContext> opts)
			: base(opts) {
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}
	}
}
