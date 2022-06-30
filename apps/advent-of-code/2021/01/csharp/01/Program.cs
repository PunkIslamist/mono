var sonarValues = File
    .ReadAllLines("../../input.txt")
    .Select(int.Parse);

var resultPart1 = sonarValues.depthChanges().depthIncreaseCount();

Console.WriteLine(string.Join(Environment.NewLine, resultPart1));
(IterationTest(5).SlidingWindow(3))
    .Select(x => string.Join(',', x))
    .ToList()
    .ForEach(Console.WriteLine);

static IEnumerable<int> IterationTest(int count)
{
    for (int i = 0; i < count; i++)
    {
        Console.WriteLine($"Evaluating iterator for {i}");
        yield return i;
    }
}

public static class Extensions
{
    public static IEnumerable<int> depthChanges(this IEnumerable<int> sonarValues) =>
        sonarValues
            .Zip(sonarValues.Skip(1))
            .Select(it => it.Second - it.First);

    public static int depthIncreaseCount(this IEnumerable<int> depthDifferences) =>
        depthDifferences.Count(it => it > 0);

    public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> source, int width)
    {
        var enumerator = source.GetEnumerator();
        var buffer = new T[width];
        buffer[0] = default;
        for (int i = 1; i < width - 1; i++)
        {
            enumerator.MoveNext();
            buffer[i] = enumerator.Current;
        }

        while (enumerator.MoveNext())
        {
            var x = new T[width];
            Array.Copy(
                sourceArray: buffer,
                destinationArray: x,
                sourceIndex: 1,
                length: buffer.Length - 1,
                destinationIndex: 0);

            x[^1] = enumerator.Current;

            yield return x;

            buffer = x;
        }
    }
}
