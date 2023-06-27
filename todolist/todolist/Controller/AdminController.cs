using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using todolist.DTOS;
using todolist.Model;

namespace todolist.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly taskDbContext context;

		public AdminController(taskDbContext _context)
		{
			context =_context;

		}
		[HttpGet("tasks")]
		[Authorize(Roles = "Admin")]
		public ActionResult getAllTaskAdmin() 
		{
			var tasks = context.Task.Select(e=> new getAllAdminTaskDto { Description= e.Description ,UserName =e.User.FirstName ,IshFinished =e.IsFinished }).ToList();

		    
			  return Ok(tasks);
		}
		[HttpPost]
		[Authorize(Roles = "Admin")]

		public IActionResult AddTaskAdmin(AddTaskAdminDto taskAdmin, string userId)
		{
			var user = context.Users.Find(userId);
			if (user == null)
			{

				return NotFound();
			}
			var task = new Tasks
			{
				UserId=user.Id,
				Description= taskAdmin.Description,
				IsFinished=taskAdmin.IsFinished,

			};
			context.Task.Add(task);
			context.SaveChanges();

			return Ok("Created");

		}

		[HttpPatch]
		[Authorize(Roles = "Admin")]

		public IActionResult updateTaskAdmin(string taskid, AdminUpdateDto AdminDto)
		{
			var tasks = context.Task.Find(taskid);

			if (tasks == null)
			{
				return NotFound();
			}
			
			tasks.IsFinished = AdminDto.IsFinished;
			context.SaveChanges();

			return Ok("Update");
		}
		[HttpDelete]
		[Authorize(Roles = "Admin")]

		public IActionResult DeletTaskAdmin(string taskId)
		{
			var task = context.Task.Find(taskId);
			if (task == null)
			{
				return NotFound();
			}
			context.Task.Remove(task);
			context.SaveChanges();

			return Ok("Deleted");
		}
	}
}
