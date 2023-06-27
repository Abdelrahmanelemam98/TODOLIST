using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using todolist.DTOS;
using todolist.Model;
using Task = todolist.Model.Tasks;

namespace todolist.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly taskDbContext context;

		public TaskController(taskDbContext _context)
		{
			context =_context;
		}
		[HttpGet("{id}")]
		[Authorize(Roles = "User")]
		public IActionResult getTaskByUserId(string id)
		{
			List<Tasks> getAllTask = context.Task.Where(e => e.Id ==id).ToList();

			var allTask = new getTasks();

			foreach(var item in getAllTask)
			{
				item.Description = allTask.Description;
				item.IsFinished = allTask.isFinished;
				item.User.FirstName = allTask.UserName;
			}
			return Ok(allTask);

		}
		[HttpPost]
		[Authorize(Roles = "User")]
		public IActionResult AddTask(AddTaskDto taskDto , string userId)
		{
			var user = context.Users.Find(userId);
			if (user == null)
			{

				return NotFound();
			}
			var task = new Tasks
			{
				UserId=user.Id,
				Description= taskDto.Description,
				IsFinished=taskDto.UnFinished,

			};
			context.Task.Add(task);
			context.SaveChanges();

			return Ok("Created");

		}
		[HttpPut]
		[Authorize(Roles = "User")]
		public  IActionResult updateTask(string taskid, updateDto update)
		{
			var tasks = context.Task.Find(taskid);

			if (tasks == null)
			{
				return NotFound();
			}
			tasks.Description = update.Descrption;
			tasks.IsFinished = update.IsFinsihed;
			context.SaveChanges();

			return Ok("Update");
		}
		[HttpDelete]
		[Authorize(Roles = "User")]
		public IActionResult DeletTask (string taskId)
		{
			var task  = context.Task.Find(taskId);
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
