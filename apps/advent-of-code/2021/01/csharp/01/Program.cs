static IEnumerable<int> depthChanges(IEnumerable<int> sonarValues) =>
    sonarValues
        .Zip(sonarValues.Skip(1))
        .Select(it => it.Second - it.First);

static int depthIncreaseCount(IEnumerable<int> depthDifferences) =>
    depthDifferences.Count(it => it > 0);

var exampleValues = new[] {
    199,
    200,
    208,
    210,
    200,
    207,
    240,
    269,
    260,
    263
};

var sonarValues = File
    .ReadAllLines("../../input.txt")
    .Select(int.Parse);
var resultPart1 = depthIncreaseCount(depthChanges(sonarValues)); // 1374

Console.WriteLine(string.Join(Environment.NewLine, resultPart1));
