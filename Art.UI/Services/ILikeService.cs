namespace Art.UI;

/// <summary>
/// Service responsible for accessing and modifying images that the user has liked
/// </summary>
public interface ILikeService
{
    /// <summary>
    /// Get the list of images that the user has liked
    /// </summary>
    /// <returns></returns>
    Task<List<ImageLike>> GetLikedImagesAsync();

    /// <summary>
    /// Add an image to the list of liked images
    /// </summary>
    /// <param name="image">The image to add to the list</param>
    /// <returns></returns>
    Task LikeImageAsync(Image image);

    /// <summary>
    /// Remove the passed in image from the list of liked images
    /// </summary>
    /// <param name="image">The image to remove</param>
    /// <returns></returns>
    Task RemoveImageFromLikesAsync(Image image);

    /// <summary>
    /// Deletes all liked images in local storage and returns it into a clean state
    /// </summary>
    /// <returns></returns>
    Task ClearLocalLikesAsync();
}
