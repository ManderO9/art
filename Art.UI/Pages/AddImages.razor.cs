using Microsoft.AspNetCore.Components.Forms;
using System.Net.Mime;
using System.Text.Json;

namespace Art.UI;

public partial class AddImages
{

    #region Private Members

    /// <summary>
    /// The maximum file size that is allowed to be uploaded to the server, which is 1Mb
    /// </summary>
    private long mMaxFileSize = 1048_576;

    /// <summary>
    /// The list of images that we loaded in the page and want to upload
    /// </summary>
    private List<IBrowserFile> mPreviewImages = [];

    /// <summary>
    /// Bool flag that indicates whether the selected file has been uploaded or not yet
    /// </summary>
    private bool[] IsFileUploaded = [];

    /// <summary>
    /// The current batch of files to upload
    /// </summary>
    private List<PreviewImage> mCurrentBatch = [];

    /// <summary>
    /// The size of the batch to display/upload
    /// </summary>
    private int mBatchSize = 10;

    /// <summary>
    /// The index of the batch that we are currently displaying
    /// </summary>
    private int mBatchIndex = 0;

    /// <summary>
    /// The time from which the images will be set as "created at" when we upload them
    /// </summary>
    private DateTimeOffset mStartDate = DateTimeOffset.Now;

    /// <summary>
    /// The time to which the maximum "created at" date will be at
    /// </summary>
    /// <remarks>
    /// The "created at" date will be calculated for the current batch independently of the other batches we uploaded,
    /// the formula will be from the form: 
    /// <code>CreatedAt = ((mEndDate - mStartDate) * ImageIndexInBatch / BatchSize) + mStartDate</code>
    /// </remarks>
    private DateTimeOffset mEndDate = DateTimeOffset.Now;

    #endregion

    #region Private Helpers

    /// <summary>
    /// Gets called when we select some files using file input
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    /// <returns></returns>
    private async Task FileChanged(InputFileChangeEventArgs eventArgs)
    {
        // Get the images we selected
        mPreviewImages = eventArgs.GetMultipleFiles(eventArgs.FileCount).ToList();

        // Set the uploaded flag to false for all the files
        IsFileUploaded = new bool[mPreviewImages.Count];

        // Set batch index to the first one
        mBatchIndex = 0;

        // Load the first batch
        await LoadBatchAsync(mBatchIndex, mBatchSize);
    }

    /// <summary>
    /// Loads a batch of files and displays them to the user
    /// </summary>
    /// <param name="index">The index of the batch to load</param>
    /// <param name="size">The size of the batch</param>
    /// <returns></returns>
    private async Task LoadBatchAsync(int index, int size)
    {
        // Clear the current batch
        mCurrentBatch.Clear();

        // Get the files in this batch from the list of available files
        var files = mPreviewImages.Skip(index * size).Take(size);

        // For each file
        foreach(var file in files)
        {
            // Create data buffer
            byte[] data = [];

            // Whether we have read the file successfully or not
            var fileReadSuccessfully = false;

            // Only allow png and jpeg files
            if(file.ContentType == MediaTypeNames.Image.Jpeg || file.ContentType == MediaTypeNames.Image.Png)
            {
                // Check file size is less than maximum allowed size 
                if(file.Size <= mMaxFileSize)
                {
                    // Set flag for loaded successfully
                    fileReadSuccessfully = true;

                    // Create data buffer
                    data = new byte[file.Size];

                    // Read file data
                    await file.OpenReadStream(mMaxFileSize).ReadAsync(data);
                }
                // Otherwise, data size is too big
                else
                {
                    // Check if it's bigger than 10 times the allowed size, if it isn't
                    // Read file data for preview, but don't set flag for read successfully
                    if(file.Size < 10 * mMaxFileSize)
                    {
                        // Create data buffer
                        data = new byte[file.Size];

                        // Read file data
                        await file.OpenReadStream(10 * mMaxFileSize).ReadAsync(data);
                    }
                }
            }

            // Create image preview object
            var previewImage = new PreviewImage()
            {
                Index = mPreviewImages.IndexOf(file),
                ContentType = file.ContentType,
                FileName = file.Name,
                Size = file.Size,
                FileContent = data,
                FileSuccessfullyRead = fileReadSuccessfully,
                Base64EncodedContent = data.Length > 0 ? Convert.ToBase64String(data) : string.Empty,
            };

            // Add the image preview to the current batch
            mCurrentBatch.Add(previewImage);
        }

    }

