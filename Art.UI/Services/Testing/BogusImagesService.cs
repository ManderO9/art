namespace Art.UI;

public class BogusImagesService : IImagesService
{
    private readonly IHistoryService mHistoryService;

    public BogusImagesService(IHistoryService historyService)
    {
        mHistoryService = historyService;
    }
    public async Task<List<Image>> GetRecommendedImagesAsync()
    {
        var list = new List<Image>()
        {
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
        };

        var history = await mHistoryService.GetHistoryAsync();

        // Remove anything that is in history
        list = list.Where(x => !history.Any(y => y.ImageId == x.Id)).ToList();
        return list;
    }

    public async Task<List<Image>> GetAllImagesAsync()
    {
        var list = await GetRecommendedImagesAsync();
        return list.OrderBy(x => x.CreateAt).ToList();
    }

    public async Task<List<Image>> GetLikedImagesAsync()
    {
        var list = await GetRecommendedImagesAsync();
        return list.OrderBy(x => x.CreateAt).ToList();
    }

    public async Task<List<Image>> GetImagesInHistoryAsync()
    {
        var history = await mHistoryService.GetHistoryAsync();
        var list = await GetRecommendedImagesAsync();
        //list = list.Join(history, i => i.Id, h => h.ImageId, (i, h) => new { image = i, history = h })
        //    .OrderBy(x => x.history.AddedAt).Select(x => x.image).ToList();

        return list;
    }

    public async Task<Image> GetImageByIdAsync(Guid id)
    {
        await Task.Delay(1);

        return new Image() { FileName = "1711294016761.jpeg", Id = id, CreateAt = DateTimeOffset.UtcNow };
    }


    public Task UploadImagesAsync(List<Image> images)
    {
        return Task.CompletedTask;
    }

}
