using System.Text.RegularExpressions;

string directory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.GetFullPath($"{directory}../../../input.txt");
string[] schematicGrid = File.ReadAllLines(filePath);

int getFullNumberFromAnyDigitIndex(string schematicRow, int column)
{
  string fullNumberAsString = schematicRow[column].ToString();
  int backwardIterator = column;
  int forwardIterator = column;

  while (backwardIterator > 0)
  {
    backwardIterator--;

    string character = schematicRow[backwardIterator].ToString();
    bool isDigit = int.TryParse(character, out _);
    if (!isDigit)
      break;

    fullNumberAsString = character + fullNumberAsString;
  }

  while (forwardIterator < schematicRow.Length - 1)
  {
    forwardIterator++;

    string character = schematicRow[forwardIterator].ToString();
    bool isDigit = int.TryParse(character, out _);
    if (!isDigit)
      break;

    fullNumberAsString = fullNumberAsString + character;
  }

  return int.Parse(fullNumberAsString);
}

int[] getAdjacentNumbers(int column, int row, string[] schematicGrid)
{
  Regex digitRegex = new(@"\d", RegexOptions.ECMAScript);
  int lastColumn = schematicGrid[row].Length - 1;
  int firstAdjacentColumn = column > 0 ? column - 1 : column;
  int lastAdjacentColumn = column < lastColumn ? column + 1 : column;
  List<int> adjacentNumbers = [];

  // Checking on the left
  if (digitRegex.IsMatch(schematicGrid[row][firstAdjacentColumn].ToString()))
  {
    int fullNumber = getFullNumberFromAnyDigitIndex(
      schematicGrid[row],
      firstAdjacentColumn
    );
    adjacentNumbers.Add(fullNumber);
  }

  // Checking on the left
  if (digitRegex.IsMatch(schematicGrid[row][lastAdjacentColumn].ToString()))
  {
    int fullNumber = getFullNumberFromAnyDigitIndex(
      schematicGrid[row],
      lastAdjacentColumn
    );
    adjacentNumbers.Add(fullNumber);
  }

  // Checking the row above
  Regex firstDigitRegex = new(@"(?<!\d)\d", RegexOptions.ECMAScript);
  if (row > 0)
  {
    string previousRow = schematicGrid[row - 1];
    string previousRowAdjacentSlice = previousRow[
      firstAdjacentColumn..(lastAdjacentColumn + 1)
    ];

    MatchCollection matches = firstDigitRegex.Matches(previousRowAdjacentSlice);
    foreach (Match match in matches.ToArray())
    {
      int fullNumber = getFullNumberFromAnyDigitIndex(
        previousRow,
        firstAdjacentColumn + match.Index
      );
      adjacentNumbers.Add(fullNumber);
    }
  }

  // Checking the row below
  int lastRow = schematicGrid.Length - 1;
  if (row < lastRow)
  {
    string nextRow = schematicGrid[row + 1];
    string nextRowAdjacentSlice = nextRow[
      firstAdjacentColumn..(lastAdjacentColumn + 1)
    ];

    MatchCollection matches = firstDigitRegex.Matches(nextRowAdjacentSlice);
    foreach (Match match in matches.ToArray())
    {
      int fullNumber = getFullNumberFromAnyDigitIndex(
        nextRow,
        firstAdjacentColumn + match.Index
      );
      adjacentNumbers.Add(fullNumber);
    }
  }

  return [..adjacentNumbers];
}

int sumOfGearRatios = 0;

foreach (
  var (rowIndex, schematicRow) in schematicGrid.Select(
    (value, index) => (index, value)
  )
)
{
  Regex starSymbolRegex = new(@"\*");
  MatchCollection matches = starSymbolRegex.Matches(schematicRow);
  foreach (Match match in matches.ToArray())
  {
    int[] numbersAdjacentToSymbol = getAdjacentNumbers(
      match.Index,
      rowIndex,
      schematicGrid
    );
    bool isGear = numbersAdjacentToSymbol.Length == 2;

    if (isGear)
    {
      int gearRatio = numbersAdjacentToSymbol[0] * numbersAdjacentToSymbol[1];
      sumOfGearRatios += gearRatio;
    }
  }
}

Console.WriteLine(sumOfGearRatios);
