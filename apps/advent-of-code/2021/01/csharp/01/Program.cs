var sonarValues = File
    .ReadAllLines("../../input.txt")
    .Select(int.Parse);

var resultPart1 = sonarValues.depthChanges().depthIncreaseCount();

Console.WriteLine(string.Join(Environment.NewLine, resultPart1));

public static class Extensions
{
    public static IEnumerable<int> depthChanges(this IEnumerable<int> sonarValues) =>
        sonarValues
            .Zip(sonarValues.Skip(1))
            .Select(it => it.Second - it.First);

    public static int depthIncreaseCount(this IEnumerable<int> depthDifferences) =>
        depthDifferences.Count(it => it > 0);
}
