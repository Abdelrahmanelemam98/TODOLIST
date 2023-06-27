using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todolist.DTOS;
using todolist.Model;

namespace todolist.Controller
{
	[Route("api/[controller]")]
	[ApiController]

	public class AccountController : ControllerBase
	{
		private readonly UserManager<Users> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IConfiguration configuration;

		public AccountController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			this.userManager= userManager;
			this.roleManager= roleManager;
			this.configuration= configuration;
		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterDto registerDto)
		{
			if (ModelState.IsValid == true)
			{
				Users newuser = new Users();
				newuser.FirstName = registerDto.FirstName;
				newuser.LastName = registerDto.LastName;
				newuser.PhoneNumber = registerDto.Phone;
				newuser.Role = "User";
				newuser.UserName = registerDto.Email;
				newuser.ProfileImg = registerDto.ProfileImg;

				IdentityResult result = await userManager.CreateAsync(newuser, registerDto.Password);

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(newuser, "User");
					return Ok("Created");
				}
				else
				{
					return BadRequest(result.Errors);
				}
			}
			return BadRequest(ModelState);

		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDto newlogin)
		{
			if (ModelState.IsValid == true)
			{
				Users loginuser = await userManager.FindByNameAsync(newlogin.Email);
				if (loginuser != null && await userManager.CheckPasswordAsync(loginuser, newlogin.Password))
				{
					List<Claim> claims = new List<Claim>();
					claims.Add(new Claim(ClaimTypes.NameIdentifier, loginuser.Id));
					claims.Add(new Claim(ClaimTypes.Name, loginuser.UserName));
					claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

					var Roles = await userManager.GetRolesAsync(loginuser);
					if (Roles != null)
					{
						foreach (var item in Roles)
						{
							claims.Add(new Claim(ClaimTypes.Role, item));
						}
					}



					var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecrytKey"]));
					SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

					JwtSecurityToken Token = new JwtSecurityToken(
						issuer: configuration["Jwt:ValidIss"],
						audience: configuration["Jwt:ValidAud"],
						expires: DateTime.Now.AddHours(1),
						claims: claims,
						signingCredentials: credentials
					);
					return Ok(new
					{
						Token = new JwtSecurityTokenHandler().WriteToken(Token),
						expiration = Token.ValidTo,
					});

				}
				return BadRequest("Invalid Acount");
			}
			return BadRequest(ModelState);
		}
	}
}
