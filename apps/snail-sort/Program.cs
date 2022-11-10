IEnumerable<IEnumerable<T>> rotate<T>(IEnumerable<IEnumerable<T>> matrix)
    => matrix.Any()
        ? matrix.First()
            .Select((_, i) => matrix.Select(x => x.ElementAt(i)))
            .Reverse()
        : matrix;

IEnumerable<T> sort<T>(IEnumerable<IEnumerable<T>> matrix, IEnumerable<T> sorted)
{
    if (!matrix.Any()) return sorted;
    return sort(rotate(matrix.Skip(1)), sorted.Concat(matrix.First()));
}

var matrix = new[]{
    new [] {0, 1, 2},
    new [] {3, 4, 5},
    new [] {6, 7, 8},
    };

var flipped = sort(matrix, new int[] { });
flipped.ToList().ForEach(Console.Write);
