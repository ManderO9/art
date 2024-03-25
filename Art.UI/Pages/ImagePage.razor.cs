
using Microsoft.AspNetCore.Components;

namespace Art.UI;

public partial class ImagePage
{
    #region Private Members

    private Image? mImage;

    #endregion

    #region Page Parameters

    [Parameter]
    public string? Id { get; set; }

    #endregion

    #region On initialized

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        // Get image data
        mImage = await mImageService.GetImageByIdAsync(Guid.Parse(Id!));

        // Get the list of liked images
        var likedImages = await mLikeService.GetLikedImagesAsync();

        // If the image was liked, set liked flag to true
        mImage.IsLiked = likedImages.Any(x => x.ImageId == mImage.Id);
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
        await mClipboardService.Copy(mNavigationManager.BaseUri + PageRoutes.ImageById(image.Id)[1..]);
        image.IsShared = true;
    }

    #endregion


}