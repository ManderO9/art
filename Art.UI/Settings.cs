namespace Art.UI;

public class Settings
{
    /// <summary>
    /// The url to all images in we gonna display, should end with a backslash '/'
    /// </summary>
    private string mImagesUrl = default!;

    public Task Init(string imagesUrl)
    {
        mImagesUrl = imagesUrl;
        return Task.CompletedTask;
    }

    public string GetImageUrl(string fileName)
    {
        return mImagesUrl + fileName;
    }

}
