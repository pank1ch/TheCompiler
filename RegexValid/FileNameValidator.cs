using System.Text.RegularExpressions;

public class FileNameValidator
{
    public static List<string> GetDocType(string input)
    {
        string pattern = @"\b\w+\.(doc|docx|pdf)\b";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = regex.Matches(input);

        List<string> foundFiles = new List<string>();
        foreach (Match match in matches)
        {
            foundFiles.Add(match.Value + " ");
        }

        return foundFiles;
    }

    public static List<string> FindYears(string input)
    {
        string pattern = @"\b(1998|1999|2000|2001|2002|2003|2004)\b";
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(input);

        List<string> foundYears = new List<string>();
        foreach (Match match in matches)
        {
            foundYears.Add(match.Value);
        }

        return foundYears;
    }

    public static bool IsPasswordStrong(string password)
    {
        string pattern = @"^(?=.*[А-Я])(?=.*[а-я])(?=.*\d)(?=.*[#!?|/@/$%\^&*\-_]).{8,}$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(password);
    }
}