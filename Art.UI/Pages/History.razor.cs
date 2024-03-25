namespace Art.UI;

public partial class History
{
    #region Private Members

    /// <summary>
    /// The list of images we gonna display in the page
    /// </summary>
    private List<Image>? mImages;
    
    #endregion

    #region On initialized

    protected override async Task OnInitializedAsync()
    {
        // Get list of all liked images
        mImages = await mImageService.GetImagesInHistoryAsync();
    }

    #endregion
}