using test2.Models;

namespace test2.Services;

public interface ITravelService
{
    Task<Characters?> GetCharacter(int characterId);
    Task<List<BackpackItemsDto>> GetItemsCharacter(int characterId);
    Task<List<TitleDto>> GetTitlesOfCharacter(int characterId);
    Task<int> GetTotalWeight(List<int> itemIdList);
    Task AddItemsCharacter(int characterId, List<int> itemIdList);
}