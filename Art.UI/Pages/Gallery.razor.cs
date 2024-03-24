namespace Art.UI;

public partial class Gallery
{
    #region Private Members

    private List<Image>? mImages;
    
    #endregion

    #region On initialized

    protected override async Task OnInitializedAsync()
    {
        // Get list of all images
        mImages = await mImageService.GetAllImagesAsync();
    }

    #endregion
}