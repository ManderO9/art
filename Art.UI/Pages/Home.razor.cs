

namespace Art.UI;

public partial class Home
{
    private List<Image>? mImages;

    protected override async Task OnInitializedAsync()
    {
        mImages = await mApiManager.GetRecommendedImages();
    }

    private void LikeImage(Image image)
    {
        image.IsLiked = !image.IsLiked;
    }
    private void DownloadImage(Image image)
    {
    }
    private void ShareImage(Image image)
    {

    }


}