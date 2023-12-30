string directory = AppDomain.CurrentDomain.BaseDirectory;
string filePath = Path.GetFullPath($"{directory}../../../input.txt");
string[] games = File.ReadAllLines(filePath);

Dictionary<string, int> numberOfCubesInsideBag =
  new()
  {
    ["red"] = 12,
    ["green"] = 13,
    ["blue"] = 14
  };

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

int sumOfIdsOfPossibleGames = games.Aggregate(
  0,
  (accumulator, game) =>
  {
    var (gameId, setOfCubes) = parseGame(game);

    foreach (var subsetOfCubes in setOfCubes)
    {
      foreach (var (cubeColor, cubeCount) in subsetOfCubes)
      {
        int numberOfCubesOfCurrentColorInsideBag = numberOfCubesInsideBag[
          cubeColor
        ];
        bool gameIsImpossible =
          cubeCount > numberOfCubesOfCurrentColorInsideBag;
        if (gameIsImpossible)
        {
          return accumulator;
        }
      }
    }

    return accumulator + gameId;
  }
);

Console.WriteLine(sumOfIdsOfPossibleGames);
