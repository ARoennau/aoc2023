namespace aoc2023.Utils
{
    public static class FileReaderUtils
    {
        public static async Task<string[]> ReadFile(string name)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Inputs", $"{name}.txt");
            if (filePath == null)
            {
                throw new Exception("Error getting file path");
            }
            return await File.ReadAllLinesAsync(filePath);
        }
    }
}
