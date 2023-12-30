string directory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.GetFullPath($"{directory}../../../input.txt");
string[] games = File.ReadAllLines(filePath);

(int gameId, (string cubeColor, int cubeCount)[][]) parseGame(string game)
{
  string[] gameTitleAndSetOfCubes = game.Split(": ");
  int gameId = int.Parse(gameTitleAndSetOfCubes[0].Split(" ")[1]);
  (string, int)[][] setOfCubes = gameTitleAndSetOfCubes[1]
    .Split("; ")
    .Select(subsetOfCubesAsString =>
    {
      (string, int)[] subsetOfCubes = subsetOfCubesAsString
        .Split(", ")
        .Select(
          (cubeCountAndColorAsString) =>
          {
            string[] cubeCountAndColor = cubeCountAndColorAsString.Split(" ");
            int cubeCount = int.Parse(cubeCountAndColor[0]);
            string cubeColor = cubeCountAndColor[1];
            return (cubeColor, cubeCount);
          }
        )
        .ToArray();
      return subsetOfCubes;
    })
    .ToArray();
  return (gameId, setOfCubes);
}

int sumOfCubeSetPowers = games.Aggregate(
  0,
  (accumulator, game) =>
  {
    var (_, setOfCubes) = parseGame(game);
    Dictionary<string, int> mininumNumberOfCubesInsideBag =
      new()
      {
        ["red"] = 0,
        ["green"] = 0,
        ["blue"] = 0
      };
    foreach (var subsetOfCubes in setOfCubes)
    {
      foreach (var (cubeColor, cubeCount) in subsetOfCubes)
      {
        if (cubeCount > mininumNumberOfCubesInsideBag[cubeColor])
        {
          mininumNumberOfCubesInsideBag[cubeColor] = cubeCount;
        }
      }
    }

    int cubeSetPower = mininumNumberOfCubesInsideBag
      .Values.ToArray()
      .Aggregate((previousValue, currentValue) => previousValue * currentValue);

    return accumulator + cubeSetPower;
  }
);

Console.WriteLine(sumOfCubeSetPowers);
