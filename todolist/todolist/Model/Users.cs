using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace todolist.Model
{
	public class Users:IdentityUser
	{
		
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Role { get; set; }
		public string? ProfileImg { get; set; }
		
/*		public virtual ICollection<Tasks> Tasks { get; set; }
*/
	}
}
