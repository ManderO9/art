using System.Text;
using System.Text.Json;

namespace Art.UI;

public partial class ImagesService : IImagesService
{
    #region Private Members

    private readonly IHistoryService mHistoryService;
    private readonly ILikeService mLikeService;
    private readonly IDataAccessService mDataAccessService;
    private const string mDataFilename = "data%2Fdata.json";

    #endregion

    #region Constructor

    public ImagesService(IHistoryService historyService, ILikeService likeService, IDataAccessService dataAccessService)
    {
        mHistoryService = historyService;
        mLikeService = likeService;
        mDataAccessService = dataAccessService;
    }

    #endregion

    #region Interface Implementation

    public async Task<List<Image>> GetRecommendedImagesAsync()
    {
        // Load images from server
        var list = await LoadImagesAsync();

        // Get history data
        var history = await mHistoryService.GetHistoryAsync();

        // Remove anything that is in history
        list = list.Where(image => !history.Any(h => h.ImageId == image.Id))
            // Order by newest to oldest
            .OrderByDescending(i => i.CreateAt).ToList();

        // Return the resulting list
        return list;
    }

    public async Task<List<Image>> GetRandomImagesAsync(int count)
    {
        // Load images from server
        var list = await LoadImagesAsync();

        // Create output list
        var output = new List<Image>();

        // Add count amount of random images to the output list
        for(int i = 0; i < count; i++)
            output.Add(list[Random.Shared.Next() % list.Count]);

        // Return the result
        return output;
    }

    public async Task<List<Image>> GetAllImagesAsync()
    {
        // Load images from server
        var list = await LoadImagesAsync();

        // Return an ordered list
        return list.OrderBy(x => x.CreateAt).ToList();
    }

    public async Task<List<Image>> GetLikedImagesAsync()
    {
        // Get the likes that the user did
        var likes = await mLikeService.GetLikedImagesAsync();

        // Load the images from the server
        var list = await LoadImagesAsync();

        // Only keep images that the user liked
        list = list.Join(likes, i => i.Id, l => l.ImageId, (i, l) => new { image = i, like = l })
            // Order by time of like
            .OrderByDescending(x => x.like.LikedAt).Select(x => x.image).ToList();

        // Return the result
        return list;
    }

    public async Task<List<Image>> GetImagesInHistoryAsync()
    {
        // Get the images in history
        var history = await mHistoryService.GetHistoryAsync();

        // Load the images from the server
        var list = await LoadImagesAsync();

        // Only keep images that are in history
        list = list.Join(history, i => i.Id, h => h.ImageId, (i, h) => new { image = i, history = h })
            // Order by time added to history
            .OrderByDescending(x => x.history.AddedAt).Select(x => x.image).ToList();

        // Return the result
        return list;
    }

    public async Task<Image> GetImageByIdAsync(Guid id)
        // Load images from server, and return the one with the matching id
        => (await LoadImagesAsync()).First(x => x.Id == id);

    public async Task UploadImagesAsync(List<Image> images)
    {
        // For each image
        foreach(var image in images)
            // Upload its content
            await mDataAccessService.WriteFileAsync(image.FileName, image.FileContent!);

        // Read data file
        var appData = await mDataAccessService.ReadFileAsync<AppData>(mDataFilename);

        // Add the list of uploaded images to it
        appData.Images.AddRange(images);

        // Serialize the data to json and get the bytes for the file to upload
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(appData));

        // Upload the data file
        await mDataAccessService.WriteFileAsync(mDataFilename, bytes);
    }

    public string GetImageUrl(Image image)
        => mDataAccessService.FilesUrl + "images%2F" + image.FileName + "?alt=media";

    #endregion

    #region Private Helpers

    private async Task<List<Image>> LoadImagesAsync()
    {
        // Read the data file
        var appData = await mDataAccessService.ReadFileAsync<AppData>(mDataFilename);

        // Only show images that we want the users to see, meaning created date is older than the current time
        var list = appData!.Images.Where(i => i.CreateAt <= DateTimeOffset.UtcNow).ToList();

        // Return the list of images
        return list;
    }

    #endregion

}
