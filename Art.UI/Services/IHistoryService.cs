namespace Art.UI;

public interface IHistoryService
{
    Task AddImageToHistoryAsync(Guid imageId);
    Task<List<ImageInHistory>> GetHistoryAsync();

}
