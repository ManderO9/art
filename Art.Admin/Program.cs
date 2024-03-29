using Art.Core;
using System.Text;
using System.Text.Json;

var settingsFilePath = "D:\\programming\\ai-art\\media\\settings.json";

var options = new JsonSerializerOptions()
{
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
};

var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsFilePath), options)!;

UploadImages(settings);



void UploadImages(Settings settings)
{
    // Create the list of imagesToUpload to upload
    var imagesToUpload = new List<Image>();

    // Read all files in the passed in input folder
    var directoryFiles = Directory.GetFiles(settings.InputFolder);

    // Only keep files which are imagesToUpload
    var files = directoryFiles.Where(x => x.EndsWith(".jpeg") || x.EndsWith(".jpg") || x.EndsWith(".png"));

    // Get the number of files that we gonna upload
    var fileCount = (double)files.Count();

    // For each file
    foreach(var file in files.Select((name, index) => (name, index)))
    {
        // Create an image object and add it to the list of imagesToUpload
        imagesToUpload.Add(new()
        {
            Id = Guid.NewGuid(),
            FileName = Guid.NewGuid().ToString() + '.' + file.name.Split('.').Last(),
            CreateAt = settings.StartDate.Add((settings.EndDate - settings.StartDate) * (file.index / fileCount)),
            FilePath = file.name,
        });
    }

    // Directory to put images into
    var outDirectory = settings.OutputFolder + DateOnly.FromDateTime(DateTime.Now).ToString("O") + Path.DirectorySeparatorChar;
 
    // Check if the directory already exist
    if(!Directory.Exists(outDirectory))
        // Create it if it doesn't
        Directory.CreateDirectory(outDirectory);

    // Move all the files into the output directory 
    foreach(var image in imagesToUpload)
    {
        // Move the image to the new directory
        File.Move(image.FilePath, outDirectory + image.FileName);
    }

    // If the data file does not exist
    if(!File.Exists(settings.DataFilePath))
    {
        // Create it 
        var file = File.Create(settings.DataFilePath);

        // Write default data to it
        file.Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new AppData() { Images = [] })));
        
        // Close the file
        file.Close();
    }

    // Read data from data file
    var appData = JsonSerializer.Deserialize<AppData>(File.ReadAllText(settings.DataFilePath));

    // Add the newly created images to the data file
    appData!.Images.AddRange(imagesToUpload);

    // Save the data file
    File.WriteAllText(settings.DataFilePath, JsonSerializer.Serialize(appData));
}

public class Settings
{
    public required string InputFolder { get; set; }
    public required string OutputFolder { get; set; }
    public required string DataFilePath { get; set; }

    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;
}

