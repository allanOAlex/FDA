namespace TB.Mvc.Extensions
{
    public static class ClientExtensions
    {
        public static bool IsFileFormatMatch(string filePath, string format)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string expectedPrefix = "log";
            string expectedYear = DateTime.Now.Year.ToString();

            if (!fileName.StartsWith(expectedPrefix))
                return false;

            string fileDate = fileName.Substring(expectedPrefix.Length);

            var bolean = DateTime.TryParseExact(fileDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate) && parsedDate.Year.ToString() == expectedYear && fileName.Length == format.Length;

            return bolean;
        }


    }
}
