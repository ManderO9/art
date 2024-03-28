
namespace Art.UI;

public class PageRoutes
{
    public const string Home = "/";
    public const string Gallery = "/gallery";
    public const string Likes = "/likes";
    public const string History = "/history";
    public const string Image = "/image/{id}";
    public const string AddImages = "/create";
    

    public static string ImageById(Guid id)
        => Image.Replace("{id}", id.ToString());

}
