using Microsoft.AspNetCore.Mvc;
using RedisSample.Models;
using RedisSample.Services;
using System.Text.Json;

namespace RedisSample.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _profileService.Get(id.ToString());
        return Ok(JsonSerializer.Deserialize<Profile>(response));
    }
    [HttpPost]
    public async Task<IActionResult> CreateProfile(Profile profile)
    {
        await _profileService.Add(profile.Id.ToString(), JsonSerializer.Serialize(profile));
        return StatusCode(StatusCodes.Status201Created, profile.Id);
    }
}