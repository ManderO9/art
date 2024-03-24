namespace Art.UI;

public interface IImagesService
{
    Task<List<Image>> GetRecommendedImagesAsync();
    Task<List<Image>> GetAllImagesAsync();
    Task<List<Image>> GetLikedImagesAsync();

    
}