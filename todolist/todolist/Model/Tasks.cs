using System.ComponentModel.DataAnnotations.Schema;

namespace todolist.Model
{
	public class Tasks
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }
		public string Description { get; set; }
		public bool IsFinished { get; set; }
		public string UserId { get; set; }
	
		public  Users User{ get; set; }

		
	}
}
