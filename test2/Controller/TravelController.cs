using Microsoft.AspNetCore.Mvc;
using test2.Models;
using test2.Services;

namespace test2.Controller;


[Route("api/characters")]
[ApiController]
public class TravelController : ControllerBase
{
    private readonly ITravelService _dbService;

    public TravelController(ITravelService dbService)
    {
        _dbService = dbService;
    }


    [HttpGet]
    [Route("{characterId:int}")]
    public async Task<IActionResult> GetCharacter(int characterId)
    {
        

        var character = await _dbService.GetCharacter(characterId);
        if (character == null)
        {
            return NotFound($"Character with {characterId} not found");
        }
        var backpackItems = await _dbService.GetItemsCharacter(characterId);
        var titles = await _dbService.GetTitlesOfCharacter(characterId);

        var characterDto = new CharacterDto
        {
            FirstName = character.FirstName,
            LastName = character.LastName,
            CurrentWeight = character.CurrentWeight,
            MaxWeight = character.MaxWeight,
            BackpackItems = backpackItems,
            Titles = titles
        };
        return Ok(characterDto);
    }
    
}