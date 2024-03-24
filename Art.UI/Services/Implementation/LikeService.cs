
namespace Art.UI;

/// <summary>
/// Service responsible for accessing and modifying images that the user has liked
/// </summary>
public class LikeService : ILikeService
{
    #region Private Members

    /// <summary>
    /// The key to use to access the liked images in the local storage  
    /// </summary>
    private const string mLikedImagesKey = "LikedImages";

    /// <summary>
    /// The local storage access service
    /// </summary>
    private readonly ILocalStorage mLocalStorage;

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="localStorage">The local storage service</param>
    public LikeService(ILocalStorage localStorage)
    {
        // Set private members
        mLocalStorage = localStorage;
    }

    #endregion

    #region Interface Implementation

    /// <inheritdoc />
    public async Task LikeImageAsync(Image image)
    {
        // Get the list of images from the local storage
        var likedImages = await mLocalStorage.GetValueAsync<List<ImageLike>>(mLikedImagesKey) ?? [];

        // Get an image with the same id
        var currentlyLikedImage = likedImages.FirstOrDefault(x => x.ImageId == image.Id);

        // If the image had already been liked
        if(currentlyLikedImage is not null)
            // Update the liked at time
            currentlyLikedImage.LikedAt = DateTimeOffset.UtcNow;
        // Otherwise...
        else
        {
            // Create a new object and add it to the list
            likedImages.Add(new()
            {
                LikedAt = DateTime.UtcNow,
                ImageId = image.Id,
            });
        }

        // Save the updated list
        await mLocalStorage.SetValueAsync(mLikedImagesKey, likedImages);
    }

    /// <inheritdoc />
    public async Task RemoveImageFromLikesAsync(Image image)
    {
        // Get the list of liked images from local storage
        var likedImages = await mLocalStorage.GetValueAsync<List<ImageLike>>(mLikedImagesKey) ?? [];

        // Remove the likes that have the passed in image id
        var removedCount = likedImages.RemoveAll(x => x.ImageId == image.Id);

        // If there was items that got removed
        if(removedCount > 0)
            // Save the result
            await mLocalStorage.SetValueAsync(mLikedImagesKey, likedImages);
    }

    /// <inheritdoc />
    public async Task<List<ImageLike>> GetLikedImagesAsync()
        // Return the list of images from local storage
        => await mLocalStorage.GetValueAsync<List<ImageLike>>(mLikedImagesKey) ?? [];

    /// <inheritdoc />
    public Task ClearLocalLikesAsync() => mLocalStorage.RemoveAsync(mLikedImagesKey);

    #endregion
}