using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Everplanner.WebApi.Controllers;
[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    [HttpGet("users/{userId}")]
    public IActionResult Get(int userId)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        if (foundUser is null)
        {
            return NotFound("Користувача не знайдено.");
        }

        return Ok(UserResponseModel.FromUser(foundUser));
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginRequestModel userLoginRequestModel)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Email == userLoginRequestModel.Email);
        if (foundUser == null || userLoginRequestModel.Password != foundUser.Password)
        {
            return NotFound("Невірна пошта і/або пароль користувача.");
        }

        return Ok(foundUser.Id);
    }

    [HttpPost("signup")]
    public IActionResult Post(CreateUserRequestModel createUserRequestModel)
    {
        if (InMemoryDatabase.Users.Any(u => u.Email == createUserRequestModel.Email))
        {
            return BadRequest("Користувач з такою електронною поштою уже існує.");
        }

        int newUserId = InMemoryDatabase.Users.Max(u => u.Id) + 1;
        var newUser = new User(newUserId, createUserRequestModel.Name, createUserRequestModel.Email,
            createUserRequestModel.Password, new List<Project>());
        InMemoryDatabase.Users.Add(newUser);
        return Ok();
    }
}
