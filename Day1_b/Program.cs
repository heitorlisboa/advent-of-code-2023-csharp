using System.Text.RegularExpressions;

string directory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.GetFullPath(
  Path.Combine(directory, "../../../input.txt")
);
string[] lines = File.ReadAllLines(filePath);

Regex numberRegex =
  new(@"(?=(one|two|three|four|five|six|seven|eight|nine|[1-9]))");

Dictionary<string, int> valuesOfNumbersSpelledWithLetters =
  new()
  {
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3,
    ["four"] = 4,
    ["five"] = 5,
    ["six"] = 6,
    ["seven"] = 7,
    ["eight"] = 8,
    ["nine"] = 9,
  };

int sumOfCalibrationValues = 0;

foreach (string line in lines)
{
  MatchCollection matchCollection = numberRegex.Matches(line);
  string firstDigitAsString = matchCollection.First().Groups[1].Value;
  int firstDigit = valuesOfNumbersSpelledWithLetters.ContainsKey(
    firstDigitAsString
  )
    ? valuesOfNumbersSpelledWithLetters[firstDigitAsString]
    : int.Parse(firstDigitAsString);

  string lastDigitAsString = matchCollection.Last().Groups[1].Value;
  int lastDigit = valuesOfNumbersSpelledWithLetters.ContainsKey(
    lastDigitAsString
  )
    ? valuesOfNumbersSpelledWithLetters[lastDigitAsString]
    : int.Parse(lastDigitAsString);

  int calibrationValue = int.Parse($"{firstDigit}{lastDigit}");
  sumOfCalibrationValues += calibrationValue;
}

Console.WriteLine(sumOfCalibrationValues.ToString());
