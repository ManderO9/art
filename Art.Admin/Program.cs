using Art.Core;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;


var settingsFilePath = "D:\\programming\\ai-art\\media\\settings.json";

var options = new JsonSerializerOptions()
{
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
};

var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsFilePath), options)!;

Log($"Loaded settings from path: {settingsFilePath}");
Log($"    Data file path: {settings.DataFilePath}");
Log($"    Input folder: {settings.InputFolder}");
Log($"    Output folder: {settings.OutputFolder}");
Log($"    Start date: {settings.StartDate}");
Log($"    End date: {settings.EndDate}");

UploadImages(settings);


void UploadImages(Settings settings)
{
    // Create the list of imagesToUpload to upload
    var imagesToUpload = new List<Image>();

    // Read all files in the passed in input folder
    var directoryFiles = Directory.GetFiles(settings.InputFolder);

    Log($"Found {directoryFiles.Length} files in input folder.");

    // Only keep files which are imagesToUpload
    var files = directoryFiles.Where(x => x.EndsWith(".jpeg") || x.EndsWith(".jpg") || x.EndsWith(".png"));

    // List of files that we are gonna have a size less than the maximum allowed file size
    var filesUnderSizeLimit = new List<string>();

    // For each image file
    foreach(var file in files)
    {
        // Create file info
        var info = new FileInfo(file);

        // If it has a length greater than the allowed size
        if(info.Length > settings.MaxAllowedFileSize)
        {
            // Log an error
            Log($"file: {file} is too large, it will be ignored and not uploaded.", error: true);

        }
        // Otherwise...
        else
        {
            // Add it to the list of files to upload
            filesUnderSizeLimit.Add(file);
        }

    }

    // Set the list of files to upload to the accepted files
    files = filesUnderSizeLimit;

    // Get the number of files that we gonna upload
    var fileCount = (double)files.Count();

    Log($"Found {fileCount} images in input folder.");

    Log($"Creating image objects...");

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

    Log($"Finished creating image objects.");

    // Directory to put images into
    var outDirectory = settings.OutputFolder + DateOnly.FromDateTime(DateTime.Now).ToString("O") + Path.DirectorySeparatorChar;

    Log($"Directory to output files into: {outDirectory}");

    // Check if the directory already exist
    if(!Directory.Exists(outDirectory))
    {
        // Create it if it doesn't
        Directory.CreateDirectory(outDirectory);

        Log($"Directory created: {outDirectory}");
    }

    Log($"Moving images into output directory...");

    // Move all the files into the output directory 
    foreach(var image in imagesToUpload)
    {
        // Move the image to the new directory
        File.Move(image.FilePath, outDirectory + image.FileName);
    }

    Log($"Finished moving {imagesToUpload.Count} images to output directory.");


    // If the data file does not exist
    if(!File.Exists(settings.DataFilePath))
    {
        // Create it 
        var file = File.Create(settings.DataFilePath);

        // Write default data to it
        file.Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new AppData() { Images = [] })));

        // Close the file
        file.Close();

        Log($"Created new data file in location: {settings.DataFilePath}");
    }

    Log($"Reading data from data file: {settings.DataFilePath}");

    // Read data from data file
    var appData = JsonSerializer.Deserialize<AppData>(File.ReadAllText(settings.DataFilePath))!;

    Log($"Number of existing images: {appData.Images.Count}");
    Log($"Adding {imagesToUpload.Count} new images.");

    // Add the newly created images to the data file
    appData.Images.AddRange(imagesToUpload);

    Log($"Number of images to write to data file: {appData.Images.Count}");

    Log($"Saving updated data into data file...");

    // Save the data file
    File.WriteAllText(settings.DataFilePath, JsonSerializer.Serialize(appData));

    Log($"Finished saving data file.");

    Log($"--");
    Log($"--");

    Log($"Stats:");
    Log($"    Files Setup for upload: {fileCount}");
    Log($"    Total number of images: {appData.Images.Count}");

    Log($"To finish image upload do the following:");
    Log($"- upload all the files in out directory to google storage.");
    Log($"      out directory: {outDirectory}");
    Log($"      google storage: https://console.firebase.google.com/u/4/project/ai-art-f6bde/storage/ai-art-f6bde.appspot.com/files/~2Fimages");

    Log($"- upload data file to google storage.");
    Log($"      data file: {settings.DataFilePath}");
    Log($"      google storage: https://console.firebase.google.com/u/4/project/ai-art-f6bde/storage/ai-art-f6bde.appspot.com/files/~2Fdata");

    Log($"- run the following command to make files downloadable:");
    Log($"gsutil -m setmeta -h \"Content-Disposition: attachment\" gs://ai-art-f6bde.appspot.com/**");

    Log($"Program existing successfully.");
}

void Log(string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = "", bool error = false)
{
    if(error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[error] ");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[info] ");
    }
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(filePath.Split('\\').Last());
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(":");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write(lineNumber);
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(": ");
    Console.Write(message);
    Console.Write(Environment.NewLine);
}

public class Settings
{
    public required string InputFolder { get; set; }
    public required string OutputFolder { get; set; }
    public required string DataFilePath { get; set; }
    public required long MaxAllowedFileSize { get; set; }

    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;
}


