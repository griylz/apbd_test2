using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using test2.Context;
using test2.Models;

namespace test2.Services;

public class TravelService : ITravelService
{

    private readonly ApplicationContext _applicationContext;

    public TravelService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Characters?> GetCharacter(int characterId) 
    {
        
        Characters? character =
        await _applicationContext.Characters.FirstAsync(e => e.Id == characterId);
        if (character == null)
        {
            throw new KeyNotFoundException($"Character with {characterId} not found");
        }

        return character;
    }

    public async Task<List<BackpackItemsDto>> GetItemsCharacter(int characterId)
    {
        var items = await _applicationContext.Backpacks
            .Where(e => e.CharacterId == characterId)
            .Join(
                _applicationContext.Items,
                e => e.ItemId,
                i => i.Id,
                (e, i) => new BackpackItemsDto
                {
                    ItemName = i.Name,
                    ItemWeight = i.Weight,
                    Amount = e.Amount
                }
            ).ToListAsync();
        
        return items;
    }

    public async Task<List<TitleDto>> GetTitlesOfCharacter(int characterId)
    {
        var titles = await _applicationContext.CharacterTitles
            .Where(c => c.CharacterId == characterId)
            .Join(
                _applicationContext.Titles,
                c => c.TitleId,
                t => t.Id,
                (c, t) => new TitleDto
                {
                    Title = t.Name,
                    AcquiredAt = c.AcquiredAt
                }
            ).ToListAsync();

        return titles;
    }

    public async Task<int> GetTotalWeight(List<int> itemIdList)
    {
        return await _applicationContext.Items
            .Where(i => itemIdList.Contains(i.Id))
            .SumAsync(i => i.Weight);
    }

    public async Task AddItemsCharacter(int characterId, List<int> itemIdList)
    {
        var character = await _applicationContext.Characters.FindAsync(characterId);
        if (character == null) throw new ArgumentException("Character not found");

        var backpackItems = itemIdList.Select(itemId => new Backpacks
        {
            CharacterId = characterId,
            ItemId = itemId,
            Amount = 1
        }).ToList();

        _applicationContext.Backpacks.AddRange(backpackItems);

        var totalWeight = await GetTotalWeight(itemIdList);
   

        await _applicationContext.SaveChangesAsync();
    }
}