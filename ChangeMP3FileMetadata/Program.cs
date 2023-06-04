namespace ChangeMP3FileMetadata
{
    internal abstract class Program
    {
        private static void Main()
        {
            string folderPath = @"C:\Users\Music";

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder path is invalid or does not exist.");
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.mp3");

            if (files.Length == 0)
            {
                Console.WriteLine("No MP3 files found in the folder.");
                return;
            }

            Console.WriteLine("Processing files...");

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    string filePath = files[i];
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    string correctedFileName = CorrectFileNameFormat(fileName);

                    string[] parts = correctedFileName.Split('-');

                    if (parts.Length != 2)
                    {
                        Console.WriteLine($"Invalid file name format: {Path.GetFileName(filePath)}");
                        continue;
                    }

                    string title = parts[0].Trim();
                    string artists = parts[1].Trim();

                    title = CorrectTitleFormat(title);

                    TagLib.File file = TagLib.File.Create(filePath);

                    // Set the properties
                    file.Tag.Title = title;
                    file.Tag.Performers = new[] { artists };
                    file.Tag.Album = "My Music";
                    file.Tag.Genres = new[] { "Rock" };
                    file.Tag.Track = (uint)(i + 1); // Assign the track number

                    // Save the changes
                    file.Save();

                    Console.WriteLine($"Modified properties of file: {Path.GetFileName(filePath)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error modifying properties of file: {Path.GetFileName(files[i])}");
                    Console.WriteLine($"Error details: {ex.Message}");
                }
            }

            Console.WriteLine("File properties modified successfully.");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string CorrectFileNameFormat(string fileName)
        {
            // Check if the first letter is uppercase
            if (!char.IsUpper(fileName[0]))
            {
                fileName = char.ToUpper(fileName[0]) + fileName.Substring(1);
            }

            bool isPrevSpace = true;
            for (int i = 1; i < fileName.Length; i++)
            {
                char currentChar = fileName[i];

                // Check if the character is uppercase when it's not the first letter
                if (char.IsUpper(currentChar) && !isPrevSpace)
                {
                    fileName = fileName.Substring(0, i) + char.ToLower(currentChar) + fileName.Substring(i + 1);
                }

                // Check if the character is lowercase when it's not the first letter
                if (char.IsLower(currentChar) && isPrevSpace)
                {
                    fileName = fileName.Substring(0, i) + char.ToUpper(currentChar) + fileName.Substring(i + 1);
                }

                // Update the flag to check for space in the next iteration
                isPrevSpace = currentChar == ' ';
            }

            return fileName;
        }

        private static string CorrectTitleFormat(string title)
        {
            // Check if the first letter is uppercase
            if (!char.IsUpper(title[0]))
            {
                title = char.ToUpper(title[0]) + title.Substring(1);
            }
            else
            {
                title = char.ToUpper(title[0]) + title.Substring(1).ToLower();
            }

            return title;
        }
    }
}