    /// <summary>
    /// Uploads the current batch to the server
    /// </summary>
    private async Task UploadAsync()
    {
        // Create the list of images to upload
        var images = new List<Image>();

        // For each image in the current batch
        foreach(var image in mCurrentBatch)
        {
            // If the file has not been successfully read
            if(!image.FileSuccessfullyRead)
                // Skip it
                continue;

            // If it has been uploaded
            if(IsFileUploaded[image.Index])
                // Skip it
                continue;

            // Set is uploaded flag to true
            IsFileUploaded[image.Index] = true;

            // Add it to the list of images
            images.Add(new()
            {
                Id = Guid.NewGuid(),
                FileName = Guid.NewGuid().ToString() + '.' + image.FileName.Split('.').Last(),
                CreateAt = mStartDate.Add((mEndDate - mStartDate) * (mCurrentBatch.IndexOf(image) / (double)mCurrentBatch.Count)),
                FileContent = image.FileContent,
            });
        }

        // If there are any images to upload
        if(images.Count > 0)
        {
            
            // Upload them
            await mImageService.UploadImagesAsync(images);



            //foreach(var image in images)
            //    Console.WriteLine(
            //    $"""
            //    Uploaded file:
            //        id: {image.Id}
            //        filename: {image.FileName}
            //        file size: {image.FileContent!.Length}
            //        created at: {image.CreateAt}
            //    """);

            //var appData = await mDataAccessService.ReadFileAsync<AppData>("data.json");
            //appData.Images.AddRange(images);
            //Console.WriteLine($"Data Saved: {JsonSerializer.Serialize(appData)}");


        }
    }

    /// <summary>
    /// Displays the batch with preceding index
    /// </summary>
    /// <returns></returns>
    private async Task PreviousBatch()
    {
        // Decrement batch index
        mBatchIndex -= 1;

        // Limit it to zero
        if(mBatchIndex < 0)
        {
            mBatchIndex = 0;
            return;
        }

        // Load the batch with the specified index
        await LoadBatchAsync(mBatchIndex, mBatchSize);
    }

    /// <summary>
    /// Loads the batch the next index
    /// </summary>
    /// <returns></returns>
    private async Task NextBatch()
    {
        // Increment batch index
        mBatchIndex += 1;

        // Load the batch
        await LoadBatchAsync(mBatchIndex, mBatchSize);
    }

    /// <summary>
    /// Returns a human readable string representation of file size
    /// </summary>
    /// <param name="image">The image to get the file size for</param>
    /// <returns></returns>
    private string FileSize(PreviewImage image)
    {
        // If the size is less than 10 kb
        if(image.Size < 10 * 1024)
            // Return the size in kb
            return (image.Size / (double)(1024)).ToString("0.##") + "kb";

        // Otherwise, return the size in mb
        return (image.Size / (double)(1024 * 1024)).ToString("0.##") + "mb";
    }
    #endregion

    #region Preview Image class

    /// <summary>
    /// Preview image data that we will show for the user
    /// </summary>
    private class PreviewImage
    {
        /// <summary>
        /// The index of the image in the list of all images
        /// </summary>
        public required int Index { get; set; }

        /// <summary>
        /// The name of the original file
        /// </summary>
        public required string FileName { get; set; }

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        public required long Size { get; set; }

        /// <summary>
        /// The mime type of the file content, <see cref="MediaTypeNames"/>
        /// </summary>
        public required string ContentType { get; set; }

        /// <summary>
        /// The content of the file
        /// </summary>
        public required byte[] FileContent { get; set; }

        /// <summary>
        /// Base64 encoded file content for image preview, could be empty if file size too large, or not an image
        /// </summary>
        public required string Base64EncodedContent { get; set; }

        /// <summary>
        /// Whether we have read the file successfully or not
        /// </summary>
        public required bool FileSuccessfullyRead { get; set; }
    }

    #endregion

}