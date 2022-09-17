using System.Collections.Concurrent;

namespace PdfTool.Extensions;

internal static class Extensions {
    /// <summary>
    /// Parallelizes func execution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">Collection of func's</param>
    /// <param name="degreeOfParallelization">essentially a batch count, if set to -1 it will be the amount of cpu cores</param>
    /// <param name="body"></param>
    /// <returns></returns>
    internal static Task ParallelForEachAsync<T>(this IEnumerable<T> source, int degreeOfParallelization, Func<T, Task> body) {
        if (degreeOfParallelization == -1) {
            degreeOfParallelization = Environment.ProcessorCount;
        }

        async Task AwaitPartition(IEnumerator<T> partition) {
            using (partition) {
                while (partition.MoveNext()) {
                    await body(partition.Current);
                }
            }
        }

        return Task.WhenAll(Partitioner
            .Create(source)
            .GetPartitions(degreeOfParallelization)
            .AsParallel()
            .Select(AwaitPartition));
    }
}
