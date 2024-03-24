namespace Art.UI;

public class BogusImagesService : IImagesService
{
    public Task<List<Image>> GetRecommendedImagesAsync()
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

        return Task.FromResult(list);
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

}
