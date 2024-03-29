namespace Art.UI;

public interface IImagesService
{
    Task<List<Image>> GetRecommendedImagesAsync();
    Task<List<Image>> GetAllImagesAsync();
    Task<List<Image>> GetLikedImagesAsync();
    Task<List<Image>> GetImagesInHistoryAsync();
    Task<Image> GetImageByIdAsync(Guid id);
    Task UploadImagesAsync(List<Image> images);
    string GetImageUrl(Image image);
}