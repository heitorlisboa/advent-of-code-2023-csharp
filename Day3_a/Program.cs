using System.Text.RegularExpressions;

string directory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.GetFullPath($"{directory}../../../input.txt");
string[] schematicGrid = File.ReadAllLines(filePath);

bool checkForAdjacentSymbol(
  int startingColumn,
  int endingColumn,
  int row,
  string[] schematicGrid
)
{
  Regex symbolRegex = new(@"[^0-9.]");
  int lastColumn = schematicGrid[row].Length - 1;
  int firstAdjacentColumn =
    startingColumn > 0 ? startingColumn - 1 : startingColumn;
  int lastAdjacentColumn =
    endingColumn < lastColumn ? endingColumn + 1 : endingColumn;

  // Checking on the left
  if (symbolRegex.IsMatch(schematicGrid[row][firstAdjacentColumn].ToString()))
  {
    return true;
  }

  // Checking on the left
  if (symbolRegex.IsMatch(schematicGrid[row][lastAdjacentColumn].ToString()))
  {
    return true;
  }

  // Checking on the row above
  if (row > 0)
  {
    string previousRow = schematicGrid[row - 1];
    string previousRowAdjacentSlice = previousRow[
      firstAdjacentColumn..(lastAdjacentColumn + 1)
    ];
    if (symbolRegex.IsMatch(previousRowAdjacentSlice))
    {
      return true;
    }
  }

  // Checking on the row below
  int lastRow = schematicGrid.Length - 1;
  if (row < lastRow)
  {
    string nextRow = schematicGrid[row + 1];
    string nextRowAdjacentSlice = nextRow[
      firstAdjacentColumn..(lastAdjacentColumn + 1)
    ];
    if (symbolRegex.IsMatch(nextRowAdjacentSlice))
    {
      return true;
    }
  }

  return false;
}

int sumOfEnginePartNumbers = 0;

foreach (
  var (rowIndex, schematicRow) in schematicGrid.Select(
    (value, index) => (index, value)
  )
)
{
  Regex numberRegex = new(@"\d+", RegexOptions.ECMAScript);
  MatchCollection matches = numberRegex.Matches(schematicRow);
  foreach (Match match in matches.ToArray())
  {
    int number = int.Parse(match.Value);
    int startingColumn = match.Index;
    int endingColumn = startingColumn + match.Length - 1;
    bool isPartNumber = checkForAdjacentSymbol(
      startingColumn,
      endingColumn,
      rowIndex,
      schematicGrid
    );

    if (isPartNumber)
    {
      sumOfEnginePartNumbers += number;
    }
  }
}

Console.WriteLine(sumOfEnginePartNumbers);
