using System.Globalization;

namespace KeyFortress.utils;

public class Utils
{
  /// <summary>
  /// Generates a complex password with the specified length. If no length is provided, the default length is 10.
  /// The password will contain at least one uppercase letter, one lowercase letter, one number, and one special character.
  /// </summary>
  /// <param name="length">Optional. The length of the generated password. Default is 10.</param>
  /// <returns>A complex password as a string.</returns>
  public static string GenerateStrongPassword(int length = 16)
  {
    const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    const string numericChars = "0123456789";
    const string specialChars = "!@#$%^&*()-_=+";

    var random = new Random();

    // Ensure at least one character from each category
    var passwordChars = new[]
    {
        uppercaseChars[random.Next(uppercaseChars.Length)],
        lowercaseChars[random.Next(lowercaseChars.Length)],
        numericChars[random.Next(numericChars.Length)],
        specialChars[random.Next(specialChars.Length)]
    }.ToList();

    // Fill remaining characters
    for (int i = passwordChars.Count; i < length; i++)
    {
      string category = random.Next(4) switch
      {
        0 => uppercaseChars,
        1 => lowercaseChars,
        2 => numericChars,
        _ => specialChars
      };
      passwordChars.Add(category[random.Next(category.Length)]);
    }

    // Shuffle characters to randomize
    for (int i = 0; i < passwordChars.Count; i++)
    {
      int swapIndex = random.Next(passwordChars.Count);
      (passwordChars[i], passwordChars[swapIndex]) = (passwordChars[swapIndex], passwordChars[i]);
    }

    return new string(passwordChars.ToArray());
  }

  /// <summary>
  /// Returns a human-readable string representation of the time difference between the specified input date 
  /// and the current date/time.
  /// </summary>
  /// <param name="inputDate">The DateTime value representing the date and time to compare against the current date/time.</param>
  /// <returns>A string indicating how long ago the input date occurred (e.g., "6 days ago", "yesterday", "today", "just now").</returns>
  public static string GetTimeAgo(DateTime inputDate)
  {
    TimeSpan timeDifference = DateTime.Now - inputDate;

    if (timeDifference.TotalDays >= 1)
    {
      if (timeDifference.TotalDays >= 2 && inputDate.Date != DateTime.Today.AddDays(-1))
      {
        return $"{(int)timeDifference.TotalDays} days ago";
      }
      else if (timeDifference.TotalDays >= 2 && inputDate.Date == DateTime.Today.AddDays(-1))
      {
        return "yesterday";
      }
      else
      {
        return "today";
      }
    }
    else if (timeDifference.TotalHours >= 1)
    {
      return $"{(int)timeDifference.TotalHours} hours ago";
    }
    else if (timeDifference.TotalMinutes >= 1)
    {
      return $"{(int)timeDifference.TotalMinutes} minutes ago";
    }
    else
    {
      return "just now";
    }
  }


  /// <summary>
  /// Capitalizes the first letter of each word in the specified sentence.
  /// </summary>
  /// <param name="sentence">The input sentence to capitalize.</param>
  /// <returns>A string with the first letter of each word capitalized.</returns>
  public static string CapitalizeEachWord(string sentence)
  {
    // Check for null or empty input
    if (string.IsNullOrEmpty(sentence))
    {
      return string.Empty;
    }

    // Create a TextInfo object to handle casing
    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

    // Capitalize the first letter of each word in the sentence
    return textInfo.ToTitleCase(sentence.ToLower());
  }


}
