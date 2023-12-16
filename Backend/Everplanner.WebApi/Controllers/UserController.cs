using Everplanner.WebApi.Core;
using Everplanner.WebApi.Data;
using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Everplanner.WebApi.Controllers;
[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly EverplannerDbContext _dbContext;

    public UserController(EverplannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> Get(int userId)
    {
        User? foundUser = await _dbContext.Users
            .Include(u => u.Projects)
                .ThenInclude(p => p.Tasks)
            .Include(u => u.Projects)
                .ThenInclude(p => p.Workers)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (foundUser is null)
        {
            return NotFound("Користувача не знайдено.");
        }

        return Ok(UserResponseModel.FromUser(foundUser));
    }

    [HttpGet("users/{userId}/name")]
    public async Task<IActionResult> GetName(int userId)
    {
        User? foundUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (foundUser is null)
        {
            return NotFound("Користувача не знайдено.");
        }

        return Ok(foundUser.Name);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequestModel userLoginRequestModel)
    {
        User? foundUser = await _dbContext.GetUserByEmailAsync(userLoginRequestModel.Email);
        if (foundUser == null || foundUser.Password != userLoginRequestModel.Password)
        {
            return NotFound("Невірна пошта і/або пароль користувача.");
        }

        return Ok(foundUser.Id);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Post(CreateUserRequestModel createUserRequestModel)
    {
        if (await _dbContext.IsUserWithEmailAsync(createUserRequestModel.Email))
        {
            return BadRequest("Користувач з такою електронною поштою уже існує.");
        }

        var newUser = new User(createUserRequestModel.Name, createUserRequestModel.Email, createUserRequestModel.Password);
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return Ok(newUser.Id);
    }
}
