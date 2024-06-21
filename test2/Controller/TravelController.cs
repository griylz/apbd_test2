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
        Characters character;
        try
        {
            character = await _dbService.GetCharacter(characterId);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
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

    [HttpPost]
    [Route("{characterId:int}/backpacks")]
    public async Task<IActionResult> AddItems(int characterId, List<int> itemIdList)
    {
        Characters character;
        try
        {
            character = await _dbService.GetCharacter(characterId);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound($"Character with {characterId} not found");
        }

        var totalWeight = await _dbService.GetTotalWeight(itemIdList);
        if (character.CurrentWeight + totalWeight > character.MaxWeight)
        {
            return BadRequest("Character cannot carry the added weight");
        }

        await _dbService.AddItemsCharacter(characterId, itemIdList);

        return Ok("Items added to character's backpack");
    }
    
}