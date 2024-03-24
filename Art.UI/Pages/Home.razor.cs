namespace Art.UI;

public partial class Home
{
    #region Private Members

    private List<Image>? mImages;
    
    #endregion

    #region On initialized

    protected override async Task OnInitializedAsync()
    {
        // Get list of recommended images
        mImages = await mImageService.GetRecommendedImagesAsync();

        // Get the list of liked images
        var likedImages = await mLikeService.GetLikedImagesAsync();

        // Foreach image
        foreach(var image in mImages)
        {
            // Update if it was liked or not
            image.IsLiked = likedImages.Any(x => x.ImageId == image.Id);
        }
    }

    #endregion

    #region Private Helpers

    private async Task LikeImage(Image image)
    {
        // Update the liked flag for the image
        image.IsLiked = !image.IsLiked;
        
        // If it was liked
        if(image.IsLiked)
            // Save the like
            await mLikeService.LikeImageAsync(image);
        // Otherwise...
        else
            // Remove the like for this image
            await mLikeService.RemoveImageFromLikesAsync(image);
    }

    private async Task ShareImage(Image image)
    {
        // TODO: not implemented completely
        await mClipboardService.Copy(mNavigationManager.BaseUri + PageRoutes.ImageById(image.Id)[1..]);
        image.IsShared = true;
    }

    #endregion

}