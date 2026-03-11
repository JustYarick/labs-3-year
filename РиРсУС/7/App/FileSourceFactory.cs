namespace RLCExamples01
{
    public static class FileSourceFactory
    {
        public static IFileSource Create(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();

            switch (ext)
            {
                case ".yaml":
                case ".yml":
                    return new YamlFileSource();

                case ".csv":
                    return new CsvFileSource();

                default:
                    throw new Exception("Unsupported file format");
            }
        }
    }
}
