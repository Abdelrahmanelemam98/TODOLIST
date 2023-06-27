using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace todolist.Model
{
	public class taskDbContext : IdentityDbContext<Users>
	{
		public DbSet<Users> User { get; set; }
		public DbSet <Tasks> Task { get; set; }
		public taskDbContext()
		{

		}
		public taskDbContext(DbContextOptions<taskDbContext>options):base(options)
		{

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=. ;Database=TODOLIST_4;Trusted_Connection=True;TrustServerCertificate=True;");
		}
	}
}
