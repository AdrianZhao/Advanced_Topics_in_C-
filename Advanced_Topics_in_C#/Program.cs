string textPath = @"C:\Users\10043\Downloads\theMachineStops.txt";

FileInfo textFile = new FileInfo(textPath);

string fullText = "";


try
{
    using (StreamReader reader = textFile.OpenText())
    {
        fullText = reader.ReadToEnd();
    }
    string outputPath = Path.Combine(Path.GetDirectoryName(textPath), "TelegramCopy.txt");

    File.WriteAllText(outputPath, Replace(fullText));
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

string Replace(string text)
{
    string modifiedContents = text.Replace(".", "STOP");

    return modifiedContents;
}