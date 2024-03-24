namespace Art.UI;

public interface IApiManager
{
    public Task<List<Image>> GetRecommendedImages();
}