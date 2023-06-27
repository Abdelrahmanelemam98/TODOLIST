using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using todolist.DTOS;

namespace todolist.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly RoleManager<IdentityRole> roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager) 
		{ 
		 this.roleManager = roleManager;
		}
		[HttpPost]
		public async Task<IActionResult> AddRole(AddRoleDto addRole)
		{
			if (ModelState.IsValid)
			{
				IdentityRole role = new IdentityRole() { Name = addRole.RoleName };
				IdentityResult identityResult = await roleManager.CreateAsync(role);
				if (identityResult.Succeeded)
				{
					return Ok();
				}
				
			}
			else
			{
				return BadRequest(ModelState);
			} 
			return BadRequest();
		}
	}
}
