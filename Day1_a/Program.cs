string directory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.GetFullPath(
  Path.Combine(directory, "../../../input.txt")
);
string[] lines = File.ReadAllLines(filePath);

int sumOfCalibrationValues = 0;

foreach (string line in lines)
{
  char? firstNumberAsChar = null;
  char? lastNumberAsChar = null;

  for (int startIndex = 0; startIndex < line.Length; startIndex++)
  {
    char character = line[startIndex];
    bool isDigit = int.TryParse(character.ToString(), out _);

    if (isDigit)
    {
      firstNumberAsChar = character;
      break;
    }
  }

  for (int endIndex = line.Length - 1; endIndex >= 0; endIndex--)
  {
    char character = line[endIndex];
    bool isDigit = int.TryParse(character.ToString(), out _);

    if (isDigit)
    {
      lastNumberAsChar = character;
      break;
    }
  }

  if (firstNumberAsChar is null || lastNumberAsChar is null)
    return;

  int calibrationValue = int.Parse($"{firstNumberAsChar}{lastNumberAsChar}");
  sumOfCalibrationValues += calibrationValue;
}

Console.WriteLine(sumOfCalibrationValues.ToString());
